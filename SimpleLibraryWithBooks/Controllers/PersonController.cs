using Mapster;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Options;
using SimpleLibraryWithBooks.Services;
using SimpleLibraryWithBooks.Extensions;
using SimpleLibraryWithBooks.Models.Person;

namespace SimpleLibraryWithBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : Controller
    {
        private readonly IPeopleRepository _peopleRepository;

        public PersonController(IPeopleRepository peopleRepository)
        {
            _peopleRepository = peopleRepository;
        }

        /// <summary>
        /// Get full list of people.
        /// </summary>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of the type <see cref="PersonResponseDto"/>, 
        /// which contains all existing elements.</returns>
        [HttpGet]
        public IEnumerable<PersonResponseDto> Get()
        {
            var people = _peopleRepository.GetAllPeople().Adapt<IEnumerable<PersonResponseDto>>();

            return people;
        }

        /// <summary>
        /// Get a list of people with a given name.
        /// </summary>
        /// <param name="name">The person's name.</param>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of type <see cref="PersonResponseDto"/>,
        /// in which there are elements in which the name equal <paramref name="name"/>.</returns>
        [HttpGet("{name}")]
        public IEnumerable<PersonResponseDto> Get(string name)
        {
            var people = _peopleRepository.GetAllPeople(p => p.FirstName == name).Adapt<IEnumerable<PersonResponseDto>>();

            return people;
        }

        /// <summary>
        /// Adds a new person.
        /// </summary>
        /// <param name="person">New person.</param>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of type <see cref="PersonResponseDto"/>,
        /// which contains all existing elements with a new <paramref name="person"/> without <c>Birthday</c> property.</returns>
        [HttpPost]
        public ActionResult<IEnumerable<PersonResponseDto>> Post([FromBody] PersonRequestDto person)
        {
            if (_peopleRepository.Contains(person.LastName, person.FirstName, person.Patronymic))
                return BadRequest("The person already exists.");

            person.Birthday = person.Birthday.SubstringTicks().ChangeTimeZone();
            _peopleRepository.InsertPerson(person.Adapt<PersonEntity>());
            _peopleRepository.Save();

            var responsePeople = _peopleRepository.GetAllPeople().Adapt<IEnumerable<PersonResponseDto>>(MapperConfigs.ForPeople);

            return Json(responsePeople, SerializerOptions.WhenWritingDefault);
        }

        /// <summary>
        /// Deletes a person.
        /// </summary>
        /// <param name="lastName">Last name of person.</param>
        /// <param name="firstName">First name of person.</param>
        /// <param name="patronymic">Patronymic name of person.</param>
        /// <returns><list type="bullet">
        /// <item><term><see cref="OkResult"/></term><description> the person was successfully deleted.</description></item>
        /// <item><term><see cref="NotFoundResult"/></term><description> the person was not found</description></item>
        /// </list></returns>
        [HttpDelete("{lastName}&{firstName}&{patronymic}")]
        public IActionResult Delete(string lastName, string firstName, string patronymic)
        {
            if (!_peopleRepository.Contains(lastName, firstName, patronymic))
                return NotFound();

            _peopleRepository.DeletePerson(lastName, firstName, patronymic);
            _peopleRepository.Save();

            return Ok();
        }
    }
}
