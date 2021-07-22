using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleLibraryWithBooks.Models.Book
{
    public class BookModel : IEquatable<BookModel>
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        
        [Required]
        [MaxLength(100)]
        [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
            ErrorMessage = "The Author field contains invalid characters.")]
        public string Author { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Genre { get; set; }

        public bool Equals(BookModel other) => (this.Title, this.Author, this.Genre) == (other.Title, other.Author, other.Genre);
    }
}
