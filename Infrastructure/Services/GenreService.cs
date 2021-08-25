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
    public class GenreService : IGenreService
    {
        private readonly IDatabaseContext _context;

        public GenreService(IDatabaseContext context) =>
            _context = context;

        public async Task<IEnumerable<GenreEntity>> GetAllAsync() =>
            await GetGenres().ToListAsync();

        public async Task<IEnumerable<GenreEntity>> GetAllAsync(Expression<Func<GenreEntity, bool>> rule) =>
            await GetGenres().Where(rule).ToListAsync();

        public async Task<GenreEntity> GetAsync(int genreId) =>
            await GetGenres().SingleAsync(g => g.Id == genreId);

        public async Task<GenreEntity> GetAsync(string genreName) =>
            await GetGenres().SingleAsync(g => g.Name == genreName);

        public async Task InsertAsync(GenreEntity genre) =>
            await _context.Genres.AddAsync(genre);

        public async Task UpdateAsync(GenreEntity genre)
        {
            var oldGenre = await GetAsync(genre.Id);
            oldGenre.Name = genre.Name;
        }

        public async Task DeleteAsync(int genreId) =>
            _context.Genres.Remove(await GetAsync(genreId));

        public async Task DeleteAsync(string genreName) =>
            _context.Genres.Remove(await GetAsync(genreName));

        public async Task<bool> ContainsAsync(int genreId) =>
            await _context.Genres.AnyAsync(g => g.Id == genreId);

        public async Task<bool> ContainsAsync(string genreName) =>
            await _context.Genres.AnyAsync(g => g.Name == genreName);

        public bool Contains(int genreId) =>
            _context.Genres.Any(g => g.Id == genreId);

        public bool Contains(string genreName) =>
            _context.Genres.Any(g => g.Name == genreName);

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

        private IQueryable<GenreEntity> GetGenres() =>
            _context.Genres.Include(g => g.Books);
    }
}
