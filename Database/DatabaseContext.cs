using Database.Models;
using Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DbSet<AuthorEntity> Authors { get; set; }
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }
        public DbSet<PersonEntity> People { get; set; }
        public DbSet<LibraryCardEntity> LibraryCards { get; set; }

        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                Database.MigrateAsync().Wait();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity("BookEntityGenreEntity", b =>
            {
                b.Property<int>("BooksId")
                    .HasColumnType("integer")
                    .HasColumnName("book_id");

                b.Property<int>("GenresId")
                    .HasColumnType("integer")
                    .HasColumnName("genre_id");

                b.HasKey("BooksId", "GenresId");

                b.HasIndex("GenresId");

                b.ToTable("book_genre_lnk");
            });

            modelBuilder.Entity<LibraryCardEntity>()
                .HasKey(e => new { e.BookId, e.PersonId });
        }
    }
}
