using Mapster;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Models;
using SimpleLibraryWithBooks.Options;
using SimpleLibraryWithBooks.Services;

namespace SimpleLibraryWithBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public IEnumerable<Genre.Response.WithoutBooks> GetGenres()
        {
            var genreEntities = _genreService.GetAll();
            var genresResponse = genreEntities.Adapt<IEnumerable<Genre.Response.WithoutBooks>>();

            return genresResponse;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<Genre.Response.Statistic> GetStatistics()
        {
            var genresResponse = _genreService.GetAll().Adapt<IEnumerable<Genre.Response.Statistic>>(MapperConfigs.ForGenreStatistic);

            return genresResponse;
        }

        [HttpPost]
        public ActionResult<Genre.Response.WithoutBooks> AddGenre([FromBody]Genre.Request.Create genreRequest)
        {
            if (_genreService.Contains(genreRequest.Name))
                return BadRequest("The genre already exists.");

            var genreEntity = genreRequest.Adapt<GenreEntity>();
            _genreService.Insert(genreEntity);
            _genreService.Save();

            var genreResponse = genreEntity.Adapt<Genre.Response.WithoutBooks>();

            return genreResponse;
        }

        [HttpPut]
        public ActionResult<Genre.Response.WithoutBooks> UpdateGenre([FromBody]Genre.Request.Update genreRequest)
        {
            if (!_genreService.Contains(genreRequest.Id))
                return NotFound();
            else if (_genreService.Contains(genreRequest.Name))
                return BadRequest("The genre with this name already exists.");

            var genreEntity = genreRequest.Adapt<GenreEntity>();
            _genreService.Update(genreEntity);
            _genreService.Save();

            var genreResponse = genreEntity.Adapt<Genre.Response.WithoutBooks>();

            return genreResponse;
        }

        [HttpDelete("{genreId}")]
        public ActionResult DeleteGenre(int genreId)
        {
            if (!_genreService.Contains(genreId))
                return NotFound();

            _genreService.Delete(genreId);
            _genreService.Save();

            return Ok();
        }
    }
}
