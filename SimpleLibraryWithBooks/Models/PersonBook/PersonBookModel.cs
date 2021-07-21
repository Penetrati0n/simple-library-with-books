using System;
using SimpleLibraryWithBooks.Models.Book;
using SimpleLibraryWithBooks.Models.Person;
using System.ComponentModel.DataAnnotations;

namespace SimpleLibraryWithBooks.Models.PersonBook
{
    public class PersonBookModel
    {
        [Required]
        public PersonModel Person { get; set; }
        
        [Required]
        public BookModel Book { get; set; }
        
        [Required]
        public DateTimeOffset DateTimeReceipt { get; set; }
    }
}
