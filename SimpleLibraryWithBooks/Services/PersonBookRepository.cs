using System;
using System.Linq;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Models.PersonBook;

namespace SimpleLibraryWithBooks.Services
{
    public class PersonBookRepository : IPersonBookRepository
    {
        private readonly static List<PersonBookEntity> _personBooks = new List<PersonBookEntity>();

        private static int _currentId = 0;

        public IEnumerable<PersonBookEntity> GetAllPersonBooks() =>
            _personBooks;

        public IEnumerable<PersonBookEntity> GetAllPersonBooks(Func<PersonBookEntity, bool> rule) =>
            _personBooks.Where(rule);

        public PersonBookEntity GetPersonBook(int personBookId) =>
            _personBooks[personBookId];

        public PersonBookEntity GetPersonBook(string lastName, string firtsName, string patronymic, string title, string author) =>
            _personBooks.Single(pb => pb.Person.LastName == lastName &&
                                      pb.Person.FirstName == firtsName &&
                                      pb.Person.Patronymic == patronymic &&
                                      pb.Book.Title == title &&
                                      pb.Book.Author == author);

        public void InsertPersonBook(PersonBookEntity personBook)
        {
            _currentId++;
            personBook.Id = _currentId;
            
            _personBooks.Add(personBook);
        }

        public void UpdatePersonBook(PersonBookEntity personBook) { }

        public void DeletePersonBook(int personBookId) =>
            _personBooks.RemoveAt(personBookId);

        public void DeletePersonBook(string lastName, string firtsName, string patronymic, string title, string author)
        {
            var personBook = GetPersonBook(lastName, firtsName, patronymic, title, author);
            _personBooks.Remove(personBook);
        }

        public void Save() { }

        public bool Contains(string lastName, string firtsName, string patronymic, string title, string author) =>
            _personBooks.Any(pb => pb.Person.LastName == lastName &&
                                      pb.Person.FirstName == firtsName &&
                                      pb.Person.Patronymic == patronymic &&
                                      pb.Book.Title == title &&
                                      pb.Book.Author == author);
    }
}
