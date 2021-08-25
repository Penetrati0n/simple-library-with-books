using System;
using Database.Models;
using System.Collections.Generic;

namespace Infrastructure.Services.Interfaces
{
    public interface IGenreService
    {
        IEnumerable<GenreEntity> GetAll();
        IEnumerable<GenreEntity> GetAll(Func<GenreEntity, bool> rule);
        GenreEntity Get(int genreId);
        GenreEntity Get(string genreName);
        void Insert(GenreEntity genre);
        void Update(GenreEntity genre);
        void Delete(int genreId);
        void Delete(string genreName);
        bool Contains(int genreId);
        bool Contains(string genreName);
        void Save();
    }
}
