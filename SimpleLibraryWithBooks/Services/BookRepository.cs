using System;
using System.Linq;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Models.Book;

namespace SimpleLibraryWithBooks.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly static List<BookModel> _books = new List<BookModel>()
        {
            new BookModel() { Title  = "Горе от ума", Author = "Александр Грибоедов", Genre = "Комедия" },
            new BookModel() { Title  = "Гордость и предубеждение", Author = "Джейн Остин", Genre = "Роман" },
            new BookModel() { Title  = "Тёмные начала", Author = "Филип Пулман", Genre = "Фэнтези" },
        };

        public IEnumerable<BookModel> GetAllBooks() =>
            _books;

        public IEnumerable<BookModel> GetAllBooks(Func<BookModel, bool> rule) =>
            _books.Where(rule);

        public BookModel GetBook(int bookId) =>
           _books[bookId];

        public BookModel GetBook(string title, string author) =>
            _books.Single(b => (b.Title, b.Author) == (title, author));

        public void DeleteBook(int bookId) =>
            _books.RemoveAt(bookId);

        public void InsertBook(BookModel book) =>
            _books.Add(book);

        public void UpdateBook(BookModel book)
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
