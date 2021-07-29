using System;
using Database;
using System.Linq;
using Database.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SimpleLibraryWithBooks.Services
{
    public class PersonBookRepository : IPersonBookRepository
    {
        private readonly DatabaseContext _context;

        public PersonBookRepository(DbContextOptions<DatabaseContext> option) =>
            _context = new DatabaseContext(option);

        public IEnumerable<PersonBookEntity> GetAllPersonBooks() =>
            _context.PersonBooks
                .Include(pb => pb.Person)
                .Include(pb => pb.Book);

        public IEnumerable<PersonBookEntity> GetAllPersonBooks(Func<PersonBookEntity, bool> rule) =>
            _context.PersonBooks.Where(rule);


        public PersonBookEntity GetPersonBook(string lastName, string firtsName, string patronymic, string title, string author) =>
            _context.PersonBooks
                .Include(pb => pb.Person)
                .Include(pb => pb.Book)
                .Single(pb => pb.Person.LastName == lastName &&
                              pb.Person.FirstName == firtsName &&
                              pb.Person.Patronymic == patronymic &&
                              pb.Book.Title == title &&
                              pb.Book.Author == author);

        public void InsertPersonBook(PersonBookEntity personBook)
        {
            var person = personBook.Person;
            var book = personBook.Book;

            personBook.Person = null;
            personBook.Book = null;

            _context.PersonBooks.Add(personBook);

            personBook.Person = person;
            personBook.Book = book;
        }

        public void UpdatePersonBook(PersonBookEntity personBook) =>
            _context.Entry(personBook).State = EntityState.Modified;

        public void DeletePersonBook(string lastName, string firtsName, string patronymic, string title, string author) =>
            _context.PersonBooks.Remove(GetPersonBook(lastName, firtsName, patronymic, title, author));

        public void Save() =>
            _context.SaveChanges();

        public bool Contains(string lastName, string firtsName, string patronymic, string title, string author) =>
            _context.PersonBooks.Any(pb => pb.Person.LastName == lastName &&
                                           pb.Person.FirstName == firtsName &&
                                           pb.Person.Patronymic == patronymic &&
                                           pb.Book.Title == title &&
                                           pb.Book.Author == author);
    }
}
