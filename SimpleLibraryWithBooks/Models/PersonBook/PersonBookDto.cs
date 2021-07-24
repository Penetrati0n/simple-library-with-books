using System;
using SimpleLibraryWithBooks.Models.Book;
using SimpleLibraryWithBooks.Models.Person;
using System.ComponentModel.DataAnnotations;

namespace SimpleLibraryWithBooks.Models.PersonBook
{
    public class PersonBookDto
    {
        [Required]
        public PersonDto Person { get; set; }

        [Required]
        public BookDto Book { get; set; }

        [Required]
        public DateTimeOffset DateTimeReceipt { get; set; }
    }
}
