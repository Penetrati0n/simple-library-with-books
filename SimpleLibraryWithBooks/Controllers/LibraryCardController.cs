using System;
using Mapster;
using System.Linq;
using Database.Models;
using System.Threading.Tasks;
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
        public async Task<IEnumerable<LibraryCard.Response.Debetor>> GetDebetors()
        {
            var libraryCardEntities = await _libraryCardService.GetAllAsync(lc => lc.TimeReturn < DateTimeOffset.Now);
            var libraryCardResponse = libraryCardEntities.Adapt<IEnumerable<LibraryCard.Response.Debetor>>(MapperConfigs.ForDebetor);

            return libraryCardResponse;
        }

        [HttpGet("[action]/{personId}")]
        public async Task<ActionResult<IEnumerable<Person.Response.WithBooks>>> GetPersonBooks(int personId)
        {
            if (!await _peopleService.ContainsAsync(personId))
                return BadRequest("The person does not exist");

            var libraryCardEntities = await _libraryCardService.GetAllAsync(lc => lc.PersonId == personId);
            var personResponse = libraryCardEntities.First().Person.Adapt<Person.Response.WithBooks>();
            personResponse.Books = libraryCardEntities.Adapt<IEnumerable<LibraryCard.Response.WithOutPerson>>(MapperConfigs.ForLibraryCard);

            return Ok(personResponse);
        }

        [HttpPost]
        public async Task<ActionResult<Person.Response.WithBooks>> AddLibraryCard([FromBody] LibraryCard.Request.Create libraryCardRequest)
        {
            if (!await _bookService.ContainsAsync(libraryCardRequest.BookId))
                return BadRequest("The book does not exist");
            else if (!await _peopleService.ContainsAsync(libraryCardRequest.PersonId))
                return BadRequest("The person does not exist");
            else if (await _libraryCardService.ContainsAsync(libraryCardRequest.BookId, libraryCardRequest.PersonId))
                return BadRequest("The person has already taken this book.");
            else if ((await _libraryCardService.GetAllAsync(lc => lc.PersonId == libraryCardRequest.PersonId)).Any(lc => lc.TimeReturn < DateTimeOffset.Now))
                return BadRequest("You can't get a new book until you return the old one!");

            var libraryCardEntity = libraryCardRequest.Adapt<LibraryCardEntity>();
            await _libraryCardService.InsertAsync(libraryCardEntity);
            await _libraryCardService.SaveAsync();

            var libraryCardEntities = await _libraryCardService.GetAllAsync(lc => lc.PersonId == libraryCardRequest.PersonId);
            var personResponse = libraryCardEntities.First().Person.Adapt<Person.Response.WithBooks>();
            personResponse.Books = libraryCardEntities.Adapt<IEnumerable<LibraryCard.Response.WithOutPerson>>(MapperConfigs.ForLibraryCard);

            return Ok(personResponse);
        }

        [HttpPut]
        public async Task<ActionResult<Person.Response.WithBooks>> UpdateLibraryCard([FromBody] LibraryCard.Request.Update libraryCardRequest)
        {
            if (!await _bookService.ContainsAsync(libraryCardRequest.BookId))
                return BadRequest("The book does not exist");
            else if (!await _peopleService.ContainsAsync(libraryCardRequest.PersonId))
                return BadRequest("The person does not exist");
            else if (!await _libraryCardService.ContainsAsync(libraryCardRequest.BookId, libraryCardRequest.PersonId))
                return NotFound();

            var libraryCardEntity = libraryCardRequest.Adapt<LibraryCardEntity>();
            libraryCardEntity.TimeReturn = libraryCardEntity.TimeReturn.AddDays(libraryCardRequest.AddedDays);
            await _libraryCardService.UpdateAsync(libraryCardEntity);
            await _libraryCardService.SaveAsync();

            libraryCardEntity = await _libraryCardService.GetAsync(libraryCardRequest.BookId, libraryCardRequest.PersonId);
            var personResponse = libraryCardEntity.Person.Adapt<Person.Response.WithBook>();
            personResponse.Book = libraryCardEntity.Adapt<LibraryCard.Response.WithOutPerson>(MapperConfigs.ForLibraryCard);

            return Ok(personResponse);
        }

        [HttpDelete("{bookId}&{personId}")]
        public async Task<ActionResult> DeleteLibraryCard(int bookId, int personId)
        {
            if (!await _bookService.ContainsAsync(bookId))
                return BadRequest("The book does not exist");
            else if (!await _peopleService.ContainsAsync(personId))
                return BadRequest("The person does not exist");
            else if (!await _libraryCardService.ContainsAsync(bookId, personId))
                return NotFound();

            await _libraryCardService.DeleteAsync(bookId, personId);
            await _libraryCardService.SaveAsync();

            return Ok();
        }
    }
}
