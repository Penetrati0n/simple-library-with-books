using System;
using Mapster;
using System.Linq;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Common.DataTransferModels;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Options;
using Infrastructure.Services.Interfaces;

namespace SimpleLibraryWithBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryCardController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IPeopleService _peopleService;
        private readonly ILibraryCardService _libraryCardService;

        public LibraryCardController(IBookService bookService, IPeopleService peopleService, ILibraryCardService libraryCardService)
        {
            _bookService = bookService;
            _peopleService = peopleService;
            _libraryCardService = libraryCardService;
        }

        [HttpGet]
        public IEnumerable<LibraryCard.Response.Debetor> GetDebetors()
        {
            var libraryCardEntities = _libraryCardService.GetAll(lc => lc.TimeReturn < DateTimeOffset.Now);
            var libraryCardResponse = libraryCardEntities.Adapt<IEnumerable<LibraryCard.Response.Debetor>>(MapperConfigs.ForDebetor);

            return libraryCardResponse;
        }

        [HttpGet("[action]/{personId}")]
        public ActionResult<IEnumerable<Person.Response.WithBooks>> GetPersonBooks(int personId)
        {
            if (!_peopleService.Contains(personId))
                return BadRequest("The person does not exist");

            var libraryCardEntities = _libraryCardService.GetAll(lc => lc.PersonId == personId);
            var personResponse = libraryCardEntities.First().Person.Adapt<Person.Response.WithBooks>();
            personResponse.Books = libraryCardEntities.Adapt<IEnumerable<LibraryCard.Response.WithOutPerson>>(MapperConfigs.ForLibraryCard);

            return Ok(personResponse);
        }

        [HttpPost]
        public ActionResult<Person.Response.WithBooks> AddLibraryCard([FromBody] LibraryCard.Request.Create libraryCardRequest)
        {
            if (!_bookService.Contains(libraryCardRequest.BookId))
                return BadRequest("The book does not exist");
            else if (!_peopleService.Contains(libraryCardRequest.PersonId))
                return BadRequest("The person does not exist");
            else if (_libraryCardService.Contains(libraryCardRequest.BookId, libraryCardRequest.PersonId))
                return BadRequest("The person has already taken this book.");
            else if (_libraryCardService.GetAll(lc => lc.PersonId == libraryCardRequest.PersonId).Any(lc => lc.TimeReturn < DateTimeOffset.Now))
                return BadRequest("You can't get a new book until you return the old one!");

            var libraryCardEntity = libraryCardRequest.Adapt<LibraryCardEntity>();
            _libraryCardService.Insert(libraryCardEntity);
            _libraryCardService.Save();

            var libraryCardEntities = _libraryCardService.GetAll(lc => lc.PersonId == libraryCardRequest.PersonId);
            var personResponse = libraryCardEntities.First().Person.Adapt<Person.Response.WithBooks>();
            personResponse.Books = libraryCardEntities.Adapt<IEnumerable<LibraryCard.Response.WithOutPerson>>(MapperConfigs.ForLibraryCard);

            return Ok(personResponse);
        }

        [HttpPut]
        public ActionResult<Person.Response.WithBooks> UpdateLibraryCard([FromBody] LibraryCard.Request.Update libraryCardRequest)
        {
            if (!_bookService.Contains(libraryCardRequest.BookId))
                return BadRequest("The book does not exist");
            else if (!_peopleService.Contains(libraryCardRequest.PersonId))
                return BadRequest("The person does not exist");
            else if (!_libraryCardService.Contains(libraryCardRequest.BookId, libraryCardRequest.PersonId))
                return NotFound();

            var libraryCardEntity = libraryCardRequest.Adapt<LibraryCardEntity>();
            libraryCardEntity.TimeReturn = libraryCardEntity.TimeReturn.AddDays(libraryCardRequest.AddedDays);
            _libraryCardService.Update(libraryCardEntity);
            _libraryCardService.Save();

            _libraryCardService.Get(libraryCardRequest.BookId, libraryCardRequest.PersonId);
            var personResponse = libraryCardEntity.Person.Adapt<Person.Response.WithBook>();
            personResponse.Book = libraryCardEntity.Adapt<LibraryCard.Response.WithOutPerson>(MapperConfigs.ForLibraryCard);

            return Ok(personResponse);
        }

        [HttpDelete("{bookId}&{personId}")]
        public ActionResult DeleteLibraryCard(int bookId, int personId)
        {
            if (!_bookService.Contains(bookId))
                return BadRequest("The book does not exist");
            else if (!_peopleService.Contains(personId))
                return BadRequest("The person does not exist");
            else if (!_libraryCardService.Contains(bookId, personId))
                return NotFound();

            _libraryCardService.Delete(bookId, personId);
            _libraryCardService.Save();

            return Ok();
        }
    }
}
