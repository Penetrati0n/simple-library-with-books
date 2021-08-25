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
        public async Task<IEnumerable<Author.Response>> GetAuthors()
        {
            var authorEntities = await _authorService.GetAllAsync();
            var authorsResponse = authorEntities.Adapt<IEnumerable<Author.Response>>();

            return authorsResponse;
        }

        [HttpGet("{authorId}")]
        public async Task<ActionResult<Author.Response.WithBooks>> GetAuthor(int authorId)
        {
            if (!await _authorService.ContainsAsync(authorId))
                return NotFound();

            var authorEntity = await _authorService.GetAsync(authorId);
            var authorReponse = authorEntity.Adapt<Author.Response.WithBooks>();
            var bookEntities = await _bookService.GetAllAsync(b => b.AuthorId == authorId);
            if (bookEntities.Any())
            {
                authorReponse.Books = bookEntities.Adapt<IEnumerable<Book.Response.Without.AuthorAndPeople>>();
                foreach (var book in authorReponse.Books)
                    book.Genres = bookEntities.Single(b => b.Id == book.Id).Genres.Adapt<IEnumerable<Genre.Response.WithoutBooks>>();
            }

            return Ok(authorReponse);
        }

        [HttpPost]
        public async Task<ActionResult<Author.Response.WithBooks>> AddAuthor([FromBody] Author.Request.Create.WithBooks authorRequest)
        {
            if (await _authorService.ContainsAsync(authorRequest.FirstName, authorRequest.MiddleName, authorRequest.LastName))
                return BadRequest("The author already exists.");
            else if (authorRequest.Books != null && authorRequest.Books.Any())
                if (authorRequest.Books.SelectMany(b => b.Genres).Any(g => !_genreService.Contains(g.Id)))
                    return BadRequest("There is no genre of any kind");
                else if (authorRequest.Books.Select(b => b.Name).Distinct().Count() != authorRequest.Books.Count())
                    return BadRequest("You can't add identical books.");

            var authorEntity = authorRequest.Adapt<AuthorEntity>();
            await _authorService.InsertAsync(authorEntity);
            await _authorService.SaveAsync();

            var authorResponse = authorEntity.Adapt<Author.Response.WithBooks.WithoutAll>();
            if (authorRequest.Books != null && authorRequest.Books.Any())
            {
                var bookEntities = authorRequest.Books.Adapt<IEnumerable<BookEntity>>();
                var bookEntitiesForResponse = new List<BookEntity>();
                foreach (var book in bookEntities)
                {
                    book.Genres.Clear();
                    await _bookService.InsertAsync(book);
                    foreach (var genre in authorRequest.Books.Single(b => b.Name == book.Name).Genres)
                        book.Genres.Add(await _genreService.GetAsync(genre.Id));
                    book.AuthorId = authorEntity.Id;
                    await _bookService.SaveAsync();
                    bookEntitiesForResponse.Add(book);
                }
                authorResponse.Books = bookEntitiesForResponse.Adapt<IEnumerable<Book.Response.Without.All>>();
            }

            return Ok(authorResponse);
        }

        [HttpPut]
        public async Task<ActionResult<Author.Response>> UpdateAuthor([FromBody] Author.Request.Update authorRequest)
        {
            if (!await _authorService.ContainsAsync(authorRequest.Id))
                return NotFound();

            var authorEntity = authorRequest.Adapt<AuthorEntity>();
            await _authorService.UpdateAsync(authorEntity);
            await _authorService.SaveAsync();

            var authorResponse = authorEntity.Adapt<Author.Response>();

            return Ok(authorResponse);
        }

        [HttpDelete("{authorId}")]
        public async Task<ActionResult> DeleteAuthor(int authorId)
        {
            if (!await _authorService.ContainsAsync(authorId))
                return NotFound();
            else if ((await _bookService.GetAllAsync(b => b.AuthorId == authorId)).Any(b => b.LibraryCards.Any()))
                return BadRequest("You can't delete the author and his books, because the user has 1 or more.");

            await _authorService.DeleteAsync(authorId);
            await _authorService.SaveAsync();
            var bookEntities = await _bookService.GetAllAsync(b => b.AuthorId == authorId);
            foreach (var book in bookEntities)
                await _bookService.DeleteAsync(book.Id);
            await _bookService.SaveAsync();

            return Ok();
        }
    }
}
