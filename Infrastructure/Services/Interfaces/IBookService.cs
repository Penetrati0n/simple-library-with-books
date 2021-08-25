using System;
using Database.Models;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Infrastructure.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookEntity>> GetAllAsync();
        Task<IEnumerable<BookEntity>> GetAllAsync(Expression<Func<BookEntity, bool>> rule);
        Task<BookEntity> GetAsync(int bookId);
        Task<BookEntity> GetAsync(string bookName, int authorId);
        Task InsertAsync(BookEntity book);
        Task UpdateAsync(BookEntity book);
        Task DeleteAsync(int bookId);
        Task DeleteAsync(string bookName, int authorId);
        Task<bool> ContainsAsync(int bookId);
        Task<bool> ContainsAsync(string bookName, int authorId);
        Task SaveAsync();
    }
}
