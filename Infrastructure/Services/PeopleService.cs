using System;
using System.Linq;
using Database.Models;
using Database.Interfaces;
using System.Threading.Tasks;
using System.Linq.Expressions;
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

        public async Task<IEnumerable<PersonEntity>> GetAllAsync() =>
            await GetPeople().ToListAsync();

        public async Task<IEnumerable<PersonEntity>> GetAllAsync(string firstName, string middleName, string lastName) =>
            await GetPeople().Where(p => p.FirstName == firstName &&
                                    p.MiddleName == middleName &&
                                    p.LastName == lastName).ToListAsync();

        public async Task<IEnumerable<PersonEntity>> GetAllAsync(Expression<Func<PersonEntity, bool>> rule) =>
            await GetPeople().Where(rule).ToListAsync();

        public async Task<PersonEntity> GetAsync(int personId) =>
           await GetPeople().SingleAsync(p => p.Id == personId);

        public async Task<PersonEntity> GetAsync(string firstName, string middleName, string lastName, DateTimeOffset birthday) =>
            await GetPeople().SingleAsync(p => p.FirstName == firstName &&
                                               p.MiddleName == middleName &&
                                               p.LastName == lastName &&
                                               p.Birthday == birthday);

        public async Task DeleteAsync(int personId) =>
            _context.People.Remove(await GetAsync(personId));

        public async Task DeleteAsync(string firstName, string middleName, string lastName) =>
            _context.People.RemoveRange(await GetAllAsync(firstName, middleName, lastName));

        public async Task InsertAsync(PersonEntity person) =>
            await _context.People.AddAsync(person);

        public async Task UpdateAsync(PersonEntity personUpdated)
        {
            var person = await GetAsync(personUpdated.Id);
            person.Birthday = personUpdated.Birthday;
            person.FirstName = personUpdated.FirstName;
            person.LastName = personUpdated.LastName;
            person.MiddleName = personUpdated.MiddleName;
        }

        public async Task<bool> ContainsAsync(int personId) =>
            await _context.People.AnyAsync(p => p.Id == personId);

        public async Task<bool> ContainsAsync(string firstName, string middleName, string lastName) =>
            await _context.People.AnyAsync(p => p.LastName == lastName &&
                                                p.FirstName == firstName &&
                                                p.MiddleName == middleName);

        public async Task<bool> ContainsAsync(string firstName, string middleName, string lastName, DateTimeOffset birthday) =>
            await _context.People.AnyAsync(p => p.LastName == lastName &&
                                                p.FirstName == firstName &&
                                                p.MiddleName == middleName &&
                                                p.Birthday == birthday);

        public async Task SaveAsync()
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
            await _context.SaveChangesAsync();
        }

        private IQueryable<PersonEntity> GetPeople() =>
            _context.People
                .Include(p => p.LibraryCards);
    }
}
