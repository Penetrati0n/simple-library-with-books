using System;
using Database.Models;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Infrastructure.Services.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreEntity>> GetAllAsync();
        Task<IEnumerable<GenreEntity>> GetAllAsync(Expression<Func<GenreEntity, bool>> rule);
        Task<GenreEntity> GetAsync(int genreId);
        Task<GenreEntity> GetAsync(string genreName);
        Task InsertAsync(GenreEntity genre);
        Task UpdateAsync(GenreEntity genre);
        Task DeleteAsync(int genreId);
        Task DeleteAsync(string genreName);
        Task<bool> ContainsAsync(int genreId);
        Task<bool> ContainsAsync(string genreName);
        bool Contains(int genreId);
        bool Contains(string genreName);
        Task SaveAsync();
    }
}
