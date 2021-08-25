using Mapster;
using System.Linq;
using Database.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Common.DataTransferModels;
using System.Collections.Generic;
using Infrastructure.Services.Interfaces;

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
        public async Task<IEnumerable<Book.Response.Without.People>> GetBooksByAuthor([FromQuery] Author.Request.FilterForBooks authorRequest)
        {
            var bookEntities = await _bookService.GetAllAsync();
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
                bookResponse.Author = (await _authorService.GetAsync(bookEntity.AuthorId)).Adapt<Author.Response>();
                bookResponse.Genres = bookEntity.Genres.Adapt<IEnumerable<Genre.Response.WithoutBooks>>();
                booksResponse.Add(bookResponse);
            }

            return booksResponse;
        }

        [HttpGet("{genreId}")]
        public async Task<ActionResult<IEnumerable<Book.Response.Without.People>>> GetBooksByGenre(int genreId)
        {
            if (!await _genreService.ContainsAsync(genreId))
                return BadRequest("The genre does not exist");

            var bookEntities = await _bookService.GetAllAsync(b => b.Genres.Any(g => g.Id == genreId));
            var booksResponse = new List<Book.Response.Without.People>();
            foreach (var bookEntity in bookEntities)
            {
                var bookResponse = bookEntity.Adapt<Book.Response.Without.People>();
                bookResponse.Author = (await _authorService.GetAsync(bookEntity.AuthorId)).Adapt<Author.Response>();
                bookResponse.Genres = bookEntity.Genres.Adapt<IEnumerable<Genre.Response.WithoutBooks>>();
                booksResponse.Add(bookResponse);
            }

            return booksResponse;
        }

        [HttpPost]
        public async Task<ActionResult<Book.Response.Without.People>> AddBook([FromBody] Book.Request.Create bookRequest)
        {
            if (!await _authorService.ContainsAsync(bookRequest.AuthorId))
                return BadRequest("The author does not exist.");
            else if (await _bookService.ContainsAsync(bookRequest.Name, bookRequest.AuthorId))
                return BadRequest("The book already exists.");
            else if (bookRequest.Genres.Any(g => !_genreService.Contains(g.Id)))
                return BadRequest("There are no such genres.");

            var bookEntity = bookRequest.Adapt<BookEntity>();
            bookEntity.Genres = bookRequest.Genres.Select(g => new GenreEntity() { Id = g.Id }).ToList();
            await _bookService.InsertAsync(bookEntity);
            await _bookService.SaveAsync();

            var bookResponse = bookEntity.Adapt<Book.Response.Without.People>();
            bookResponse.Author = (await _authorService.GetAsync(bookEntity.AuthorId)).Adapt<Author.Response>();
            bookResponse.Genres = bookEntity.Genres.Adapt<IEnumerable<Genre.Response.WithoutBooks>>();

            return Ok(bookResponse);
        }

        [HttpPut]
        public async Task<ActionResult<Book.Response.Without.People>> UpdateBook([FromBody] Book.Request.Update bookRequest)
        {
            if (bookRequest.AddGenres == null)
                bookRequest.AddGenres = new List<Genre.Request.ForBook>();
            if (bookRequest.DeleteGenres == null)
                bookRequest.DeleteGenres = new List<Genre.Request.ForBook>();

            if (!await _bookService.ContainsAsync(bookRequest.Id))
                return NotFound();
            else if (!await _authorService.ContainsAsync(bookRequest.AuthorId))
                return BadRequest("The author does not exist.");
            else if (bookRequest.AddGenres.Concat(bookRequest.DeleteGenres).Distinct().Any(g => !_genreService.Contains(g.Id)))
                return BadRequest("There is no genre from this list.");

            var bookEntity = bookRequest.Adapt<BookEntity>();
            bookEntity.Genres = bookRequest.AddGenres
                .Select(g => new GenreEntity() { Id = g.Id, Name = "A" })
                .Concat(bookRequest.DeleteGenres.Select(g => new GenreEntity() { Id = g.Id, Name = "D" }))
                .ToList();
            await _bookService.UpdateAsync(bookEntity);
            await _bookService.SaveAsync();

            bookEntity = await _bookService.GetAsync(bookRequest.Id);
            var bookResponse = bookEntity.Adapt<Book.Response.Without.People>();
            bookResponse.Author = (await _authorService.GetAsync(bookEntity.AuthorId)).Adapt<Author.Response>();
            bookResponse.Genres = bookEntity.Genres.Adapt<IEnumerable<Genre.Response.WithoutBooks>>();

            return Ok(bookResponse);
        }

        [HttpDelete("{bookId}")]
        public async Task<ActionResult> DeleteBook(int bookId)
        {
            if (!await _bookService.ContainsAsync(bookId))
                return NotFound("The book does not exist.");
            else if ((await _bookService.GetAsync(bookId)).LibraryCards.Any())
                return BadRequest("Deletion is not possible. This book is in the possession of a person.");

            await _bookService.DeleteAsync(bookId);
            await _bookService.SaveAsync();

            return Ok();
        }
    }
}
