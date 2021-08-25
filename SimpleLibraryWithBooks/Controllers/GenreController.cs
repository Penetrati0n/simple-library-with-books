using Mapster;
using Database.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Common.DataTransferModels;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Options;
using Infrastructure.Services.Interfaces;

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
        public async Task<IEnumerable<Genre.Response.WithoutBooks>> GetGenres()
        {
            var genreEntities = await _genreService.GetAllAsync();
            var genresResponse = genreEntities.Adapt<IEnumerable<Genre.Response.WithoutBooks>>();

            return genresResponse;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IEnumerable<Genre.Response.Statistic>> GetStatistics()
        {
            var genresEntity = await _genreService.GetAllAsync();
            var genresResponse = genresEntity.Adapt<IEnumerable<Genre.Response.Statistic>>(MapperConfigs.ForGenreStatistic);

            return genresResponse;
        }

        [HttpPost]
        public async Task<ActionResult<Genre.Response.WithoutBooks>> AddGenre([FromBody]Genre.Request.Create genreRequest)
        {
            if (await _genreService.ContainsAsync(genreRequest.Name))
                return BadRequest("The genre already exists.");

            var genreEntity = genreRequest.Adapt<GenreEntity>();
            await _genreService.InsertAsync(genreEntity);
            await _genreService.SaveAsync();

            var genreResponse = genreEntity.Adapt<Genre.Response.WithoutBooks>();

            return genreResponse;
        }

        [HttpPut]
        public async Task<ActionResult<Genre.Response.WithoutBooks>> UpdateGenre([FromBody]Genre.Request.Update genreRequest)
        {
            if (!await _genreService.ContainsAsync(genreRequest.Id))
                return NotFound();
            else if (await _genreService.ContainsAsync(genreRequest.Name))
                return BadRequest("The genre with this name already exists.");

            var genreEntity = genreRequest.Adapt<GenreEntity>();
            await _genreService.UpdateAsync(genreEntity);
            await _genreService.SaveAsync();

            var genreResponse = genreEntity.Adapt<Genre.Response.WithoutBooks>();

            return genreResponse;
        }

        [HttpDelete("{genreId}")]
        public async Task<ActionResult> DeleteGenre(int genreId)
        {
            if (!await _genreService.ContainsAsync(genreId))
                return NotFound();

            await _genreService.DeleteAsync(genreId);
            await _genreService.SaveAsync();

            return Ok();
        }
    }
}
