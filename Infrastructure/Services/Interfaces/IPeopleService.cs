using System;
using Database.Models;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Infrastructure.Services.Interfaces
{
    public interface IPeopleService
    {
        Task<IEnumerable<PersonEntity>> GetAllAsync();
        Task<IEnumerable<PersonEntity>> GetAllAsync(Expression<Func<PersonEntity, bool>> rule);
        Task<IEnumerable<PersonEntity>> GetAllAsync(string firstName, string middleName, string lastName);
        Task<PersonEntity> GetAsync(int personId);
        Task<PersonEntity> GetAsync(string firstName, string middleName, string lastName, DateTimeOffset birthday);
        Task InsertAsync(PersonEntity person);
        Task UpdateAsync(PersonEntity person);
        Task DeleteAsync(int personId);
        Task DeleteAsync(string firstName, string middleName, string lastName);
        Task<bool> ContainsAsync(int personId);
        Task<bool> ContainsAsync(string firstName, string middleName, string lastName);
        Task<bool> ContainsAsync(string firstName, string middleName, string lastName, DateTimeOffset birthday);
        Task SaveAsync();
    }
}
