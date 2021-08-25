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
    public class AuthorService : IAuthorService
    {
        private readonly IDatabaseContext _context;

        public AuthorService(IDatabaseContext context) =>
            _context = context;

        public async Task<IEnumerable<AuthorEntity>> GetAllAsync() =>
            await GetAuthors().ToListAsync();

        public async Task<IEnumerable<AuthorEntity>> GetAllAsync(Expression<Func<AuthorEntity, bool>> rule) =>
            await GetAuthors().Where(rule).ToListAsync();

        public async Task<AuthorEntity> GetAsync(int authorId) =>
            await GetAuthors().SingleAsync(a => a.Id == authorId);

        public async Task<AuthorEntity> GetAsync(string firstName, string middleName, string lastName) =>
            await GetAuthors().SingleAsync(a => a.FirstName == firstName &&
                                           a.MiddleName == middleName &&
                                           a.LastName == lastName);

        public async Task InsertAsync(AuthorEntity author) =>
            await _context.Authors.AddAsync(author);

        public async Task UpdateAsync(AuthorEntity author)
        {
            var oldAuthor = await GetAsync(author.Id);
            oldAuthor.FirstName = author.FirstName;
            oldAuthor.MiddleName = author.MiddleName;
            oldAuthor.LastName = author.LastName;
        }

        public async Task DeleteAsync(int authorId) =>
            _context.Authors.Remove(await GetAsync(authorId));

        public async Task DeleteAsync(string firstName, string middleName, string lastName) =>
            _context.Authors.Remove(await GetAsync(firstName, middleName, lastName));

        public async Task<bool> ContainsAsync(int authorId) =>
            await _context.Authors.AnyAsync(a => a.Id == authorId);

        public async Task<bool> ContainsAsync(string firstName, string middleName, string lastName) =>
            await _context.Authors.AnyAsync(a => a.FirstName == firstName &&
                                                 a.MiddleName == middleName &&
                                                 a.LastName == lastName);

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

        private IQueryable<AuthorEntity> GetAuthors() =>
            _context.Authors;
    }
}
