using System;
using System.Linq;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Models.Person;

namespace SimpleLibraryWithBooks.Services
{
    public class PeopleRepository : IPeopleRepository
    {
        private static readonly List<PersonModel> _people = new List<PersonModel>()
        {
            new PersonModel() { LastName = "Кармазин", FirstName = "Лев", Patronymic = "Олегович", Birthday = DateTimeOffset.Parse("12/03/1983")},
            new PersonModel() { LastName = "Тихомиров", FirstName = "Филипп", Patronymic = "Михайлович", Birthday = DateTimeOffset.Parse("17/09/1980")},
            new PersonModel() { LastName = "Травникова", FirstName = "Мариетта", Patronymic = "Платоновна", Birthday = DateTimeOffset.Parse("03/07/2001")},
        };

        public IEnumerable<PersonModel> GetAllPeople() =>
            _people;

        public IEnumerable<PersonModel> GetAllPeople(Func<PersonModel, bool> rule) =>
            _people.Where(rule);

        public PersonModel GetPerson(int personId) =>
           _people[personId];

        public PersonModel GetPerson(string lastName, string firstName, string patronymic) =>
            _people.Single(p => (p.LastName, p.FirstName, p.Patronymic) == (lastName, firstName, patronymic));

        public void DeletePerson(int personId) =>
            _people.RemoveAt(personId);

        public void InsertPerson(PersonModel person) =>
            _people.Add(person);

        public void UpdatePerson(PersonModel person)
        { }

        public void DeletePerson(string lastName, string firstName, string patronymic)
        {
            var person = GetPerson(lastName, firstName, patronymic);
            _people.Remove(person);
        }

        public void Save()
        { }

        public bool Contains(string lastName, string firstName, string patronymic) =>
            _people.Any(p => (p.LastName, p.FirstName, p.Patronymic) == (lastName, firstName, patronymic));
    }
}
