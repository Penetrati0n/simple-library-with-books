using System;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Models.PersonBook;

namespace SimpleLibraryWithBooks.Services
{
    public interface IPersonBookRepository
    {
        IEnumerable<PersonBookModel> GetAllPersonBooks();
        IEnumerable<PersonBookModel> GetAllPersonBooks(Func<PersonBookModel, bool> rule);
        PersonBookModel GetPersonBook(int personBookId);
        PersonBookModel GetPersonBook(string lastName, string firtsName, string patronymic, string title, string author);
        void InsertPersonBook(PersonBookModel personBook);
        void UpdatePersonBook(PersonBookModel personBook);
        void DeletePersonBook(int personBookId);
        void DeletePersonBook(string lastName, string firtsName, string patronymic, string title, string author);
        void Save();
        bool Contains(string lastName, string firtsName, string patronymic, string title, string author);
    }
}
