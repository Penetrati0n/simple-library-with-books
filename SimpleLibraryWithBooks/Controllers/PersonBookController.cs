using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Services;
using SimpleLibraryWithBooks.Extensions;
using SimpleLibraryWithBooks.Models.PersonBook;

namespace SimpleLibraryWithBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonBookController : Controller
    {
        /// <summary>
        /// Adds a new bunch of person-book.
        /// </summary>
        /// <param name="personBook">New bunch of person-book.</param>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of the type <see cref="PersonBookModel"/>, 
        /// which contains all existing elements with a new <paramref name="personBook"/>.</returns>
        [HttpPost]
        public IEnumerable<PersonBookModel> Post([FromBody]PersonBookModel personBook)
        {
            var personFromBody = personBook.Person;
            var bookFromBody = personBook.Book;

            var person = PeopleRepository.People.Single(p => p.LastName == personFromBody.LastName &&
                                                             p.FirstName == personFromBody.FirstName &&
                                                             p.Patronymic == personFromBody.Patronymic &&
                                                             p.Birthday == personFromBody.Birthday);
            
            var book = BookRepository.Books.Single(b => b.Title == bookFromBody.Title &&
                                                        b.Author == bookFromBody.Author &&
                                                        b.Genre == bookFromBody.Genre);
            personBook.Person = person;
            personBook.Book = book;
            personBook.DateTimeReceipt = personBook.DateTimeReceipt.SubstringTicks().ChangeTimeZone();
            
            PersonBookRepository.PersonBooks.Add(personBook);

            return PersonBookRepository.PersonBooks;
        }
    }
}
