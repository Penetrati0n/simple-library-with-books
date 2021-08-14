using System;
using Database.Models;
using System.Collections.Generic;

namespace SimpleLibraryWithBooks.Services
{
    public interface ILibraryCardService
    {
        IEnumerable<LibraryCardEntity> GetAll();
        IEnumerable<LibraryCardEntity> GetAll(Func<LibraryCardEntity, bool> rule);
        LibraryCardEntity Get(int bookId, int personId);
        void Insert(LibraryCardEntity LibraryCardEntity);
        void Update(LibraryCardEntity LibraryCardEntity);
        void Delete(int bookId, int personId);
        bool Contains(int bookId, int personId);
        void Save();
    }
}
