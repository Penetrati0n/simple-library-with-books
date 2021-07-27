using System;
using SimpleLibraryWithBooks.Models.Book;
using SimpleLibraryWithBooks.Models.Person;
using System.ComponentModel.DataAnnotations;

namespace SimpleLibraryWithBooks.Models.PersonBook
{
    public class PersonBookRequestDto
    {
        [Required]
        public PersonRequestDto Person { get; set; }

        [Required]
        public BookRequestDto Book { get; set; }

        [Required]
        public DateTimeOffset DateTimeReceipt { get; set; }
    }
}
