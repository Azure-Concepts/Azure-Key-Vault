using LMS.Data.DbModel;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) {  }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAvailability> BookAvailabilities { get; set; }
        public DbSet<BookCheckout> BookCheckouts { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{ 
        //}
    }
}