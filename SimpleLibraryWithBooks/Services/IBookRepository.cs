using System;
using Database.Models;
using System.Collections.Generic;

namespace SimpleLibraryWithBooks.Services
{
    public interface IBookRepository
    {
        IEnumerable<BookEntity> GetAllBooks();
        IEnumerable<BookEntity> GetAllBooks(Func<BookEntity, bool> rule);
        BookEntity GetBook(int bookId);
        BookEntity GetBook(string title, string author);
        void InsertBook(BookEntity book);
        void UpdateBook(BookEntity book);
        void DeleteBook(int bookId);
        void DeleteBook(string title, string author);
        void Save();
        bool Contains(string title, string author);
    }
}
