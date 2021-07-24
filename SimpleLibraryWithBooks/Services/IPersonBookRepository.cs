using System;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Models.PersonBook;

namespace SimpleLibraryWithBooks.Services
{
    public interface IPersonBookRepository
    {
        IEnumerable<PersonBookEntity> GetAllPersonBooks();
        IEnumerable<PersonBookEntity> GetAllPersonBooks(Func<PersonBookEntity, bool> rule);
        PersonBookEntity GetPersonBook(int personBookId);
        PersonBookEntity GetPersonBook(string lastName, string firtsName, string patronymic, string title, string author);
        void InsertPersonBook(PersonBookEntity personBook);
        void UpdatePersonBook(PersonBookEntity personBook);
        void DeletePersonBook(int personBookId);
        void DeletePersonBook(string lastName, string firtsName, string patronymic, string title, string author);
        void Save();
        bool Contains(string lastName, string firtsName, string patronymic, string title, string author);
    }
}
