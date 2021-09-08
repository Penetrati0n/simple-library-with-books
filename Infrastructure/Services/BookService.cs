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
    public class BookService : IBookService
    {
        private readonly IDatabaseContext _context;

        public BookService(IDatabaseContext context) =>
            _context = context;

        public async Task<IEnumerable<BookEntity>> GetAllAsync() =>
            await GetBooks().ToListAsync();

        public async Task<IEnumerable<BookEntity>> GetAllAsync(Expression<Func<BookEntity, bool>> rule) =>
            await GetBooks().Where(rule).ToListAsync();

        public async Task<BookEntity> GetAsync(int bookId) =>
            await GetBooks().SingleAsync(b => b.Id == bookId);

        public async Task<BookEntity> GetAsync(string bookName, int authorId) =>
            await GetBooks().SingleAsync(b => b.Name == bookName && b.AuthorId == authorId);

        public async Task InsertAsync(BookEntity book)
        {
            var genres = book.Genres.ToList();
            book.Genres.Clear();
            await _context.Books.AddAsync(book);
            foreach (var genre in genres)
            {
                var genreEntity = await _context.Genres.SingleAsync(g => g.Id == genre.Id);
                genreEntity.Books.Add(book); 
            }
        }

        public async Task UpdateAsync(BookEntity book)
        {
            var oldBook = await GetAsync(book.Id);
            oldBook.AuthorId = book.AuthorId;
            oldBook.Name = book.Name;
            foreach (var genre in book.Genres.Where(g => g.Name == "D"))
            {
                var genreEntity = await _context.Genres.SingleAsync(g => g.Id == genre.Id);
                if (oldBook.Genres.Contains(genreEntity))
                    oldBook.Genres.Remove(genreEntity);
            }
            foreach (var genre in book.Genres.Where(g => g.Name == "A"))
            {
                var genreEntity = await _context.Genres.SingleAsync(g => g.Id == genre.Id);
                if (!oldBook.Genres.Contains(genreEntity))
                    oldBook.Genres.Add(genreEntity);
            }
        }

        public async Task DeleteAsync(int bookId) =>
            _context.Books.Remove(await GetAsync(bookId));

        public async Task DeleteAsync(string bookName, int authorId) =>
            _context.Books.Remove(await GetAsync(bookName, authorId));

        public async Task<bool> ContainsAsync(int bookId) =>
            await _context.Books.AnyAsync(b => b.Id == bookId);

        public async Task<bool> ContainsAsync(string bookName, int authorId) =>
            await _context.Books.AnyAsync(b => b.Name == bookName && b.AuthorId == authorId);

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

        private IQueryable<BookEntity> GetBooks() =>
            _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genres)
                .Include(b => b.LibraryCards);
    }
}
