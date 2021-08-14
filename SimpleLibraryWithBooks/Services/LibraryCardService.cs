using System;
using Database;
using System.Linq;
using Database.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SimpleLibraryWithBooks.Services
{
    public class LibraryCardService : ILibraryCardService
    {
        private readonly DatabaseContext _context;

        public LibraryCardService(DbContextOptions<DatabaseContext> options) =>
            _context = new DatabaseContext(options);

        public IEnumerable<LibraryCardEntity> GetAll() =>
            _context.LibraryCards
                .Include(lc => lc.Book)
                    .ThenInclude(b => b.Genres)
                .Include(lc => lc.Book)
                    .ThenInclude(b => b.Author)
                .Include(lc => lc.Person);

        public IEnumerable<LibraryCardEntity> GetAll(Func<LibraryCardEntity, bool> rule) =>
            GetAll().Where(rule);

        public LibraryCardEntity Get(int bookId, int personId) =>
            GetAll().Single(lc => lc.BookId == bookId && lc.PersonId == personId);

        public void Insert(LibraryCardEntity libraryCard) =>
            _context.LibraryCards.Add(libraryCard);

        public void Update(LibraryCardEntity libraryCard)
        {
            var oldLibraryCard = Get(libraryCard.BookId, libraryCard.PersonId);
            _context.Entry(oldLibraryCard).State = EntityState.Modified;
            oldLibraryCard.TimeReturn = oldLibraryCard.TimeReturn.Add(libraryCard.TimeReturn - default(DateTimeOffset));
        }

        public void Delete(int bookId, int personId) =>
            _context.LibraryCards.Remove(Get(bookId, personId));

        public bool Contains(int bookId, int personId) =>
            _context.LibraryCards.Any(lc => lc.BookId == bookId && lc.PersonId == personId);

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
