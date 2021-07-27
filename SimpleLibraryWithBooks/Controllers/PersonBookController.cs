using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Options;
using SimpleLibraryWithBooks.Services;
using SimpleLibraryWithBooks.Extensions;
using SimpleLibraryWithBooks.Models.PersonBook;

namespace SimpleLibraryWithBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonBookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IPeopleRepository _peopleRepository;
        private readonly IPersonBookRepository _personBookRepository;

        public PersonBookController(IBookRepository bookRepository,
                                    IPeopleRepository peopleRepository,
                                    IPersonBookRepository personBookRepository)
        {
            _bookRepository = bookRepository;
            _peopleRepository = peopleRepository;
            _personBookRepository = personBookRepository;
        }

        /// <summary>
        /// Adds a new bunch of person-book.
        /// </summary>
        /// <param name="personBook">New bunch of person-book.</param>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of type <see cref="PersonBookRequestDto"/>,
        /// which contains all existing elements with a new <paramref name="personBook"/>.</returns>
        [HttpPost]
        public ActionResult<IEnumerable<PersonBookResponseDto>> Post([FromBody] PersonBookRequestDto personBook)
        {
            var personFromBody = personBook.Person;
            var bookFromBody = personBook.Book;

            if (!_peopleRepository.Contains(personFromBody.LastName, personFromBody.FirstName, personFromBody.Patronymic))
                return BadRequest("Person is not found.");
            if (!_bookRepository.Contains(bookFromBody.Title, bookFromBody.Author))
                return BadRequest("Book is not found.");

            var person = _peopleRepository.GetPerson(personFromBody.LastName, personFromBody.FirstName, personFromBody.Patronymic);
            var book = _bookRepository.GetBook(bookFromBody.Title, bookFromBody.Author);

            personBook.DateTimeReceipt = personBook.DateTimeReceipt.SubstringTicks().ChangeTimeZone();
            var personBookEntity = new PersonBookEntity()
            {
                Person = person,
                Book = book,
                DateTimeReceipt = personBook.DateTimeReceipt
            };
            _personBookRepository.InsertPersonBook(personBookEntity);
            _personBookRepository.Save();

            var responsePersonBooks = _personBookRepository
                .GetAllPersonBooks()
                .Adapt<IEnumerable<PersonBookResponseDto>>(MapperConfigs.ForPersonBooks);

            return Json(responsePersonBooks, SerializerOptions.WhenWritingDefault);
        }
    }
}
