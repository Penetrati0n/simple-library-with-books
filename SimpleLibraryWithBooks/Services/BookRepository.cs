using System;
using Database;
using System.Linq;
using Database.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SimpleLibraryWithBooks.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly DatabaseContext _context;

        public BookRepository(DbContextOptions<DatabaseContext> options) =>
            _context = new DatabaseContext(options);

        public IEnumerable<BookEntity> GetAllBooks() =>
            _context.Books;

        public IEnumerable<BookEntity> GetAllBooks(Func<BookEntity, bool> rule) =>
            _context.Books.Where(rule);

        public BookEntity GetBook(int bookId) =>
           _context.Books.Single(b => b.Id == bookId);

        public BookEntity GetBook(string title, string author) =>
            _context.Books.Single(b => b.Title == title &&
                                       b.Author == author);

        public void DeleteBook(int bookId) =>
            _context.Books.Remove(GetBook(bookId));

        public void DeleteBook(string title, string author) =>
            _context.Books.Remove(GetBook(title, author));

        public void InsertBook(BookEntity book) =>
            _context.Books.Add(book);

        public void UpdateBook(BookEntity book) =>
            _context.Entry(book).State = EntityState.Modified;

        public void Save() =>
            _context.SaveChanges();

        public bool Contains(string title, string author) =>
            _context.Books.Any(b => b.Title == title &&
                                    b.Author == author);
    }
}
