using System.Linq;
using System.Buffers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Services;
using SimpleLibraryWithBooks.Extensions;
using SimpleLibraryWithBooks.Models.PersonBook;

namespace SimpleLibraryWithBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonBookController : Controller
    {
        /// <summary>
        /// Adds a new bunch of person-book.
        /// </summary>
        /// <param name="personBook">New bunch of person-book.</param>
        /// <returns>Returns <see cref="object"/> which contains all existing elements 
        /// <see cref="PersonBookModel"/> with a new <paramref name="personBook"/>.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] PersonBookModel personBook)
        {
            var personFromBody = personBook.Person;
            var bookFromBody = personBook.Book;

            if (!PeopleRepository.People.Contains(personFromBody))
                return BadRequest("Person is not found.");
            if (!BookRepository.Books.Contains(bookFromBody))
                return BadRequest("Book is not found.");

            var person = PeopleRepository.People.Single(p => personFromBody.Equals(p));
            var book = BookRepository.Books.Single(b => bookFromBody.Equals(b));

            personBook.Person = person;
            personBook.Book = book;
            personBook.DateTimeReceipt = personBook.DateTimeReceipt.SubstringTicks().ChangeTimeZone();
            PersonBookRepository.PersonBooks.Add(personBook);

            var buffer = GetUtf8JsonBytes(PersonBookRepository.PersonBooks);
            return Ok(JsonSerializer.Deserialize<object>(buffer.WrittenSpan));
        }

        /// <summary>
        /// Creates and writes a list of <see cref="PersonBookModel"/> to the buffer.
        /// </summary>
        /// <param name="personBooks">List of <see cref="PersonBookModel"/>.</param>
        /// <returns>Buffer with a json representation of the <paramref name="personBooks"/>.</returns>
        private static ArrayBufferWriter<byte> GetUtf8JsonBytes(IEnumerable<PersonBookModel> personBooks)
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
        public static void WritePersonBook(Utf8JsonWriter writer, PersonBookModel personBook)
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
