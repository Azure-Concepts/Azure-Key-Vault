using LMS.Shared.Model;
using LMS.Shared.Spec;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Controllers
{
    [Route("api/books")]
    public class BooksController: Controller
    {
        private readonly IBookRespository _bookRepository;
        public BooksController(IBookRespository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public List<Book> GetBooks()
        {
            return _bookRepository.GetAllBooks().ToList();
        }

        [HttpGet]
        [Route("{title}")]
        public Book GetBookDetails([FromRoute]string title)
        {
            return _bookRepository.GetBook(title);
        }

        [HttpGet]
        [Route("{title}/checkouts")]
        public Book GetCheckouts([FromRoute]string title)
        {
            var book = new Book()
            {
                Title = title
            };
            return _bookRepository.GetCheckoutHistory(book);
        }
    }
}