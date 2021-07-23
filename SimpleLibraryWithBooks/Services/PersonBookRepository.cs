using System;
using System.Linq;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Models.PersonBook;

namespace SimpleLibraryWithBooks.Services
{
    public class PersonBookRepository : IPersonBookRepository
    {
        private readonly static List<PersonBookModel> _personBooks = new List<PersonBookModel>();

        public IEnumerable<PersonBookModel> GetAllPersonBooks() =>
            _personBooks;

        public IEnumerable<PersonBookModel> GetAllPersonBooks(Func<PersonBookModel, bool> rule) =>
            _personBooks.Where(rule);

        public PersonBookModel GetPersonBook(int personBookId) =>
            _personBooks[personBookId];

        public PersonBookModel GetPersonBook(string lastName, string firtsName, string patronymic, string title, string author) =>
            _personBooks.Single(pb => pb.Person.LastName == lastName &&
                                      pb.Person.FirstName == firtsName &&
                                      pb.Person.Patronymic == patronymic &&
                                      pb.Book.Title == title &&
                                      pb.Book.Author == author);

        public void InsertPersonBook(PersonBookModel personBook) =>
            _personBooks.Add(personBook);

        public void UpdatePersonBook(PersonBookModel personBook) { }

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
