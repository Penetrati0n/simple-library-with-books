using Mapster;
using System.Linq;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Common.DataTransferModels;
using System.Collections.Generic;
using Infrastructure.Services.Interfaces;

namespace SimpleLibraryWithBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;
        private readonly IAuthorService _authorService;

        public AuthorController(IBookService bookService, IGenreService genreService, IAuthorService authorService)
        {
            _bookService = bookService;
            _genreService = genreService;
            _authorService = authorService;
        }

        [HttpGet]
        public IEnumerable<Author.Response> GetAuthors()
        {
            var authorEntities = _authorService.GetAll();
            var authorsResponse = authorEntities.Adapt<IEnumerable<Author.Response>>();

            return authorsResponse;
        }

        [HttpGet("{authorId}")]
        public ActionResult<Author.Response.WithBooks> GetAuthor(int authorId)
        {
            if (!_authorService.Contains(authorId))
                return NotFound();

            var authorEntity = _authorService.Get(authorId);
            var authorReponse = authorEntity.Adapt<Author.Response.WithBooks>();
            var bookEntities = _bookService.GetAll(b => b.AuthorId == authorId).ToList();
            if (bookEntities.Any())
            {
                authorReponse.Books = bookEntities.Adapt<IEnumerable<Book.Response.Without.AuthorAndPeople>>();
                foreach (var book in authorReponse.Books)
                    book.Genres = bookEntities.Single(b => b.Id == book.Id).Genres.Adapt<IEnumerable<Genre.Response.WithoutBooks>>();
            }

            return Ok(authorReponse);
        }

        [HttpPost]
        public ActionResult<Author.Response.WithBooks> AddAuthor([FromBody] Author.Request.Create.WithBooks authorRequest)
        {
            if (_authorService.Contains(authorRequest.FirstName, authorRequest.MiddleName, authorRequest.LastName))
                return BadRequest("The author already exists.");
            else if (authorRequest.Books != null && authorRequest.Books.Any())
                if (authorRequest.Books.SelectMany(b => b.Genres).Any(g => !_genreService.Contains(g.Id)))
                    return BadRequest("There is no genre of any kind");
                else if (authorRequest.Books.Select(b => b.Name).Distinct().Count() != authorRequest.Books.Count())
                    return BadRequest("You can't add identical books.");

            var authorEntity = authorRequest.Adapt<AuthorEntity>();
            _authorService.Insert(authorEntity);
            _authorService.Save();

            var authorResponse = authorEntity.Adapt<Author.Response.WithBooks.WithoutAll>();
            if (authorRequest.Books != null && authorRequest.Books.Any())
            {
                var bookEntities = authorRequest.Books.Adapt<IEnumerable<BookEntity>>();
                var bookEntitiesForResponse = new List<BookEntity>();
                foreach (var book in bookEntities)
                {
                    book.Genres.Clear();
                    _bookService.Insert(book);
                    foreach (var genre in authorRequest.Books.Single(b => b.Name == book.Name).Genres)
                        book.Genres.Add(_genreService.Get(genre.Id));
                    book.AuthorId = authorEntity.Id;
                    _bookService.Save();
                    bookEntitiesForResponse.Add(book);
                }
                authorResponse.Books = bookEntitiesForResponse.Adapt<IEnumerable<Book.Response.Without.All>>();
            }

            return Ok(authorResponse);
        }

        [HttpPut]
        public ActionResult<Author.Response> UpdateAuthor([FromBody] Author.Request.Update authorRequest)
        {
            if (!_authorService.Contains(authorRequest.Id))
                return NotFound();

            var authorEntity = authorRequest.Adapt<AuthorEntity>();
            _authorService.Update(authorEntity);
            _authorService.Save();

            var authorResponse = authorEntity.Adapt<Author.Response>();

            return Ok(authorResponse);
        }

        [HttpDelete("{authorId}")]
        public ActionResult DeleteAuthor(int authorId)
        {
            if (!_authorService.Contains(authorId))
                return NotFound();
            else if (_bookService.GetAll(b => b.AuthorId == authorId).Any(b => b.LibraryCards.Any()))
                return BadRequest("You can't delete the author and his books, because the user has 1 or more.");

            _authorService.Delete(authorId);
            _authorService.Save();
            var bookEntities = _bookService.GetAll(b => b.AuthorId == authorId);
            foreach (var book in bookEntities)
                _bookService.Delete(book.Id);
            _bookService.Save();

            return Ok();
        }
    }
}
