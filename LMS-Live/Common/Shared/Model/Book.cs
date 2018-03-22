using System;

namespace LMS.Shared.Model
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string PublisherName { get; set; }
        public string Author { get; set; }
        public int AvailableUnits { get; set; }
        public User[] CheckoutBy { get; set; }
        public CheckoutRecord[] CheckoutHistory;
    }

    public class CheckoutRecord
    {
        public DateTime CheckoutOn { get; set; }
        public User CheckoutBy { get; set; }
        public DateTime ReturnedOn { get; set; }
    }
}