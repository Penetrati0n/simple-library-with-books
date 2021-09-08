using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("dim_genre")]
    public class GenreEntity : Expansion
    {
        [Column("genre_id")]
        public int Id { get; set; }

        [Required]
        [Column("genre_name")]
        public string Name { get; set; }

        public ICollection<BookEntity> Books { get; set; }

        public GenreEntity()
        {
            Books = new List<BookEntity>();
        }
    }
}
