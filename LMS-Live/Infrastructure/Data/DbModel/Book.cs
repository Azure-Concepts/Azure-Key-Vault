using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data.DbModel
{
    public class Book: BaseDbModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }
        public string ISBN { get; set; }
        public string BookName { get; set; }
        public string Publisher { get; set; }
        public Author Author { get; set; }
        public int AuthorId { get; set; }
    }
}