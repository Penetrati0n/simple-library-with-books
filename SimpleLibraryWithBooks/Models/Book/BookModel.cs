using System.ComponentModel.DataAnnotations;

namespace SimpleLibraryWithBooks.Models.Book
{
    public class BookModel
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
    }
}
