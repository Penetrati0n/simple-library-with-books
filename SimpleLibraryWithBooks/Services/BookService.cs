﻿using System;
using Database;
using System.Linq;
using Database.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SimpleLibraryWithBooks.Services
{
    public class BookService : IBookService
    {
        private readonly DatabaseContext _context;

        public BookService(DbContextOptions<DatabaseContext> options) =>
            _context = new DatabaseContext(options);

        public IEnumerable<BookEntity> GetAll() =>
            _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genres)
                .Include(b => b.People);

        public IEnumerable<BookEntity> GetAll(Func<BookEntity, bool> rule) =>
            GetAll().Where(rule);

        public BookEntity Get(int bookId) =>
            GetAll().Single(b => b.Id == bookId);

        public BookEntity Get(string bookName, int authorId) =>
            GetAll().Single(b => b.Name == bookName && b.AuthorId == authorId);

        public void Insert(BookEntity book) =>
            _context.Books.Add(book);

        public void Update(BookEntity book)
        {
            var oldBook = Get(book.Id);
            _context.Entry(oldBook).State = EntityState.Modified;
            oldBook.AuthorId = book.AuthorId;
            oldBook.Name = book.Name;
            foreach (var genre in book.Genres.Where(g => g.Name == "D"))
            {
                var genreEntity = _context.Genres.Single(g => g.Id == genre.Id);
                if (oldBook.Genres.Contains(genreEntity))
                    oldBook.Genres.Remove(genreEntity);
            }
            foreach (var genre in book.Genres.Where(g => g.Name == "A"))
            {
                var genreEntity = _context.Genres.Single(g => g.Id == genre.Id);
                if (!oldBook.Genres.Contains(genreEntity))
                    oldBook.Genres.Add(genreEntity);
            }
        }

        public void Delete(int bookId) =>
            _context.Books.Remove(Get(bookId));

        public void Delete(string bookName, int authorId) =>
            _context.Books.Remove(Get(bookName, authorId));

        public bool Contains(int bookId) =>
            _context.Books.Any(b => b.Id == bookId);

        public bool Contains(string bookName, int authorId) =>
            _context.Books.Any(b => b.Name == bookName && b.AuthorId == authorId);

        public void Save() =>
            _context.SaveChanges();
    }
}
