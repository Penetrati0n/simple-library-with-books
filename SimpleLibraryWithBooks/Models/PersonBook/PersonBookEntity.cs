using System;
using SimpleLibraryWithBooks.Models.Book;
using SimpleLibraryWithBooks.Models.Person;

namespace SimpleLibraryWithBooks.Models.PersonBook
{
    public class PersonBookEntity
    {
        public int Id { get; set; }

        public PersonEntity Person { get; set; }
        
        public BookEntity Book { get; set; }
        
        public DateTimeOffset DateTimeReceipt { get; set; }
    }
}
