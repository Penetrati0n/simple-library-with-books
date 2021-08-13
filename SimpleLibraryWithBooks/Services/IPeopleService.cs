using System;
using Database.Models;
using System.Collections.Generic;

namespace SimpleLibraryWithBooks.Services
{
    public interface IPeopleService
    {
        IEnumerable<PersonEntity> GetAll();
        IEnumerable<PersonEntity> GetAll(Func<PersonEntity, bool> rule);
        IEnumerable<PersonEntity> GetAll(string firstName, string middleName, string lastName);
        PersonEntity Get(int personId);
        PersonEntity Get(string firstName, string middleName, string lastName, DateTimeOffset birthday);
        void Insert(PersonEntity person);
        void Update(PersonEntity person);
        void Delete(int personId);
        void Delete(string firstName, string middleName, string lastName);
        bool Contains(int personId);
        bool Contains(string firstName, string middleName, string lastName);
        bool Contains(string firstName, string middleName, string lastName, DateTimeOffset birthday);
        void Save();
    }
}
