using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Models;

namespace SimpleLibraryWithBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : Controller
    {
        private static readonly List<PersonDto> _people = new List<PersonDto>()
        {
            new PersonDto() { LastName = "Кармазин", FirstName = "Лев", Patronymic = "Олегович", Birthday = DateTime.Parse("12/03/1983")},
            new PersonDto() { LastName = "Тихомиров", FirstName = "Филипп", Patronymic = "Михайлович", Birthday = DateTime.Parse("17/09/1980")},
            new PersonDto() { LastName = "Травникова", FirstName = "Мариетта", Patronymic = "Платоновна", Birthday = DateTime.Parse("03/07/2001")},
        };

        /// <summary>
        /// Get full list of people.
        /// </summary>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of the type <see cref="PersonDto"/>, 
        /// which contains all existing elements.</returns>
        [HttpGet]
        public IEnumerable<PersonDto> Get() => _people;

        /// <summary>
        /// Get a list of people with a given name.
        /// </summary>
        /// <param name="name">The person's name.</param>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of type <see cref="PersonDto"/>,
        /// in which there are elements in which the name equal <paramref name="name"/>.</returns>
        [HttpGet("{name}")]
        public IEnumerable<PersonDto> Get(string name) => _people.Where(p => p.FirstName == name);

        /// <summary>
        /// Adds a new person.
        /// </summary>
        /// <param name="person">New person.</param>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of the type <see cref="PersonDto"/>, 
        /// which contains all existing elements with a new <paramref name="person"/>.</returns>
        [HttpPost]
        public IEnumerable<PersonDto> Post([FromBody]PersonDto person)
        {
            _people.Add(person);

            return _people;
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
        [HttpDelete]
        public IActionResult Delete(string lastName, string firstName, string patronymic)
        {
            var person = _people.SingleOrDefault(p => p.LastName == lastName && p.FirstName == firstName && p.Patronymic == patronymic);

            if (person == null) return NotFound();

            _people.Remove(person);

            return Ok();
        }
    }
}
