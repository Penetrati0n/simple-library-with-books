using System;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Models.Book;

namespace SimpleLibraryWithBooks.Services
{
    public interface IBookRepository
    {
        IEnumerable<BookModel> GetAllBooks();
        IEnumerable<BookModel> GetAllBooks(Func<BookModel, bool> rule);
        BookModel GetBook(int bookId);
        BookModel GetBook(string title, string author);
        void InsertBook(BookModel book);
        void UpdateBook(BookModel book);
        void DeleteBook(int bookId);
        void DeleteBook(string title, string author);
        void Save();
        bool Contains(string title, string author);
    }
}
