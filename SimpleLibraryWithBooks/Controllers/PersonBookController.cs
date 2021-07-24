using AutoMapper;
using System.Buffers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Services;
using SimpleLibraryWithBooks.Extensions;
using SimpleLibraryWithBooks.Models.Book;
using SimpleLibraryWithBooks.Models.Person;
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
        private readonly Mapper _mapperEntityToDto;

        public PersonBookController(IBookRepository bookRepository,
                                    IPeopleRepository peopleRepository,
                                    IPersonBookRepository personBookRepository)
        {
            _bookRepository = bookRepository;
            _peopleRepository = peopleRepository;
            _personBookRepository = personBookRepository;

            var configBookEntityToDto = new MapperConfiguration(cfg => cfg.CreateMap<BookEntity, BookDto>());
            var mapperBookEntityToDto = new Mapper(configBookEntityToDto);

            var configPersonEntityToDto = new MapperConfiguration(cfg => cfg.CreateMap<PersonEntity, PersonDto>());
            var mapperPersonEntityToDto = new Mapper(configPersonEntityToDto);

            var configPersonBookEntityToDto = new MapperConfiguration(cfg => cfg.CreateMap<PersonBookEntity, PersonBookDto>()
                .ForMember("Person", opt => opt.MapFrom(pb => mapperPersonEntityToDto.Map<PersonDto>(pb.Person)))
                .ForMember("Book", opt => opt.MapFrom(pb => mapperBookEntityToDto.Map<BookDto>(pb.Book))));

            _mapperEntityToDto = new Mapper(configPersonBookEntityToDto);
        }

        /// <summary>
        /// Adds a new bunch of person-book.
        /// </summary>
        /// <param name="personBook">New bunch of person-book.</param>
        /// <returns>Returns <see cref="object"/> which contains all existing elements 
        /// <see cref="PersonBookDto"/> with a new <paramref name="personBook"/>.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] PersonBookDto personBook)
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

            var buffer = GetUtf8JsonBytes(_mapperEntityToDto.Map<IEnumerable<PersonBookDto>>(_personBookRepository.GetAllPersonBooks()));
            return Ok(JsonSerializer.Deserialize<object>(buffer.WrittenSpan));
        }

        /// <summary>
        /// Creates and writes a list of <see cref="PersonBookDto"/> to the buffer.
        /// </summary>
        /// <param name="personBooks">List of <see cref="PersonBookDto"/>.</param>
        /// <returns>Buffer with a json representation of the <paramref name="personBooks"/>.</returns>
        private static ArrayBufferWriter<byte> GetUtf8JsonBytes(IEnumerable<PersonBookDto> personBooks)
        {
            var buffer = new ArrayBufferWriter<byte>();
            var writer = new Utf8JsonWriter(buffer);

            writer.WriteStartArray();
            foreach (var pb in personBooks)
            {
                WritePersonBook(writer, pb);
            }
            writer.WriteEndArray();
            writer.Flush();

            return buffer;
        }

        /// <summary>
        /// Writes a personBook to the buffer.
        /// </summary>
        /// <param name="writer">The writer with whom you need to write a <paramref name="personBook"/>.</param>
        /// <param name="personBook">The personBook to be written to the buffer.</param>
        public static void WritePersonBook(Utf8JsonWriter writer, PersonBookDto personBook)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("person");
            PersonController.WritePerson(writer, personBook.Person);

            writer.WritePropertyName("book");
            BookController.WriteBook(writer, personBook.Book);

            writer.WriteString("dateTimeReceipt", personBook.DateTimeReceipt);

            writer.WriteEndObject();
        }
    }
}
