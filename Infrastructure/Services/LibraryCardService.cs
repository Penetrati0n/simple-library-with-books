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
    public class LibraryCardService : ILibraryCardService
    {
        private readonly IDatabaseContext _context;

        public LibraryCardService(IDatabaseContext context) =>
            _context = context;

        public async Task<IEnumerable<LibraryCardEntity>> GetAllAsync() =>
            await GetLibraryCards().ToListAsync();

        public async Task<IEnumerable<LibraryCardEntity>> GetAllAsync(Expression<Func<LibraryCardEntity, bool>> rule) =>
            await GetLibraryCards().Where(rule).ToListAsync();

        public async Task<LibraryCardEntity> GetAsync(int bookId, int personId) =>
            await GetLibraryCards().SingleAsync(lc => lc.BookId == bookId && lc.PersonId == personId);

        public async Task InsertAsync(LibraryCardEntity libraryCard) =>
            await _context.LibraryCards.AddAsync(libraryCard);

        public async Task UpdateAsync(LibraryCardEntity libraryCard)
        {
            var oldLibraryCard = await GetAsync(libraryCard.BookId, libraryCard.PersonId);
            oldLibraryCard.TimeReturn = oldLibraryCard.TimeReturn.Add(libraryCard.TimeReturn - default(DateTimeOffset));
        }

        public async Task DeleteAsync(int bookId, int personId) =>
            _context.LibraryCards.Remove(await GetAsync(bookId, personId));

        public async Task<bool> ContainsAsync(int bookId, int personId) =>
            await _context.LibraryCards.AnyAsync(lc => lc.BookId == bookId && lc.PersonId == personId);

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

        private IQueryable<LibraryCardEntity> GetLibraryCards() =>
            _context.LibraryCards
                .Include(lc => lc.Book)
                    .ThenInclude(b => b.Genres)
                .Include(lc => lc.Book)
                    .ThenInclude(b => b.Author)
                .Include(lc => lc.Person);
    }
}
