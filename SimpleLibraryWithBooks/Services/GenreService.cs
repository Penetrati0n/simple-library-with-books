using System;
using Database;
using System.Linq;
using Database.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SimpleLibraryWithBooks.Services
{
    public class GenreService : IGenreService
    {
        private readonly DatabaseContext _context;

        public GenreService(DbContextOptions<DatabaseContext> options) =>
            _context = new DatabaseContext(options);

        public IEnumerable<GenreEntity> GetAll() =>
            _context.Genres.Include(g => g.Books);

        public IEnumerable<GenreEntity> GetAll(Func<GenreEntity, bool> rule) =>
            GetAll().Where(rule);

        public GenreEntity Get(int genreId) =>
            GetAll().Single(g => g.Id == genreId);

        public GenreEntity Get(string genreName) =>
            GetAll().Single(g => g.Name == genreName);

        public void Insert(GenreEntity genre) =>
            _context.Genres.Add(genre);

        public void Update(GenreEntity genre)
        {
            var oldGenre = Get(genre.Id);
            _context.Entry(oldGenre).State = EntityState.Modified;
            oldGenre.Name = genre.Name;
        }

        public void Delete(int genreId) =>
            _context.Genres.Remove(Get(genreId));

        public void Delete(string genreName) =>
            _context.Genres.Remove(Get(genreName));

        public bool Contains(int genreId) =>
            _context.Genres.Any(g => g.Id == genreId);

        public bool Contains(string genreName) =>
            _context.Genres.Any(g => g.Name == genreName);

        public void Save() =>
            _context.SaveChanges();
    }
}
