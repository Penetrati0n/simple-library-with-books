using System.Collections.Generic;
using SimpleLibraryWithBooks.Models.PersonBook;

namespace SimpleLibraryWithBooks.Services
{
    public static class PersonBookRepository
    {
        public readonly static List<PersonBookModel> PersonBooks = new List<PersonBookModel>();
    }
}
