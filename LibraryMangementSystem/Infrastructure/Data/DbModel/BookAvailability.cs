using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data.DbModel
{
    public class BookAvailability: BaseDbModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookAvailabilityId { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
        public int AvailableCopies { get; set; }
    }
}