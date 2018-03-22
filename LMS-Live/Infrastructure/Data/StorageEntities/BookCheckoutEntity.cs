using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace LMS.Data.StorageEntities
{
    public class BookCheckoutEntity: TableEntity
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string UserAlias { get; set; }
        public DateTime CheckoutOn { get; set; }
        public DateTime ReturnOn { get; set; }
    }
}