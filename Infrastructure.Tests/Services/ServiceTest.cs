using System;
using Database;
using Database.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Services
{
    public class ServiceTest
    {
        protected DbContextOptions<DatabaseContext> ContextOptions { get; }

        protected ServiceTest(DbContextOptions<DatabaseContext> contextOptions)
        {
            ContextOptions = contextOptions;

            Seed();
        }

        private void Seed()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var genres = new GenreEntity[]
                {
                    new GenreEntity { Id = 1, Name = "Роман", TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                    new GenreEntity { Id = 2, Name = "Детектив", TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                    new GenreEntity { Id = 3, Name = "Фантастика", TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                    new GenreEntity { Id = 4, Name = "Приключения", TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                    new GenreEntity { Id = 5, Name = "Научная книга", TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                };

                var authors = new AuthorEntity[]
                {
                    new AuthorEntity { Id = 1, FirstName = "Эрих", MiddleName = "Ремарк", LastName = "Мария", TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                    new AuthorEntity { Id = 2, FirstName = "Анджей", MiddleName = "Сапковский", LastName = "", TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                    new AuthorEntity { Id = 3, FirstName = "Дина", MiddleName = "Рубина", LastName = "Ильинична", TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                    new AuthorEntity { Id = 4, FirstName = "Дэвид", MiddleName = "Грегори", LastName = "Робертс", TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                    new AuthorEntity { Id = 5, FirstName = "Лев", MiddleName = "Толстой", LastName = "Николаевич", TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                };

                var people = new PersonEntity[]
                {
                    new PersonEntity { Id = 1, FirstName = "Лютов", MiddleName = "Дорофей", LastName = "Валентинович", Birthday = new DateTimeOffset(1973, 7, 17, 0, 0, 0, TimeSpan.Zero), TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                    new PersonEntity { Id = 2, FirstName = "Фролов", MiddleName = "Виссарион", LastName = "Сергеевич", Birthday = new DateTimeOffset(1999, 9, 2, 0, 0, 0, TimeSpan.Zero), TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                    new PersonEntity { Id = 3, FirstName = "Никитина", MiddleName = "Виктория", LastName = "Сергеевна", Birthday = new DateTimeOffset(1999, 3, 10, 0, 0, 0, TimeSpan.Zero), TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                    new PersonEntity { Id = 4, FirstName = "Ерофеев", MiddleName = "Ануфри", LastName = "Святославович", Birthday = new DateTimeOffset(2002, 6, 16, 0, 0, 0, TimeSpan.Zero), TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                    new PersonEntity { Id = 5, FirstName = "Беляков", MiddleName = "Лаврентий", LastName = "ВаИвановичлентинович", Birthday = new DateTimeOffset(1990, 8, 23, 0, 0, 0, TimeSpan.Zero), TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                };

                var books = new BookEntity[]
                {
                    new BookEntity { Id = 1, AuthorId = 1, Genres = new List<GenreEntity>() { genres[0] }, Name = "Мастер и Маргарита", TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                    new BookEntity { Id = 2, AuthorId = 2, Genres = new List<GenreEntity>() { genres[1] }, Name = "Мёртвые души", TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                    new BookEntity { Id = 3, AuthorId = 3, Genres = new List<GenreEntity>() { genres[2] }, Name = "Двенадцать стульев", TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                    new BookEntity { Id = 4, AuthorId = 4, Genres = new List<GenreEntity>() { genres[3] }, Name = "Собачье сердце", TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                    new BookEntity { Id = 5, AuthorId = 5, Genres = new List<GenreEntity>() { genres[4] }, Name = "Война и мир", TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, Version = 1 },
                };

                var libraryCards = new LibraryCardEntity[]
                {
                    new LibraryCardEntity { BookId = 1, PersonId = 5, TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, TimeReturn = DateTimeOffset.Now.AddDays(7), Version = 1 },
                    new LibraryCardEntity { BookId = 2, PersonId = 4, TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, TimeReturn = DateTimeOffset.Now.AddDays(7), Version = 1 },
                    new LibraryCardEntity { BookId = 3, PersonId = 3, TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, TimeReturn = DateTimeOffset.Now.AddDays(7), Version = 1 },
                    new LibraryCardEntity { BookId = 4, PersonId = 2, TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, TimeReturn = DateTimeOffset.Now.AddDays(7), Version = 1 },
                    new LibraryCardEntity { BookId = 5, PersonId = 1, TimeCreate = DateTimeOffset.Now, TimeEdit = DateTimeOffset.Now, TimeReturn = DateTimeOffset.Now.AddDays(7), Version = 1 },
                };

                context.AddRange(genres);
                context.AddRange(authors);
                context.AddRange(people);
                context.AddRange(books);
                context.AddRange(libraryCards);
                context.SaveChanges();
            }
        }

    }
}
