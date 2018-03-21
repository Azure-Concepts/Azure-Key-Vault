using System;
using System.Collections.Generic;

namespace LMS.Shared.Model
{
    public class Author
    {
        public string Name { get; set; }
        public List<Book> Books { get; set; }
        public DateTime DoB { get; set; }
        public string Nationality { get; set; }
    }
}