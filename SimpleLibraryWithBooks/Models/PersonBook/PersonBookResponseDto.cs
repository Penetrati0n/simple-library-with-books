using System;
using SimpleLibraryWithBooks.Models.Book;
using SimpleLibraryWithBooks.Models.Person;

namespace SimpleLibraryWithBooks.Models.PersonBook
{
    public class PersonBookResponseDto
    {
        public PersonResponseDto Person { get; set; }

        public BookResponseDto Book { get; set; }

        public DateTimeOffset DateTimeReceipt { get; set; }
    }
}
