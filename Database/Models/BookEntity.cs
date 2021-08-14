using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("book")]
    public class BookEntity : Expansion
    {
        [Column("book_id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("author_id")]
        public int AuthorId { get; set; }
        public AuthorEntity Author { get; set; }

        public ICollection<GenreEntity> Genres { get; set; }

        public ICollection<LibraryCardEntity> LibraryCards { get; set; }

        public BookEntity()
        {
            Genres = new List<GenreEntity>();
            LibraryCards = new List<LibraryCardEntity>();
        }
    }
}
