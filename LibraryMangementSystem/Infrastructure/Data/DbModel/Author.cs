using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data.DbModel
{
    public class Author: BaseDbModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }

        public List<Book> Books { get; set; }
    }
}