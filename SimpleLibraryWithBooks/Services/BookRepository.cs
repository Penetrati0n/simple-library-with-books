using System;
using System.Linq;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Models.Book;

namespace SimpleLibraryWithBooks.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly static List<BookEntity> _books = new List<BookEntity>()
        {
            new BookEntity() {  Id = 1, Title  = "Горе от ума", Author = "Александр Грибоедов", Genre = "Комедия" },
            new BookEntity() {  Id = 2, Title  = "Гордость и предубеждение", Author = "Джейн Остин", Genre = "Роман" },
            new BookEntity() {  Id = 3, Title  = "Тёмные начала", Author = "Филип Пулман", Genre = "Фэнтези" },
        };

        private static int _currentId = _books.Count;

        public IEnumerable<BookEntity> GetAllBooks() =>
            _books;

        public IEnumerable<BookEntity> GetAllBooks(Func<BookEntity, bool> rule) =>
            _books.Where(rule);

        public BookEntity GetBook(int bookId) =>
           _books[bookId];

        public BookEntity GetBook(string title, string author) =>
            _books.Single(b => (b.Title, b.Author) == (title, author));

        public void DeleteBook(int bookId) =>
            _books.RemoveAt(bookId);

        public void InsertBook(BookEntity book)
        {
            _currentId++;
            book.Id = _currentId;

            _books.Add(book);
        }

        public void UpdateBook(BookEntity book)
        { }

        public void DeleteBook(string title, string author)
        {
            var book = GetBook(title, author);
            _books.Remove(book);
        }

        public void Save()
        { }

        public bool Contains(string title, string author) =>
            _books.Any(b => (b.Title, b.Author) == (title, author));
    }
}
