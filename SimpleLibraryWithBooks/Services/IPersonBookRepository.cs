using System;
using Database.Models;
using System.Collections.Generic;

namespace SimpleLibraryWithBooks.Services
{
    public interface IPersonBookRepository
    {
        IEnumerable<PersonBookEntity> GetAllPersonBooks();
        IEnumerable<PersonBookEntity> GetAllPersonBooks(Func<PersonBookEntity, bool> rule);
        PersonBookEntity GetPersonBook(string lastName, string firtsName, string patronymic, string title, string author);
        void InsertPersonBook(PersonBookEntity personBook);
        void UpdatePersonBook(PersonBookEntity personBook);
        void DeletePersonBook(string lastName, string firtsName, string patronymic, string title, string author);
        void Save();
        bool Contains(string lastName, string firtsName, string patronymic, string title, string author);
    }
}
