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
    public class BookServiceTest : ServiceTest
    {
        public BookServiceTest()
            : base(new DbContextOptionsBuilder<DatabaseContext>()
                    .UseInMemoryDatabase("book.db")
                    .Options)
        { }

        [Fact]
        public async void GetAllAsync_Default_NotNull()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var bookService = new BookService(context);

                var actionResult = await bookService.GetAllAsync();

                Assert.NotNull(actionResult);
            }
        }

        [Fact]
        public async void GetAllAsync_Default_ExactNumberOfBooks()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var bookService = new BookService(context);
                var booksCount = await context.Books.CountAsync();

                var books = await bookService.GetAllAsync();

                Assert.Equal(booksCount, books.Count());
            }
        }

        [Theory]
        [InlineData(3)]
        public async void GetAllAsync_WithRule_NotNull(int minId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                Expression<Func<BookEntity, bool>> rule = (book) => book.Id >= minId;
                var bookService = new BookService(context);

                var actionResult = await bookService.GetAllAsync(rule);

                Assert.NotNull(actionResult);
            }
        }

        [Theory]
        [InlineData(3)]
        public async void GetAllAsync_WithRule_ExactNumberOfBooks(int minId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                Expression<Func<BookEntity, bool>> rule = (book) => book.Id >= minId;
                var bookService = new BookService(context);
                var booksCount = await context.Books.Where(rule).CountAsync();

                var books = await bookService.GetAllAsync(rule);

                Assert.Equal(booksCount, books.Count());
            }
        }

        [Theory]
        [InlineData(3)]
        public async void GetAsync_ById_IsBookEntity(int id)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var bookService = new BookService(context);

                var book = await bookService.GetAsync(id);

                Assert.IsType<BookEntity>(book);
            }
        }

        [Theory]
        [InlineData(10)]
        public async void GetAsync_ById_InvalidOperationException(int id)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var bookService = new BookService(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await bookService.GetAsync(id));
            }
        }

        [Theory]
        [InlineData("Мастер и Маргарита", 1)]
        public async void GetAsync_ByNameAndAuthorId_IsBookEntity(string name, int authorId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var bookService = new BookService(context);

                var book = await bookService.GetAsync(name, authorId);

                Assert.IsType<BookEntity>(book);
            }
        }

        [Theory]
        [InlineData("qwerty", 0)]
        public async void GetAsync_ByNameAndAuthorId_InvalidOperationException(string name, int authorId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var bookService = new BookService(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await bookService.GetAsync(name, authorId));
            }
        }

        [Fact]
        public async void InsertAsync_Default_IsIncreased()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var bookService = new BookService(context);
                var startCount = await context.Books.CountAsync();
                var book = new BookEntity();

                await bookService.InsertAsync(book);
                await bookService.SaveAsync();
                var endCount = await context.Books.CountAsync();

                Assert.True(startCount < endCount);
            }
        }

        [Fact]
        public async void InsertAsync_Default_NullReferenceException()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var bookService = new BookService(context);
                BookEntity book = null;

                await Assert.ThrowsAsync<NullReferenceException>(async () => await bookService.InsertAsync(book));
            }
        }

        [Theory]
        [InlineData(1, 1, "qwerty")]
        public async void UpdateAsync_Default_IsUpdated(int id, int authorId, string name)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var bookService = new BookService(context);
                var book = new BookEntity() { Id = id, AuthorId = authorId, Name = name };

                await bookService.UpdateAsync(book);
                await bookService.SaveAsync();
                var bookUpdated = await bookService.GetAsync(id);

                Assert.True((bookUpdated.AuthorId, bookUpdated.Name) == (authorId, name));
                Assert.True(bookUpdated.Version > 1);
            }
        }

        [Theory]
        [InlineData(10, 1, "qwerty")]
        public async void UpdateAsync_Default_InvalidOperationException(int id, int authorId, string name)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var bookService = new BookService(context);
                var book = new BookEntity() { Id = id, AuthorId = authorId, Name = name };

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await bookService.UpdateAsync(book));
            }
        }

        [Fact]
        public async void UpdateAsync_Default_NullReferenceException()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var bookService = new BookService(context);
                BookEntity book = null;

                await Assert.ThrowsAsync<NullReferenceException>(async () => await bookService.UpdateAsync(book));
            }
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteAsync_ById_IsDecreased(int id)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var bookService = new BookService(context);
                var startCount = await context.Books.CountAsync();

                await bookService.DeleteAsync(id);
                await bookService.SaveAsync();
                var endCount = await context.Books.CountAsync();

                Assert.True(startCount > endCount);
            }
        }

        [Theory]
        [InlineData(10)]
        public async void DeleteAsync_ById_InvalidOperationException(int id)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var bookService = new BookService(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await bookService.DeleteAsync(id));
            }
        }

        [Theory]
        [InlineData("Мастер и Маргарита", 1)]
        public async void DeleteAsync_ByNameAndAuthorId_IsDecreased(string name, int authorId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var bookService = new BookService(context);
                var startCount = await context.Books.CountAsync();

                await bookService.DeleteAsync(name, authorId);
                await bookService.SaveAsync();
                var endCount = await context.Books.CountAsync();

                Assert.True(startCount > endCount);
            }
        }

        [Theory]
        [InlineData("qwerty", 10)]
        public async void DeleteAsync_ByNameAndAuthorId_InvalidOperationException(string name, int authorId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var bookService = new BookService(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await bookService.DeleteAsync(name, authorId));
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
                var bookService = new BookService(context);

                var actual = await bookService.ContainsAsync(id);

                Assert.Equal(actual, expected);
            }
        }

        [Theory]
        [InlineData("qwerty", 10, false)]
        [InlineData("Мастер и Маргарита", 1, true)]
        public async void ContainsAsync_ByNameAndAuthorId_IsContains(string name, int authorId, bool expected)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var bookService = new BookService(context);

                var actual = await bookService.ContainsAsync(name, authorId);

                Assert.Equal(actual, expected);
            }
        }
    }
}
