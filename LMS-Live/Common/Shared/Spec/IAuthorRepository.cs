using LMS.Shared.Model;
using System.Collections.Generic;

namespace LMS.Shared.Spec
{
    public interface IAuthorRepository
    {
        Author GetAuthor(Book book);
        List<Book> GetBooks(Author author);
        int AddAuthor(Author author);
    }
}