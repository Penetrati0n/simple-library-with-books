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
    public class AuthorServiceTest : ServiceTest
    {
        public AuthorServiceTest()
            : base(new DbContextOptionsBuilder<DatabaseContext>()
                    .UseInMemoryDatabase("author.db")
                    .Options)
        { }

        [Fact]
        public async void GetAllAsync_Default_NotNull()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var authorService = new AuthorService(context);

                var actionResult = await authorService.GetAllAsync();

                Assert.NotNull(actionResult);
            }
        }

        [Fact]
        public async void GetAllAsync_Default_ExactNumberOfAuthors()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var authorService = new AuthorService(context);
                var authorsCount = await context.Authors.CountAsync();

                var authors = await authorService.GetAllAsync();

                Assert.Equal(authorsCount, authors.Count());
            }
        }

        [Theory]
        [InlineData(3)]
        public async void GetAllAsync_WithRule_NotNull(int minId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                Expression<Func<AuthorEntity, bool>> rule = (author) => author.Id >= minId;
                var authorService = new AuthorService(context);

                var actionResult = await authorService.GetAllAsync(rule);

                Assert.NotNull(actionResult);
            }
        }

        [Theory]
        [InlineData(3)]
        public async void GetAllAsync_WithRule_ExactNumberOfAuthors(int minId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                Expression<Func<AuthorEntity, bool>> rule = (author) => author.Id >= minId;
                var authorService = new AuthorService(context);
                var authorsCount = await context.Authors.Where(rule).CountAsync();

                var authors = await authorService.GetAllAsync(rule);

                Assert.Equal(authorsCount, authors.Count());
            }
        }

        [Theory]
        [InlineData(3)]
        public async void GetAsync_ById_IsAuthorEntity(int id)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var authorService = new AuthorService(context);

                var author = await authorService.GetAsync(id);

                Assert.IsType<AuthorEntity>(author);
            }
        }

        [Theory]
        [InlineData(10)]
        public async void GetAsync_ById_InvalidOperationException(int id)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var authorService = new AuthorService(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await authorService.GetAsync(id));
            }
        }

        [Theory]
        [InlineData("Эрих", "Ремарк", "Мария")]
        public async void GetAsync_ByNames_IsAuthorEntity(string firstName, string middleName, string lastName)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var authorService = new AuthorService(context);

                var author = await authorService.GetAsync(firstName, middleName, lastName);

                Assert.IsType<AuthorEntity>(author);
            }
        }

        [Theory]
        [InlineData("qwerty", "qwerty", "qwerty")]
        public async void GetAsync_ByNames_InvalidOperationException(string firstName, string middleName, string lastName)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var authorService = new AuthorService(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await authorService.GetAsync(firstName, middleName, lastName));
            }
        }

        [Fact]
        public async void InsertAsync_Default_IsIncreased()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var authorService = new AuthorService(context);
                var startCount = await context.Authors.CountAsync();
                var author = new AuthorEntity();

                await authorService.InsertAsync(author);
                await authorService.SaveAsync();
                var endCount = await context.Authors.CountAsync();

                Assert.True(startCount < endCount);
            }
        }

        [Fact]
        public async void InsertAsync_Default_ArgumentNullException()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var authorService = new AuthorService(context);
                AuthorEntity author = null;

                await Assert.ThrowsAsync<ArgumentNullException>(async () => await authorService.InsertAsync(author));
            }
        }

        [Theory]
        [InlineData(1, "qwert", "qwerty", "qwerty")]
        public async void UpdateAsync_Default_IsUpdated(int id, string firstName, string middleName, string lastName)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var authorService = new AuthorService(context);
                var author = new AuthorEntity() { Id = id, FirstName = firstName, MiddleName = middleName, LastName = lastName };

                await authorService.UpdateAsync(author);
                await authorService.SaveAsync();
                var authorUpdated = await authorService.GetAsync(id);

                Assert.True((authorUpdated.FirstName, authorUpdated.MiddleName, authorUpdated.LastName) == (firstName, middleName, lastName));
                Assert.True(authorUpdated.Version > 1);
            }
        }

        [Theory]
        [InlineData(10, "qwert", "qwerty", "qwerty")]
        public async void UpdateAsync_Default_InvalidOperationException(int id, string firstName, string middleName, string lastName)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var authorService = new AuthorService(context);
                var author = new AuthorEntity() { Id = id, FirstName = firstName, MiddleName = middleName, LastName = lastName };

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await authorService.UpdateAsync(author));
            }
        }

        [Fact]
        public async void UpdateAsync_Default_NullReferenceException()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var authorService = new AuthorService(context);
                AuthorEntity author = null;

                await Assert.ThrowsAsync<NullReferenceException>(async () => await authorService.UpdateAsync(author));
            }
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteAsync_ById_IsDecreased(int id)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var authorService = new AuthorService(context);
                var startCount = await context.Authors.CountAsync();

                await authorService.DeleteAsync(id);
                await authorService.SaveAsync();
                var endCount = await context.Authors.CountAsync();

                Assert.True(startCount > endCount);
            }
        }

        [Theory]
        [InlineData(10)]
        public async void DeleteAsync_ById_InvalidOperationException(int id)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var authorService = new AuthorService(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await authorService.DeleteAsync(id));
            }
        }

        [Theory]
        [InlineData("Эрих", "Ремарк", "Мария")]
        public async void DeleteAsync_ByNames_IsDecreased(string firstName, string middleName, string lastName)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var authorService = new AuthorService(context);
                var startCount = await context.Authors.CountAsync();

                await authorService.DeleteAsync(firstName, middleName, lastName);
                await authorService.SaveAsync();
                var endCount = await context.Authors.CountAsync();

                Assert.True(startCount > endCount);
            }
        }

        [Theory]
        [InlineData("qwerty", "qwerty", "qwerty")]
        public async void DeleteAsync_ByNames_InvalidOperationException(string firstName, string middleName, string lastName)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var authorService = new AuthorService(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await authorService.DeleteAsync(firstName, middleName, lastName));
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
                var authorService = new AuthorService(context);

                var actual = await authorService.ContainsAsync(id);

                Assert.Equal(actual, expected);
            }
        }

        [Theory]
        [InlineData("Эрих", "Ремарк", "Мария", true)]
        [InlineData("qwerty", "qwerty", "qwerty", false)]
        public async void ContainsAsync_ByNames_IsContains(string firstName, string middleName, string lastName, bool expected)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var authorService = new AuthorService(context);

                var actual = await authorService.ContainsAsync(firstName, middleName, lastName);

                Assert.Equal(actual, expected);
            }
        }
    }
}
