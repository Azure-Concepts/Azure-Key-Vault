using LMS.Shared.Model;
using System.Collections.Generic;

namespace LMS.Shared.Spec
{
    public interface IBookRespository
    {
        Book GetBook(string bookTitle);
        IEnumerable<Book> GetAllAvailableBooks();
        int AddBook(Book book);
        int CheckoutBook(Book book, User user);
        int ReturnBook(Book book, User user);
    }
}