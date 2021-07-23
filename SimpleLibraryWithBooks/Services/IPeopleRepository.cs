using System;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Models.Person;

namespace SimpleLibraryWithBooks.Services
{
    public interface IPeopleRepository
    {
        IEnumerable<PersonModel> GetAllPeople();
        IEnumerable<PersonModel> GetAllPeople(Func<PersonModel, bool> rule);
        PersonModel GetPerson(int personId);
        PersonModel GetPerson(string lastName, string firstName, string patronymic);
        void InsertPerson(PersonModel person);
        void UpdatePerson(PersonModel person);
        void DeletePerson(int personId);
        void DeletePerson(string lastName, string firstName, string patronymic);
        void Save();
        bool Contains(string lastName, string firstName, string patronymic);
    }
}
