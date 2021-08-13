using Mapster;
using System.Linq;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Models;
using SimpleLibraryWithBooks.Services;

namespace SimpleLibraryWithBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;
        private readonly IAuthorService _authorService;

        public BookController(IBookService bookService, IGenreService genreService, IAuthorService authorService)
        {
            _bookService = bookService;
            _genreService = genreService;
            _authorService = authorService;
        }

        [HttpGet]
        public IEnumerable<Book.Response.Without.People> GetBooksByAuthor([FromQuery] Author.Request.FilterForBooks authorRequest)
        {
            var bookEntities = _bookService.GetAll();
            if (!string.IsNullOrEmpty(authorRequest.FirstName))
                bookEntities = bookEntities.Where(b => b.Author.FirstName.ToLower().Contains(authorRequest.FirstName.ToLower()));
            if (!string.IsNullOrEmpty(authorRequest.MiddleName))
                bookEntities = bookEntities.Where(b => b.Author.MiddleName.ToLower().Contains(authorRequest.MiddleName.ToLower()));
            if (!string.IsNullOrEmpty(authorRequest.LastName))
                bookEntities = bookEntities.Where(b => b.Author.LastName.ToLower().Contains(authorRequest.LastName.ToLower()));
            var booksResponse = new List<Book.Response.Without.People>();
            foreach (var bookEntity in bookEntities)
            {
                var bookResponse = bookEntity.Adapt<Book.Response.Without.People>();
                bookResponse.Author = _authorService.Get(bookEntity.AuthorId).Adapt<Author.Response>();
                bookResponse.Genres = bookEntity.Genres.Adapt<IEnumerable<Genre.Response.WithoutBooks>>();
                booksResponse.Add(bookResponse);
            }

            return booksResponse;
        }

        [HttpGet("{genreId}")]
        public ActionResult<IEnumerable<Book.Response.Without.People>> GetBooksByGenre(int genreId)
        {
            if (!_genreService.Contains(genreId))
                return BadRequest("The genre does not exist");

            var bookEntities = _bookService.GetAll(b => b.Genres.Any(g => g.Id == genreId));
            var booksResponse = new List<Book.Response.Without.People>();
            foreach (var bookEntity in bookEntities)
            {
                var bookResponse = bookEntity.Adapt<Book.Response.Without.People>();
                bookResponse.Author = _authorService.Get(bookEntity.AuthorId).Adapt<Author.Response>();
                bookResponse.Genres = bookEntity.Genres.Adapt<IEnumerable<Genre.Response.WithoutBooks>>();
                booksResponse.Add(bookResponse);
            }

            return booksResponse;
        }

        [HttpPost]
        public ActionResult<Book.Response.Without.People> AddBook([FromBody] Book.Request.Create bookRequest)
        {
            if (!_authorService.Contains(bookRequest.AuthorId))
                return BadRequest("The author does not exist.");
            else if (_bookService.Contains(bookRequest.Name, bookRequest.AuthorId))
                return BadRequest("The book already exists.");
            else if (bookRequest.Genres.Any(g => !_genreService.Contains(g.Id)))
                return BadRequest("There are no such genres.");

            var bookEntity = bookRequest.Adapt<BookEntity>();
            bookEntity.Genres.Clear(); ;
            _bookService.Insert(bookEntity);
            _bookService.Save();
            foreach (var genre in bookRequest.Genres.ToList())
                _genreService.Get(genre.Id).Books.Add(bookEntity);
            _genreService.Save();

            var bookResponse = bookEntity.Adapt<Book.Response.Without.People>();
            bookResponse.Author = _authorService.Get(bookEntity.AuthorId).Adapt<Author.Response>();
            bookResponse.Genres = bookEntity.Genres.Adapt<IEnumerable<Genre.Response.WithoutBooks>>();

            return Ok(bookResponse);
        }

        [HttpPut]
        public ActionResult<Book.Response.Without.People> UpdateBook([FromBody] Book.Request.Update bookRequest)
        {
            if (bookRequest.AddGenres == null)
                bookRequest.AddGenres = new List<Genre.Request.ForBook>();
            if (bookRequest.DeleteGenres == null)
                bookRequest.DeleteGenres = new List<Genre.Request.ForBook>();

            if (!_bookService.Contains(bookRequest.Id))
                return NotFound();
            else if (!_authorService.Contains(bookRequest.AuthorId))
                return BadRequest("The author does not exist.");
            else if (bookRequest.AddGenres.Concat(bookRequest.DeleteGenres).Distinct().Any(g => !_genreService.Contains(g.Id)))
                return BadRequest("There is no genre from this list.");

            var bookEntity = bookRequest.Adapt<BookEntity>();
            bookEntity.Genres = bookRequest.AddGenres
                .Select(g => new GenreEntity() { Id = g.Id, Name = "A" })
                .Concat(bookRequest.DeleteGenres.Select(g => new GenreEntity() { Id = g.Id, Name = "D" }))
                .ToList();
            _bookService.Update(bookEntity);
            _bookService.Save();

            bookEntity = _bookService.Get(bookRequest.Id);
            var bookResponse = bookEntity.Adapt<Book.Response.Without.People>();
            bookResponse.Author = _authorService.Get(bookEntity.AuthorId).Adapt<Author.Response>();
            bookResponse.Genres = bookEntity.Genres.Adapt<IEnumerable<Genre.Response.WithoutBooks>>();

            return Ok(bookResponse);
        }

        [HttpDelete("{bookId}")]
        public ActionResult DeleteBook(int bookId)
        {
            if (!_bookService.Contains(bookId))
                return NotFound("The book does not exist.");
            else if (_bookService.Get(bookId).People.Any())
                return BadRequest("Deletion is not possible. This book is in the possession of a person.");

            _bookService.Delete(bookId);
            _bookService.Save();

            return Ok();
        }
    }
}
