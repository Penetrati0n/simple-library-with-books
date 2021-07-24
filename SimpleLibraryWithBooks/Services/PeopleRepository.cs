using System;
using System.Linq;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Models.Person;

namespace SimpleLibraryWithBooks.Services
{
    public class PeopleRepository : IPeopleRepository
    {
        private static readonly List<PersonEntity> _people = new List<PersonEntity>()
        {
            new PersonEntity() { Id = 1, LastName = "Кармазин", FirstName = "Лев", Patronymic = "Олегович", Birthday = DateTimeOffset.Parse("12/03/1983")},
            new PersonEntity() { Id = 2, LastName = "Тихомиров", FirstName = "Филипп", Patronymic = "Михайлович", Birthday = DateTimeOffset.Parse("17/09/1980")},
            new PersonEntity() { Id = 3, LastName = "Травникова", FirstName = "Мариетта", Patronymic = "Платоновна", Birthday = DateTimeOffset.Parse("03/07/2001")},
        };

        private static int _currentId = _people.Count;

        public IEnumerable<PersonEntity> GetAllPeople() =>
            _people;

        public IEnumerable<PersonEntity> GetAllPeople(Func<PersonEntity, bool> rule) =>
            _people.Where(rule);

        public PersonEntity GetPerson(int personId) =>
           _people[personId];

        public PersonEntity GetPerson(string lastName, string firstName, string patronymic) =>
            _people.Single(p => (p.LastName, p.FirstName, p.Patronymic) == (lastName, firstName, patronymic));

        public void DeletePerson(int personId) =>
            _people.RemoveAt(personId);

        public void InsertPerson(PersonEntity person)
        {
            _currentId++;
            person.Id = _currentId;

            _people.Add(person);
        }

        public void UpdatePerson(PersonEntity person)
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
