using System;
using Database.Models;
using System.Collections.Generic;

namespace SimpleLibraryWithBooks.Services
{
    public interface IPeopleRepository
    {
        IEnumerable<PersonEntity> GetAllPeople();
        IEnumerable<PersonEntity> GetAllPeople(Func<PersonEntity, bool> rule);
        PersonEntity GetPerson(int personId);
        PersonEntity GetPerson(string lastName, string firstName, string patronymic);
        void InsertPerson(PersonEntity person);
        void UpdatePerson(PersonEntity person);
        void DeletePerson(int personId);
        void DeletePerson(string lastName, string firstName, string patronymic);
        void Save();
        bool Contains(string lastName, string firstName, string patronymic);
    }
}
