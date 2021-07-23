using System.Buffers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Services;
using SimpleLibraryWithBooks.Models.Book;

namespace SimpleLibraryWithBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository) =>
            _bookRepository = bookRepository;

        /// <summary>
        /// Get full list of books.
        /// </summary>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of the type <see cref="BookModel"/>, 
        /// which contains all existing elements.</returns>
        [HttpGet]
        public IEnumerable<BookModel> Get() =>
            _bookRepository.GetAllBooks();

        /// <summary>
        /// Get a list of books with a given author.
        /// </summary>
        /// <param name="author">Author of the book.</param>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of type <see cref="BookModel"/>,
        /// in which there are elements in which the author equal <paramref name="author"/>.</returns>
        [HttpGet("{author}")]
        public IEnumerable<BookModel> Get(string author) =>
            _bookRepository.GetAllBooks(b => b.Author == author);

        /// <summary>
        /// Adds a new book.
        /// </summary>
        /// <param name="book">New book.</param>
        /// <returns>Returns <see cref="object"/> which contains all existing elements <see cref="BookModel"/>
        /// with a new <paramref name="book"/> without <c>Genre</c> property.</returns>
        [HttpPost]
        public object Post([FromBody] BookModel book)
        {
            _bookRepository.InsertBook(book);
            _bookRepository.Save();

            var buffer = GetUtf8JsonBytes(_bookRepository.GetAllBooks());

            return JsonSerializer.Deserialize<object>(buffer.WrittenSpan);
        }

        /// <summary>
        /// Deletes a book.
        /// </summary>
        /// <param name="title">Book title.</param>
        /// <param name="author">Author of the book.</param>
        /// <returns><list type="bullet">
        /// <item><term><see cref="OkResult"/></term><description> the book was successfully deleted.</description></item>
        /// <item><term><see cref="NotFoundResult"/></term><description> the book was not found</description></item>
        /// </list></returns>
        [HttpDelete("{title}&{author}")]
        public IActionResult Delete(string title, string author)
        {
            if (!_bookRepository.Contains(title, author))
                return NotFound();

            _bookRepository.DeleteBook(title, author);
            _bookRepository.Save();

            return Ok();
        }

        /// <summary>
        /// Creates and writes a list of <see cref="BookModel"/> to the buffer.
        /// </summary>
        /// <param name="books">List of <see cref="BookModel"/>.</param>
        /// <returns>Buffer with a json representation of the <paramref name="books"/>.</returns>
        private static ArrayBufferWriter<byte> GetUtf8JsonBytes(IEnumerable<BookModel> books)
        {
            var buffer = new ArrayBufferWriter<byte>();
            var writer = new Utf8JsonWriter(buffer);

            writer.WriteStartArray();
            foreach (var b in books)
            {
                WriteBook(writer, b);
            }
            writer.WriteEndArray();
            writer.Flush();

            return buffer;
        }

        /// <summary>
        /// Writes a book to the buffer.
        /// </summary>
        /// <param name="writer">The writer with whom you need to write a <paramref name="book"/>.</param>
        /// <param name="book">The book to be written to the buffer.</param>
        public static void WriteBook(Utf8JsonWriter writer, BookModel book)
        {
            writer.WriteStartObject();
            writer.WriteString("title", book.Title);
            writer.WriteString("author", book.Author);
            writer.WriteEndObject();
        }
    }
}
