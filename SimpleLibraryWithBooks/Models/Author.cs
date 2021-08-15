using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleLibraryWithBooks.Models
{
    public class Author
    {
        public class Request
        {
            public class Create
            {
                [Required]
                [MaxLength(100)]
                [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                    ErrorMessage = "The First Name field contains invalid characters.")]
                public string FirstName { get; set; }

                [MaxLength(100)]
                [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                    ErrorMessage = "The First Name field contains invalid characters.")]
                public string MiddleName { get; set; }

                [Required]
                [MaxLength(100)]
                [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                    ErrorMessage = "The First Name field contains invalid characters.")]
                public string LastName { get; set; }

                public class WithBooks
                {
                    [Required]
                    [MaxLength(100)]
                    [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                        ErrorMessage = "The First Name field contains invalid characters.")]
                    public string FirstName { get; set; }

                    [MaxLength(100)]
                    [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                        ErrorMessage = "The First Name field contains invalid characters.")]
                    public string MiddleName { get; set; }

                    [Required]
                    [MaxLength(100)]
                    [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                        ErrorMessage = "The First Name field contains invalid characters.")]
                    public string LastName { get; set; }

                    [MaxLength(100)]
                    public IEnumerable<Book.Request.Create.WithoutAutor> Books { get; set; }

                    public WithBooks()
                    {
                        Books = new List<Book.Request.Create.WithoutAutor>();
                    }
                }
            }

            public class Update
            {
                [Required]
                public int Id { get; set; }

                [Required]
                [MaxLength(100)]
                [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                    ErrorMessage = "The First Name field contains invalid characters.")]
                public string FirstName { get; set; }

                [MaxLength(100)]
                [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                    ErrorMessage = "The First Name field contains invalid characters.")]
                public string MiddleName { get; set; }

                [Required]
                [MaxLength(100)]
                [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                    ErrorMessage = "The First Name field contains invalid characters.")]
                public string LastName { get; set; }
            }

            public class FilterForBooks
            {
                [MaxLength(100)]
                [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                    ErrorMessage = "The First Name field contains invalid characters.")]
                public string FirstName { get; set; }

                [MaxLength(100)]
                [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                    ErrorMessage = "The First Name field contains invalid characters.")]
                public string MiddleName { get; set; }

                [MaxLength(100)]
                [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                    ErrorMessage = "The First Name field contains invalid characters.")]
                public string LastName { get; set; }
            }
        }

        public class Response
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }

            public class WithBooks
            {
                public int Id { get; set; }
                public string FirstName { get; set; }
                public string MiddleName { get; set; }
                public string LastName { get; set; }
                public IEnumerable<Book.Response.Without.AuthorAndPeople> Books { get; set; }
                public WithBooks()
                {
                    Books = new List<Book.Response.Without.AuthorAndPeople>();
                }

                public class WithoutAll
                {
                    public int Id { get; set; }
                    public string FirstName { get; set; }
                    public string MiddleName { get; set; }
                    public string LastName { get; set; }
                    public IEnumerable<Book.Response.Without.All> Books { get; set; }
                    public WithoutAll()
                    {
                        Books = new List<Book.Response.Without.All>();
                    }
                }
            }
        }
    }
}
