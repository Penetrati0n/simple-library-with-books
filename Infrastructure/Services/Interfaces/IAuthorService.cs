using System;
using Database.Models;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Infrastructure.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorEntity>> GetAllAsync();
        Task<IEnumerable<AuthorEntity>> GetAllAsync(Expression<Func<AuthorEntity, bool>> rule);
        Task<AuthorEntity> GetAsync(int authorId);
        Task<AuthorEntity> GetAsync(string firstName, string middleName, string lastName);
        Task InsertAsync(AuthorEntity author);
        Task UpdateAsync(AuthorEntity author);
        Task DeleteAsync(int authorId);
        Task DeleteAsync(string firstName, string middleName, string lastName);
        Task<bool> ContainsAsync(int authorId);
        Task<bool> ContainsAsync(string firstName, string middleName, string lastName);
        Task SaveAsync();
    }
}
