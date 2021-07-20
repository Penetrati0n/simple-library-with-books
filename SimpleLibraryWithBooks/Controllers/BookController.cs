using System.Linq;
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
        /// <summary>
        /// Get full list of books.
        /// </summary>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of the type <see cref="BookDetailDto"/>, 
        /// which contains all existing elements.</returns>
        [HttpGet]
        public IEnumerable<BookDetailDto> Get() => BookRepository.Books;

        /// <summary>
        /// Get a list of books with a given author.
        /// </summary>
        /// <param name="author">Author of the book.</param>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of type <see cref="BookDetailDto"/>,
        /// in which there are elements in which the author equal <paramref name="author"/>.</returns>
        [HttpGet("{author}")]
        public IEnumerable<BookDetailDto> Get(string author) => BookRepository.Books.Where(b => b.Author == author);

        /// <summary>
        /// Adds a new book.
        /// </summary>
        /// <param name="book">New book.</param>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of the type <see cref="BookDetailDto"/>, 
        /// which contains all existing elements with a new <paramref name="book"/>.</returns>
        [HttpPost]
        public IEnumerable<BookDetailDto> Post([FromBody] BookDetailDto book)
        {
            BookRepository.Books.Add(book);

            return BookRepository.Books;
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
            var book = BookRepository.Books.SingleOrDefault(b => b.Title == title && b.Author == author);
            if (book == null) return NotFound();

            BookRepository.Books.Remove(book);

            return Ok();
        }
    }
}
