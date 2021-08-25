using System;
using Database.Models;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Infrastructure.Services.Interfaces
{
    public interface ILibraryCardService
    {
        Task<IEnumerable<LibraryCardEntity>> GetAllAsync();
        Task<IEnumerable<LibraryCardEntity>> GetAllAsync(Expression<Func<LibraryCardEntity, bool>> rule);
        Task<LibraryCardEntity> GetAsync(int bookId, int personId);
        Task InsertAsync(LibraryCardEntity LibraryCardEntity);
        Task UpdateAsync(LibraryCardEntity LibraryCardEntity);
        Task DeleteAsync(int bookId, int personId);
        Task<bool> ContainsAsync(int bookId, int personId);
        Task SaveAsync();
    }
}
