using System;
using LMS.Shared.Spec;
using LMS.Shared.Model;
using System.Collections.Generic;

namespace LMS.Data.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryContext _libraryDbContext;
        private readonly IApplicationAuthorizationContext _applicationAuthorizationContext;

        public AuthorRepository(LibraryContext libraryDbContext, IApplicationAuthorizationContext applicationAuthorizationContext)
        {
            _libraryDbContext = libraryDbContext;
            _applicationAuthorizationContext = applicationAuthorizationContext;
        }

        public int AddAuthor(Author author)
        {
            var userName = _applicationAuthorizationContext.GetCurrentUserName();
            var currentDateTime = DateTime.UtcNow;

            var authorEntity = new DbModel.Author()
            {
                AuthorName = author.Name,
                Country = author.Nationality,
                CreatedBy = userName,
                CreatedOn = currentDateTime,
                ModifiedBy = userName,
                ModifiedOn = currentDateTime,
            };
            _libraryDbContext.Authors.Add(authorEntity);
            _libraryDbContext.SaveChanges();

            return authorEntity.AuthorId;
        }

        public Author GetAuthor(Book book)
        {
            throw new NotImplementedException();
        }

        public List<Book> GetBooks(Author author)
        {
            throw new NotImplementedException();
        }
    }
}