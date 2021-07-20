using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Models;

namespace SimpleLibraryWithBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private static readonly List<BookDto> _books = new List<BookDto>()
        {
            new BookDto() { Title  = "Горе от ума", Author = "Александр Грибоедов", Genre = "Комедия" },
            new BookDto() { Title  = "Гордость и предубеждение", Author = "Джейн Остин", Genre = "Роман" },
            new BookDto() { Title  = "Тёмные начала", Author = "Филип Пулман", Genre = "Фэнтези" },
        };

        /// <summary>
        /// Get full list of books.
        /// </summary>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of the type <see cref="BookDto"/>, 
        /// which contains all existing elements.</returns>
        [HttpGet]
        public IEnumerable<BookDto> Get() => _books;

        /// <summary>
        /// Get a list of books with a given author.
        /// </summary>
        /// <param name="author">Author of the book.</param>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of type <see cref="BookDto"/>,
        /// in which there are elements in which the author equal <paramref name="author"/>.</returns>
        [HttpGet("{author}")]
        public IEnumerable<BookDto> Get(string author) =>_books.Where(b => b.Author == author);

        /// <summary>
        /// Adds a new book.
        /// </summary>
        /// <param name="book">New book.</param>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of the type <see cref="BookDto"/>, 
        /// which contains all existing elements with a new <paramref name="book"/>.</returns>
        [HttpPost]
        public IEnumerable<BookDto> Post([FromBody]BookDto book)
        {
            _books.Add(book);

            return _books;
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
        [HttpDelete]
        public IActionResult Delete(string title, string author)
        {
            var book = _books.SingleOrDefault(b => b.Title == title && b.Author == author);
            if (book == null) return NotFound();

            _books.Remove(book);

            return Ok();
        }
    }
}
