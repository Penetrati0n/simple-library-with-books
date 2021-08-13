using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<AuthorEntity> Authors { get; set; }
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }
        public DbSet<PersonEntity> People { get; set; }

        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
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

            modelBuilder.Entity("BookEntityPersonEntity", b =>
            {
                b.Property<int>("BooksId")
                    .HasColumnType("integer")
                    .HasColumnName("book_book_id");

                b.Property<int>("PeopleId")
                    .HasColumnType("integer")
                    .HasColumnName("person_person_id");

                b.HasKey("BooksId", "PeopleId");

                b.HasIndex("PeopleId");

                b.ToTable("library_card");
            });
        }
    }
}
