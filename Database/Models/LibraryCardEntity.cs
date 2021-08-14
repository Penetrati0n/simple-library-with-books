using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("library_card")]
    public class LibraryCardEntity : LibraryCardExpansion
    {
        [Column("book_book_id")]
        public int BookId { get; set; }
        [Column("person_person_id")]
        public int PersonId { get; set; }

        public BookEntity Book { get; set; }
        public PersonEntity Person { get; set; }
    }
}
