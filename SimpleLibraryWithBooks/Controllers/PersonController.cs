using Mapster;
using System.Linq;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Models;
using SimpleLibraryWithBooks.Options;
using SimpleLibraryWithBooks.Services;

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
        public IEnumerable<Person.Response> GetPeople()
        {
            var personEntities = _peopleService.GetAll();
            var personReponse = personEntities.Adapt<IEnumerable<Person.Response>>();

            return personReponse;
        }

        [HttpGet("{firstName}&{lastName}&{middleName}")]
        public IEnumerable<Person.Response> GetPeople(string firstName, string middleName, string lastName)
        {
            var personEntities = _peopleService.GetAll(firstName, middleName, lastName);
            var personReponse = personEntities.Adapt<IEnumerable<Person.Response>>();

            return personReponse;
        }

        [HttpGet("{id}")]
        public ActionResult<Person.Response> GetPerson(int id)
        {
            if (!_peopleService.Contains(id))
                return NotFound();

            var personEntity = _peopleService.Get(id);
            var personResponse = personEntity.Adapt<Person.Response>();

            return Ok(personResponse);
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
        public ActionResult<Person.Response> AddPerson([FromBody] Person.Request.Create personRequest)
        {
            if (_peopleService.Contains(personRequest.FirstName, personRequest.MiddleName, personRequest.LastName, personRequest.Birthday))
                return BadRequest("The person already exists.");

            var personEntity = personRequest.Adapt<PersonEntity>();
            _peopleService.Insert(personEntity);
            _peopleService.Save();

            var personResponse = personEntity.Adapt<Person.Response>();
            
            return Ok(personResponse);
        }

        [HttpPut]
        public ActionResult<Person.Response> UpdatePerson([FromBody] Person.Request.Update personRequest)
        {
            if (!_peopleService.Contains(personRequest.Id))
                return NotFound();

            var personEntity = personRequest.Adapt<PersonEntity>();
            _peopleService.Update(personEntity);
            _peopleService.Save();

            var personResponse = personEntity.Adapt<Person.Response>();

            return personResponse;
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePerson(int id)
        {
            if (!_peopleService.Contains(id))
                return NotFound();

            _peopleService.Delete(id);
            _peopleService.Save();

            return Ok();
        }

        [HttpDelete("{firstName}&{middleName}&{lastName}")]
        public ActionResult DeletePeople(string firstName, string middleName, string lastName)
        {
            if (!_peopleService.Contains(firstName, middleName, lastName))
                return NotFound();

            _peopleService.Delete(firstName, middleName, lastName);
            _peopleService.Save();

            return Ok();
        }
    }
}
