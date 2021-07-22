using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleLibraryWithBooks.Models.Person
{
    public class PersonModel : IEquatable<PersonModel>
    {
        [Required]
        [MaxLength(100)]
        [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
            ErrorMessage = "The Last Name field contains invalid characters.")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
            ErrorMessage = "The First Name field contains invalid characters.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
            ErrorMessage = "The Patronymic field contains invalid characters.")]
        public string Patronymic { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTimeOffset Birthday { get; set; }

        public bool Equals(PersonModel other) =>
            this.LastName == other.LastName &&
            this.FirstName == other.FirstName &&
            this.Patronymic == other.Patronymic &&
            this.Birthday == other.Birthday;
    }
}
