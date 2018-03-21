using System;
using System.Linq;
using LMS.Shared.Spec;
using LMS.Shared.Model;
using LMS.Data.StorageEntities;
using System.Collections.Generic;

namespace LMS.Data.Repository
{
    public class BookRepository: IBookRespository
    {
        private readonly LibraryContext _libraryDbContext;
        private readonly ITableService<BookCheckoutEntity> _tableService;
        private readonly IApplicationAuthorizationContext _applicationAuthorizationContext;

        public BookRepository(LibraryContext libraryDbContext, ITableService<BookCheckoutEntity> tableService, IApplicationAuthorizationContext applicationAuthorizationContext)
        {
            _libraryDbContext = libraryDbContext;
            _tableService = tableService;
            _applicationAuthorizationContext = applicationAuthorizationContext;
        }

        public int AddBook(Book book)
        {
            var userName = _applicationAuthorizationContext.GetCurrentUserName();
            var currentDateTime = DateTime.UtcNow;
            var bookDbEntity = new DbModel.Book()
            {
                BookName = book.Title,
                ISBN = book.ISBN,
                Publisher = book.PublisherName,
                CreatedBy = userName,
                CreatedOn = currentDateTime,
                ModifiedBy = userName,
                ModifiedOn = currentDateTime,
            };

            var currentAuthor = _libraryDbContext.Authors.FirstOrDefault(author => author.AuthorName == book.Author);
            if (currentAuthor == null)
            {
                var authorDbEntity = new DbModel.Author()
                {
                    AuthorName = book.Author,
                    CreatedBy = userName,
                    CreatedOn = currentDateTime,
                    ModifiedBy = userName,
                    ModifiedOn = currentDateTime,
                };
                currentAuthor = _libraryDbContext.Authors.Add(authorDbEntity).Entity;
                _libraryDbContext.SaveChanges();
            }

            bookDbEntity.Author = currentAuthor;
            bookDbEntity.AuthorId = currentAuthor.AuthorId;
            var entity = _libraryDbContext.Books.Add(bookDbEntity);
            _libraryDbContext.SaveChanges();

            var bookAvailableEntity = new DbModel.BookAvailability()
            {
                Book = entity.Entity,
                BookId = entity.Entity.BookId,
                AvailableCopies = book.AvailableUnits,
                CreatedBy = userName,
                CreatedOn = currentDateTime,
                ModifiedBy = userName,
                ModifiedOn = currentDateTime,
            };

            _libraryDbContext.BookAvailabilities.Add(bookAvailableEntity);
            _libraryDbContext.SaveChanges();
            return entity.Entity.BookId;
        }

        public int CheckoutBook(Book book, User user)
        {
            var userName = _applicationAuthorizationContext.GetCurrentUserName();
            var currentDateTime = DateTime.UtcNow;
            var dueDate = currentDateTime.AddDays(7).Date;

            var bookDbEntity = _libraryDbContext.Books.FirstOrDefault(bk => bk.BookId == book.Id);
            if (bookDbEntity == null)
                throw new Exception("Invalid Book entered");

            var bookAvailabilityEntity = _libraryDbContext.BookAvailabilities.FirstOrDefault(bk => bk.BookId == bk.BookId);
            if (bookAvailabilityEntity == null || bookAvailabilityEntity.AvailableCopies < 1)
                throw new Exception("Not enough books available");

            bookAvailabilityEntity.AvailableCopies--;
            bookAvailabilityEntity.ModifiedBy = userName;
            bookAvailabilityEntity.ModifiedOn = currentDateTime;

            _libraryDbContext.Update(bookAvailabilityEntity);

            var bookCheckoutEntity = new DbModel.BookCheckout()
            {
                Book = bookDbEntity,
                BookId = book.Id,
                CheckoutOn = currentDateTime.Date,
                CheckoutUser = user.Name,
                CreatedBy = userName,
                CreatedOn = currentDateTime,
                DueDate = dueDate,
                ModifiedBy = userName,
                ModifiedOn = currentDateTime
            };
            _libraryDbContext.BookCheckouts.Add(bookCheckoutEntity);

            _libraryDbContext.SaveChanges();

            var bookCheckoutEvent = new BookCheckoutEntity()
            {
                PartitionKey = book.Title,
                RowKey = $"{user.Name}-{bookCheckoutEntity.CheckoutOn.ToShortDateString()}",
                BookId = book.Id,
                BookName = book.Title,
                UserAlias = user.Name,
                CheckoutOn = currentDateTime,
                ReturnOn = new DateTime(9999, 12, 31)
            };
            _tableService.UpsertAsync(bookCheckoutEvent);

            return bookAvailabilityEntity.AvailableCopies;
        }

        public IEnumerable<Book> GetAllAvailableBooks()
        {
            var availableBookEntities = _libraryDbContext.BookAvailabilities
                .Where(ba => ba.AvailableCopies > 0)
                .Select(ba => ba.Book)
                .ToList();

            return availableBookEntities.Select(bookEntity =>
                new Book()
                {
                    Id = bookEntity.BookId,
                    ISBN = bookEntity.ISBN,
                    Author = bookEntity.Author.AuthorName,
                    Title = bookEntity.BookName,
                    PublisherName = bookEntity.Publisher
                }
            ).ToList();
        }

        public Book GetBook(string bookTitle)
        {
            var bookEntity = _libraryDbContext.Books.FirstOrDefault(book => book.BookName == bookTitle);

            if (bookEntity == null)
                return null;

            return new Book()
            {
                Id = bookEntity.BookId,
                ISBN = bookEntity.ISBN,
                Author = bookEntity.Author.AuthorName,
                Title = bookEntity.BookName,
                PublisherName = bookEntity.Publisher
            };
        }
        
        public int ReturnBook(Book book, User user)
        {
            var userName = _applicationAuthorizationContext.GetCurrentUserName();
            var currentDateTime = DateTime.UtcNow;
            var dueDate = currentDateTime.AddDays(7);

            var bookDbEntity = _libraryDbContext.Books.FirstOrDefault(bk => bk.BookId == book.Id);
            if (bookDbEntity == null)
                throw new Exception("Invalid Book entered");

            var bookCheckoutEntity = _libraryDbContext.BookCheckouts.FirstOrDefault(bchk => bchk.BookId == book.Id && bchk.CheckoutUser == user.Name);
            if (bookCheckoutEntity == null)
                throw new Exception("No checkout history found");
            var checkoutDate = bookCheckoutEntity.CheckoutOn.Date;
            if (checkoutDate == currentDateTime.Date)
                throw new Exception("Book cannot be returned on the same date");
            _libraryDbContext.BookCheckouts.Remove(bookCheckoutEntity);

            var bookAvailabilityEntity = _libraryDbContext.BookAvailabilities.FirstOrDefault(bk => bk.BookId == bk.BookId);
            bookAvailabilityEntity.AvailableCopies++;
            bookAvailabilityEntity.ModifiedBy = userName;
            bookAvailabilityEntity.ModifiedOn = currentDateTime;
            _libraryDbContext.Update(bookAvailabilityEntity);

            _libraryDbContext.SaveChanges();

            var bookCheckoutEvent = new BookCheckoutEntity()
            {
                PartitionKey = book.Title,
                RowKey = $"{user.Name}-{checkoutDate.ToShortDateString()}",
                BookId = book.Id,
                BookName = book.Title,
                UserAlias = user.Name,
                CheckoutOn = checkoutDate,
                ReturnOn = currentDateTime
            };
            _tableService.UpsertAsync(bookCheckoutEvent);

            return bookAvailabilityEntity.AvailableCopies;
        }
    }
}