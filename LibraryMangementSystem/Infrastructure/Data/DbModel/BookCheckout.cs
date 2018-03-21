using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data.DbModel
{
    public class BookCheckout : BaseDbModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookCheckoutId { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
        public string CheckoutUser { get; set; }
        public DateTime CheckoutOn { get; set; }
        public DateTime DueDate { get; set; }
    }
}