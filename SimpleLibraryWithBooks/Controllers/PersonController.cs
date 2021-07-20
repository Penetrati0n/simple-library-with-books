using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Services;
using SimpleLibraryWithBooks.Extensions;
using SimpleLibraryWithBooks.Models.Person;

namespace SimpleLibraryWithBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : Controller
    {
        /// <summary>
        /// Get full list of people.
        /// </summary>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of the type <see cref="PersonModel"/>, 
        /// which contains all existing elements.</returns>
        [HttpGet]
        public IEnumerable<PersonModel> Get() => PeopleRepository.People;

        /// <summary>
        /// Get a list of people with a given name.
        /// </summary>
        /// <param name="name">The person's name.</param>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of type <see cref="PersonModel"/>,
        /// in which there are elements in which the name equal <paramref name="name"/>.</returns>
        [HttpGet("{name}")]
        public IEnumerable<PersonModel> Get(string name) => PeopleRepository.People.Where(p => p.FirstName == name);

        /// <summary>
        /// Adds a new person.
        /// </summary>
        /// <param name="person">New person.</param>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of the type <see cref="PersonModel"/>, 
        /// which contains all existing elements with a new <paramref name="person"/>.</returns>
        [HttpPost]
        public IEnumerable<PersonModel> Post([FromBody] PersonModel person)
        {
            person.Birthday = person.Birthday.SubstringTicks().ChangeTimeZone();
            PeopleRepository.People.Add(person);

            return PeopleRepository.People;
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
            var person = PeopleRepository.People
                .SingleOrDefault(p => p.LastName == lastName && p.FirstName == firstName && p.Patronymic == patronymic);

            if (person == null) return NotFound();

            PeopleRepository.People.Remove(person);

            return Ok();
        }
    }
}
