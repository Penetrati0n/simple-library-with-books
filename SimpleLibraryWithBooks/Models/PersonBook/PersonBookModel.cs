using System;
using SimpleLibraryWithBooks.Models.Book;
using SimpleLibraryWithBooks.Models.Person;

namespace SimpleLibraryWithBooks.Models.PersonBook
{
    public class PersonBookModel
    {
        public PersonModel Person { get; set; }
        public BookModel Book { get; set; }
        public DateTimeOffset DateTimeReceipt { get; set; }
    }
}
