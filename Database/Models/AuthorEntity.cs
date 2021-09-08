using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("author")]
    public class AuthorEntity : Expansion
    {
        [Column("author_id")]
        public int Id { get; set; }

        [Required]
        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("middle_name")]
        public string MiddleName { get; set; }

        [Required]
        [Column("last_name")]
        public string LastName { get; set; }
    }
}
