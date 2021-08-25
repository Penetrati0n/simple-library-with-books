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
    public class PersonController : Controller
    {
        private readonly IPeopleService _peopleService;
        private readonly ILibraryCardService _libraryCardService;

        public PersonController(IPeopleService peopleService, ILibraryCardService libraryCardService)
        {
            _peopleService = peopleService;
            _libraryCardService = libraryCardService;
        }

        [HttpGet]
        public async Task<IEnumerable<Person.Response>> GetPeople()
        {
            var personEntities = await _peopleService.GetAllAsync();
            var personReponse = personEntities.Adapt<IEnumerable<Person.Response>>();

            return personReponse;
        }

        [HttpGet("{firstName}&{lastName}&{middleName}")]
        public async Task<IEnumerable<Person.Response>> GetPeople(string firstName, string middleName, string lastName)
        {
            var personEntities = await _peopleService.GetAllAsync(firstName, middleName, lastName);
            var personReponse = personEntities.Adapt<IEnumerable<Person.Response>>();

            return personReponse;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person.Response>> GetPerson(int id)
        {
            if (!await _peopleService.ContainsAsync(id))
                return NotFound();

            var personEntity = await _peopleService.GetAsync(id);
            var personResponse = personEntity.Adapt<Person.Response>();

            return Ok(personResponse);
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
        public async Task<ActionResult<Person.Response>> AddPerson([FromBody] Person.Request.Create personRequest)
        {
            if (await _peopleService.ContainsAsync(personRequest.FirstName, personRequest.MiddleName, personRequest.LastName, personRequest.Birthday))
                return BadRequest("The person already exists.");

            var personEntity = personRequest.Adapt<PersonEntity>();
            await _peopleService.InsertAsync(personEntity);
            await _peopleService.SaveAsync();

            var personResponse = personEntity.Adapt<Person.Response>();
            
            return Ok(personResponse);
        }

        [HttpPut]
        public async Task<ActionResult<Person.Response>> UpdatePerson([FromBody] Person.Request.Update personRequest)
        {
            if (!await _peopleService.ContainsAsync(personRequest.Id))
                return NotFound();

            var personEntity = personRequest.Adapt<PersonEntity>();
            await _peopleService.UpdateAsync(personEntity);
            await _peopleService.SaveAsync();

            var personResponse = personEntity.Adapt<Person.Response>();

            return personResponse;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePerson(int id)
        {
            if (!await _peopleService.ContainsAsync(id))
                return NotFound();

            await _peopleService.DeleteAsync(id);
            await _peopleService.SaveAsync();

            return Ok();
        }

        [HttpDelete("{firstName}&{middleName}&{lastName}")]
        public async Task<ActionResult> DeletePeople(string firstName, string middleName, string lastName)
        {
            if (!await _peopleService.ContainsAsync(firstName, middleName, lastName))
                return NotFound();

            await _peopleService.DeleteAsync(firstName, middleName, lastName);
            await _peopleService.SaveAsync();

            return Ok();
        }
    }
}
