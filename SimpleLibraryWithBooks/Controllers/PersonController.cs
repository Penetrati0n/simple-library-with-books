using System.Linq;
using System.Buffers;
using System.Text.Json;
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
        /// <returns>Returns <see cref="object"/> which contains all existing elements <see cref="PersonModel"/>
        /// with a new <paramref name="person"/> without <c>Birthday</c> property.</returns>
        [HttpPost]
        public object Post([FromBody] PersonModel person)
        {
            person.Birthday = person.Birthday.SubstringTicks().ChangeTimeZone();
            PeopleRepository.People.Add(person);

            var buffer = GetUtf8JsonBytes(PeopleRepository.People);

            return JsonSerializer.Deserialize<object>(buffer.WrittenSpan);
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

        /// <summary>
        /// Creates and writes a list of <see cref="PersonModel"/> to the buffer.
        /// </summary>
        /// <param name="people">List of <see cref="PersonModel"/>.</param>
        /// <returns>Buffer with a json representation of the <paramref name="people"/>.</returns>
        private static ArrayBufferWriter<byte> GetUtf8JsonBytes(IEnumerable<PersonModel> people)
        {
            var buffer = new ArrayBufferWriter<byte>();
            var writer = new Utf8JsonWriter(buffer);

            writer.WriteStartArray();
            foreach (var p in people)
            {
                WritePerson(writer, p);
            }
            writer.WriteEndArray();
            writer.Flush();

            return buffer;
        }

        /// <summary>
        /// Writes a person to the buffer.
        /// </summary>
        /// <param name="writer">The writer with which to record the <paramref name="person"/>.</param>
        /// <param name="person">The person to be written to the buffer.</param>
        public static void WritePerson(Utf8JsonWriter writer, PersonModel person)
        {
            writer.WriteStartObject();
            writer.WriteString("lastName", person.LastName);
            writer.WriteString("firstName", person.FirstName);
            writer.WriteString("patronymic", person.Patronymic);
            writer.WriteEndObject();
        }
    }
}
