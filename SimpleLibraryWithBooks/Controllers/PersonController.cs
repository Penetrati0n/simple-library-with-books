using AutoMapper;
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
        private readonly IPeopleRepository _peopleRepository;
        private readonly Mapper _mapperEntityToDto;
        private readonly Mapper _mapperDtoToEntity;

        public PersonController(IPeopleRepository peopleRepository)
        {
            _peopleRepository = peopleRepository;

            var configEntityToDto = new MapperConfiguration(cfg => cfg.CreateMap<PersonEntity, PersonDto>());
            _mapperEntityToDto = new Mapper(configEntityToDto);

            var configDtoToEntity = new MapperConfiguration(cfg => cfg.CreateMap<PersonDto, PersonEntity>());
            _mapperDtoToEntity = new Mapper(configDtoToEntity);
        }

        /// <summary>
        /// Get full list of people.
        /// </summary>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of the type <see cref="PersonDto"/>, 
        /// which contains all existing elements.</returns>
        [HttpGet]
        public IEnumerable<PersonDto> Get()
        {
            var people = _mapperEntityToDto.Map<IEnumerable<PersonDto>>(_peopleRepository.GetAllPeople());

            return people;
        }

        /// <summary>
        /// Get a list of people with a given name.
        /// </summary>
        /// <param name="name">The person's name.</param>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of type <see cref="PersonDto"/>,
        /// in which there are elements in which the name equal <paramref name="name"/>.</returns>
        [HttpGet("{name}")]
        public IEnumerable<PersonDto> Get(string name)
        {
            var people = _mapperEntityToDto.Map<IEnumerable<PersonDto>>(_peopleRepository.GetAllPeople(p => p.FirstName == name));

            return people;
        }

        /// <summary>
        /// Adds a new person.
        /// </summary>
        /// <param name="person">New person.</param>
        /// <returns>Returns <see cref="object"/> which contains all existing elements <see cref="PersonDto"/>
        /// with a new <paramref name="person"/> without <c>Birthday</c> property.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] PersonDto person)
        {
            person.Birthday = person.Birthday.SubstringTicks().ChangeTimeZone();
            var personEntity = _mapperDtoToEntity.Map<PersonEntity>(person);
            _peopleRepository.InsertPerson(personEntity);
            _peopleRepository.Save();

            var buffer = GetUtf8JsonBytes(_mapperEntityToDto.Map<IEnumerable<PersonDto>>(_peopleRepository.GetAllPeople()));

            return Ok(JsonSerializer.Deserialize<object>(buffer.WrittenSpan));
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

        /// <summary>
        /// Creates and writes a list of <see cref="PersonDto"/> to the buffer.
        /// </summary>
        /// <param name="people">List of <see cref="PersonDto"/>.</param>
        /// <returns>Buffer with a json representation of the <paramref name="people"/>.</returns>
        private static ArrayBufferWriter<byte> GetUtf8JsonBytes(IEnumerable<PersonDto> people)
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
        public static void WritePerson(Utf8JsonWriter writer, PersonDto person)
        {
            writer.WriteStartObject();
            writer.WriteString("lastName", person.LastName);
            writer.WriteString("firstName", person.FirstName);
            writer.WriteString("patronymic", person.Patronymic);
            writer.WriteEndObject();
        }
    }
}
