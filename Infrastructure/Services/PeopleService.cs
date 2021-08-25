using System;
using System.Linq;
using Database.Models;
using Database.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Services.Interfaces;

namespace Infrastructure.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly IDatabaseContext _context;

        public PeopleService(IDatabaseContext context) =>
            _context = context;

        public IEnumerable<PersonEntity> GetAll() =>
            _context.People
                .Include(p => p.LibraryCards);

        public IEnumerable<PersonEntity> GetAll(string firstName, string middleName, string lastName) =>
            GetAll().Where(p => p.FirstName == firstName &&
                                p.MiddleName == middleName &&
                                p.LastName == lastName);

        public IEnumerable<PersonEntity> GetAll(Func<PersonEntity, bool> rule) =>
            GetAll().Where(rule);

        public PersonEntity Get(int personId) =>
           GetAll().Single(p => p.Id == personId);

        public PersonEntity Get(string firstName, string middleName, string lastName, DateTimeOffset birthday) =>
            GetAll().Single(p => p.FirstName == firstName &&
                                 p.MiddleName == middleName &&
                                 p.LastName == lastName &&
                                 p.Birthday == birthday);

        public void Delete(int personId) =>
            _context.People.Remove(Get(personId));

        public void Delete(string firstName, string middleName, string lastName) =>
            _context.People.RemoveRange(GetAll(firstName, middleName, lastName));

        public void Insert(PersonEntity person) =>
            _context.People.Add(person);

        public void Update(PersonEntity personUpdated)
        {
            var person = Get(personUpdated.Id);
            person.Birthday = personUpdated.Birthday;
            person.FirstName = personUpdated.FirstName;
            person.LastName = personUpdated.LastName;
            person.MiddleName = personUpdated.MiddleName;
        }

        public bool Contains(int personId) =>
            _context.People.Any(p => p.Id == personId);

        public bool Contains(string firstName, string middleName, string lastName) =>
            _context.People.Any(p => p.LastName == lastName &&
                                     p.FirstName == firstName &&
                                     p.MiddleName == middleName);

        public bool Contains(string firstName, string middleName, string lastName, DateTimeOffset birthday) =>
            _context.People.Any(p => p.LastName == lastName &&
                                     p.FirstName == firstName &&
                                     p.MiddleName == middleName &&
                                     p.Birthday == birthday);

        public void Save()
        {
            var entries = _context.ChangeTracker.Entries();
            foreach (var entry in entries.Where(e => e.State == EntityState.Added))
            {
                var entity = entry.Entity as Expansion;
                if (entity is null)
                    continue;
                entity.TimeCreate = DateTimeOffset.Now;
                entity.TimeEdit = entity.TimeCreate;
                entity.Version = 1;
                if (entry.Entity is LibraryCardExpansion)
                    (entry.Entity as LibraryCardExpansion).TimeReturn = entity.TimeCreate.AddDays(7);
            }
            foreach (var entry in entries.Where(e => e.State == EntityState.Modified))
            {
                var entity = entry.Entity as Expansion;
                if (entity is null)
                    continue;
                entity.TimeEdit = DateTimeOffset.Now;
                entity.Version++;
            }
            _context.SaveChanges();
        }
    }
}
