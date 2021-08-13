using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleLibraryWithBooks.Models
{
    public class Person
    {
        public class Request
        {
            public class Create
            {
                [Required]
                [MaxLength(100)]
                [RegularExpression("^([A-ZÀ-ÿА-ЯЁ][-,a-zа-яё. ']+[ ]*)+$",
                    ErrorMessage = "The First Name field contains invalid characters.")]
                public string FirstName { get; set; }

                [Required]
                [MaxLength(100)]
                [RegularExpression("^([A-ZÀ-ÿА-ЯЁ][-,a-zа-яё. ']+[ ]*)+$",
                    ErrorMessage = "The Patronymic field contains invalid characters.")]
                public string MiddleName { get; set; }

                [Required]
                [MaxLength(100)]
                [RegularExpression("^([A-ZÀ-ÿА-ЯЁ][-,a-zа-яё. ']+[ ]*)+$",
                    ErrorMessage = "The Last Name field contains invalid characters.")]
                public string LastName { get; set; }

                [Required]
                [DataType(DataType.Date)]
                public DateTimeOffset Birthday { get; set; }
            }

            public class Update
            {
                [Required]
                public int Id { get; set; }

                [Required]
                [MaxLength(100)]
                [RegularExpression("^([A-ZÀ-ÿА-ЯЁ][-,a-zа-яё. ']+[ ]*)+$",
                    ErrorMessage = "The First Name field contains invalid characters.")]
                public string FirstName { get; set; }

                [Required]
                [MaxLength(100)]
                [RegularExpression("^([A-ZÀ-ÿА-ЯЁ][-,a-zа-яё. ']+[ ]*)+$",
                    ErrorMessage = "The Patronymic field contains invalid characters.")]
                public string MiddleName { get; set; }

                [Required]
                [MaxLength(100)]
                [RegularExpression("^([A-ZÀ-ÿА-ЯЁ][-,a-zа-яё. ']+[ ]*)+$",
                    ErrorMessage = "The Last Name field contains invalid characters.")]
                public string LastName { get; set; }

                [Required]
                [DataType(DataType.Date)]
                public DateTimeOffset Birthday { get; set; }
            }
        }

        public class Response
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public DateTimeOffset Birthday { get; set; }
        }
    }
}
