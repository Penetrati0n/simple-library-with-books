using System;
using Database.Models;
using System.Collections.Generic;

namespace SimpleLibraryWithBooks.Services
{
    public interface IAuthorService
    {
        IEnumerable<AuthorEntity> GetAll();
        IEnumerable<AuthorEntity> GetAll(Func<AuthorEntity, bool> rule);
        AuthorEntity Get(int authorId);
        AuthorEntity Get(string firstName, string middleName, string lastName);
        void Insert(AuthorEntity author);
        void Update(AuthorEntity author);
        void Delete(int authorId);
        void Delete(string firstName, string middleName, string lastName);
        bool Contains(int authorId);
        bool Contains(string firstName, string middleName, string lastName);
        void Save();
    }
}
