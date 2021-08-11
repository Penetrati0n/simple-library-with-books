using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<PersonEntity> People { get; set; }
        public DbSet<PersonBookEntity> PersonBooks { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
