using System;
using Database;
using System.Linq;
using Database.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SimpleLibraryWithBooks.Services
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly DatabaseContext _context;

        public PeopleRepository(DbContextOptions<DatabaseContext> options) =>
            _context = new DatabaseContext(options);

        public IEnumerable<PersonEntity> GetAllPeople() =>
            _context.People;

        public IEnumerable<PersonEntity> GetAllPeople(Func<PersonEntity, bool> rule) =>
            _context.People.Where(rule);

        public PersonEntity GetPerson(int personId) =>
           _context.People.Single(p => p.Id == personId);

        public PersonEntity GetPerson(string lastName, string firstName, string patronymic) =>
            _context.People.Single(p => p.LastName == lastName &&
                                        p.FirstName == firstName &&
                                        p.Patronymic == patronymic);

        public void DeletePerson(int personId) =>
            _context.People.Remove(GetPerson(personId));

        public void DeletePerson(string lastName, string firstName, string patronymic) =>
            _context.People.Remove(GetPerson(lastName, firstName, patronymic));

        public void InsertPerson(PersonEntity person) =>
            _context.People.Add(person);

        public void UpdatePerson(PersonEntity person) =>
            _context.Entry(person).State = EntityState.Modified;

        public void Save() =>
            _context.SaveChanges();

        public bool Contains(string lastName, string firstName, string patronymic) =>
            _context.People.Any(p => p.LastName == lastName &&
                                     p.FirstName == firstName &&
                                     p.Patronymic == patronymic);
    }
}
