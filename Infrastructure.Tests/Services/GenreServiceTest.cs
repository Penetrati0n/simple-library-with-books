using Xunit;
using System;
using Database;
using System.Linq;
using Database.Models;
using Infrastructure.Services;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Services
{
    public class GenreServiceTest : ServiceTest
    {
        public GenreServiceTest()
            : base(new DbContextOptionsBuilder<DatabaseContext>()
                    .UseInMemoryDatabase("genre.db")
                    .Options)
        { }

        [Fact]
        public async void GetAllAsync_Default_NotNull()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);

                var actionResult = await genreService.GetAllAsync();

                Assert.NotNull(actionResult);
            }
        }

        [Fact]
        public async void GetAllAsync_Default_ExactNumberOfGenres()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);
                var genresCount = await context.Genres.CountAsync();

                var genres = await genreService.GetAllAsync();

                Assert.Equal(genresCount, genres.Count());
            }
        }

        [Theory]
        [InlineData(3)]
        public async void GetAllAsync_WithRule_NotNull(int minId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                Expression<Func<GenreEntity, bool>> rule = (genre) => genre.Id >= minId;
                var genreService = new GenreService(context);

                var actionResult = await genreService.GetAllAsync(rule);

                Assert.NotNull(actionResult);
            }
        }

        [Theory]
        [InlineData(3)]
        public async void GetAllAsync_WithRule_ExactNumberOfGenres(int minId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                Expression<Func<GenreEntity, bool>> rule = (genre) => genre.Id >= minId;
                var genreService = new GenreService(context);
                var genresCount = await context.Genres.Where(rule).CountAsync();

                var genres = await genreService.GetAllAsync(rule);

                Assert.Equal(genresCount, genres.Count());
            }
        }

        [Theory]
        [InlineData(3)]
        public async void GetAsync_ById_IsGenreEntity(int id)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);

                var genre = await genreService.GetAsync(id);

                Assert.IsType<GenreEntity>(genre);
            }
        }

        [Theory]
        [InlineData(10)]
        public async void GetAsync_ById_InvalidOperationException(int id)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await genreService.GetAsync(id));
            }
        }

        [Theory]
        [InlineData("Роман")]
        public async void GetAsync_ByName_IsGenreEntity(string name)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);

                var genre = await genreService.GetAsync(name);

                Assert.IsType<GenreEntity>(genre);
            }
        }

        [Theory]
        [InlineData("qwerty")]
        public async void GetAsync_ByName_InvalidOperationException(string name)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await genreService.GetAsync(name));
            }
        }

        [Fact]
        public async void InsertAsync_Default_IsIncreased()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);
                var startCount = await context.Genres.CountAsync();
                var genre = new GenreEntity();

                await genreService.InsertAsync(genre);
                await genreService.SaveAsync();
                var endCount = await context.Genres.CountAsync();

                Assert.True(startCount < endCount);
            }
        }

        [Fact]
        public async void InsertAsync_Default_ArgumentNullException()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);
                GenreEntity genre = null;

                await Assert.ThrowsAsync<ArgumentNullException>(async () => await genreService.InsertAsync(genre));
            }
        }

        [Theory]
        [InlineData(1, "qwerty")]
        public async void UpdateAsync_Default_IsUpdated(int id, string name)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);
                var genre = new GenreEntity() { Id = id, Name = name };

                await genreService.UpdateAsync(genre);
                await genreService.SaveAsync();
                var genreUpdated = await genreService.GetAsync(id);

                Assert.Equal(genreUpdated.Name, name);
                Assert.True(genreUpdated.Version > 1);
            }
        }

        [Theory]
        [InlineData(10, "qwerty")]
        public async void UpdateAsync_Default_InvalidOperationException(int id, string name)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);
                var genre = new GenreEntity() { Id = id, Name = name };

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await genreService.UpdateAsync(genre));
            }
        }

        [Fact]
        public async void UpdateAsync_Default_NullReferenceException()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);
                GenreEntity genre = null;

                await Assert.ThrowsAsync<NullReferenceException>(async () => await genreService.UpdateAsync(genre));
            }
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteAsync_ById_IsDecreased(int id)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);
                var startCount = await context.Genres.CountAsync();

                await genreService.DeleteAsync(id);
                await genreService.SaveAsync();
                var endCount = await context.Genres.CountAsync();

                Assert.True(startCount > endCount);
            }
        }

        [Theory]
        [InlineData(10)]
        public async void DeleteAsync_ById_InvalidOperationException(int id)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await genreService.DeleteAsync(id));
            }
        }

        [Theory]
        [InlineData("Роман")]
        public async void DeleteAsync_ByName_IsDecreased(string name)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);
                var startCount = await context.Genres.CountAsync();

                await genreService.DeleteAsync(name);
                await genreService.SaveAsync();
                var endCount = await context.Genres.CountAsync();

                Assert.True(startCount > endCount);
            }
        }

        [Theory]
        [InlineData("qwerty")]
        public async void DeleteAsync_ByName_InvalidOperationException(string name)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await genreService.DeleteAsync(name));
            }
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, true)]
        [InlineData(5, true)]
        [InlineData(6, false)]
        public async void ContainsAsync_ById_IsContains(int id, bool expected)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);

                var actual = await genreService.ContainsAsync(id);

                Assert.Equal(actual, expected);
            }
        }

        [Theory]
        [InlineData("qwerty", false)]
        [InlineData("Роман", true)]
        public async void ContainsAsync_ByNames_IsContains(string name, bool expected)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);

                var actual = await genreService.ContainsAsync(name);

                Assert.Equal(actual, expected);
            }
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, true)]
        [InlineData(5, true)]
        [InlineData(6, false)]
        public void Contains_ById_IsContains(int id, bool expected)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);

                var actual = genreService.Contains(id);

                Assert.Equal(actual, expected);
            }
        }

        [Theory]
        [InlineData("qwerty", false)]
        [InlineData("Роман", true)]
        public void Contains_ByNames_IsContains(string name, bool expected)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var genreService = new GenreService(context);

                var actual = genreService.Contains(name);

                Assert.Equal(actual, expected);
            }
        }
    }
}
