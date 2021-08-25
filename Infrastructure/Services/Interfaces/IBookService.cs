using System;
using Database.Models;
using System.Collections.Generic;

namespace Infrastructure.Services.Interfaces
{
    public interface IBookService
    {
        IEnumerable<BookEntity> GetAll();
        IEnumerable<BookEntity> GetAll(Func<BookEntity, bool> rule);
        BookEntity Get(int bookId);
        BookEntity Get(string bookName, int authorId);
        void Insert(BookEntity book);
        void Update(BookEntity book);
        void Delete(int bookId);
        void Delete(string bookName, int authorId);
        bool Contains(int bookId);
        bool Contains(string bookName, int authorId);
        void Save();
    }
}
