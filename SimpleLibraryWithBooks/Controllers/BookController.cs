using Mapster;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Options;
using SimpleLibraryWithBooks.Services;
using SimpleLibraryWithBooks.Models.Book;

namespace SimpleLibraryWithBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        /// <summary>
        /// Get full list of books.
        /// </summary>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of the type <see cref="BookResponseDto"/>, 
        /// which contains all existing elements.</returns>
        [HttpGet]
        public IEnumerable<BookResponseDto> Get()
        {
            var books = _bookRepository.GetAllBooks().Adapt<IEnumerable<BookResponseDto>>();

            return books;
        }

        /// <summary>
        /// Get a list of books with a given author.
        /// </summary>
        /// <param name="author">Author of the book.</param>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of type <see cref="BookResponseDto"/>,
        /// in which there are elements in which the author equal <paramref name="author"/>.</returns>
        [HttpGet("{author}")]
        public IEnumerable<BookResponseDto> Get(string author)
        {
            var books = _bookRepository.GetAllBooks(b => b.Author == author).Adapt<IEnumerable<BookResponseDto>>();

            return books;
        }

        /// <summary>
        /// Adds a new book.
        /// </summary>
        /// <param name="book">New book.</param>
        /// <returns>Returns <see cref="IEnumerable{T}"/> of type <see cref="BookResponseDto"/>,
        /// which contains all existing elements with a new <paramref name="book"/> without <c>Genre</c> property.</returns>
        [HttpPost]
        public ActionResult<IEnumerable<BookResponseDto>> Post([FromBody] BookRequestDto book)
        {
            if (_bookRepository.Contains(book.Title, book.Author))
                return BadRequest("The book already exists.");

            _bookRepository.InsertBook(book.Adapt<BookEntity>());
            _bookRepository.Save();

            var responseBooks = _bookRepository.GetAllBooks().Adapt<IEnumerable<BookResponseDto>>(MapperConfigs.ForBooks);

            return Json(responseBooks, SerializerOptions.WhenWritingDefault);
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
    }
}
