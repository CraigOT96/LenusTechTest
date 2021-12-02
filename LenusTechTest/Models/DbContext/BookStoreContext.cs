using LenusTechTest.Models;
using Microsoft.EntityFrameworkCore;

namespace LenusTechTest
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options)
            : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
    }
}
