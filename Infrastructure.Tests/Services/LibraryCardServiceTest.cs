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
    public class LibraryCardServiceTest : ServiceTest
    {
        public LibraryCardServiceTest()
            : base(new DbContextOptionsBuilder<DatabaseContext>()
                    .UseInMemoryDatabase("libraryCard.db")
                    .Options)
        { }

        [Fact]
        public async void GetAllAsync_Default_NotNull()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var libraryCardService = new LibraryCardService(context);

                var actionResult = await libraryCardService.GetAllAsync();

                Assert.NotNull(actionResult);
            }
        }

        [Fact]
        public async void GetAllAsync_Default_ExactNumberOfLibraryCards()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var libraryCardService = new LibraryCardService(context);
                var libraryCardsCount = await context.LibraryCards.CountAsync();

                var libraryCards = await libraryCardService.GetAllAsync();

                Assert.Equal(libraryCardsCount, libraryCards.Count());
            }
        }

        [Theory]
        [InlineData(3, 3)]
        public async void GetAllAsync_WithRule_NotNull(int minBookId, int minPersonId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                Expression<Func<LibraryCardEntity, bool>> rule = (libraryCard) => libraryCard.BookId >= minBookId && libraryCard.PersonId >= minPersonId;
                var libraryCardService = new LibraryCardService(context);

                var actionResult = await libraryCardService.GetAllAsync(rule);

                Assert.NotNull(actionResult);
            }
        }

        [Theory]
        [InlineData(3, 3)]
        public async void GetAllAsync_WithRule_ExactNumberOfLibraryCards(int minBookId, int minPersonId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                Expression<Func<LibraryCardEntity, bool>> rule = (libraryCard) => libraryCard.BookId >= minBookId && libraryCard.PersonId >= minPersonId;
                var libraryCardService = new LibraryCardService(context);
                var libraryCardsCount = await context.LibraryCards.Where(rule).CountAsync();

                var libraryCards = await libraryCardService.GetAllAsync(rule);

                Assert.Equal(libraryCardsCount, libraryCards.Count());
            }
        }

        [Theory]
        [InlineData(3, 3)]
        public async void GetAsync_ByIds_IsLibraryCardEntity(int bookId, int personId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var libraryCardService = new LibraryCardService(context);

                var libraryCard = await libraryCardService.GetAsync(bookId, personId);

                Assert.IsType<LibraryCardEntity>(libraryCard);
            }
        }

        [Theory]
        [InlineData(10, 10)]
        public async void GetAsync_ByIds_InvalidOperationException(int bookId, int personId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var libraryCardService = new LibraryCardService(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await libraryCardService.GetAsync(bookId, personId));
            }
        }

        [Fact]
        public async void InsertAsync_Default_IsIncreased()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var libraryCardService = new LibraryCardService(context);
                var startCount = await context.LibraryCards.CountAsync();
                var libraryCard = new LibraryCardEntity() { BookId = 10, PersonId = 10 };

                await libraryCardService.InsertAsync(libraryCard);
                await libraryCardService.SaveAsync();
                var endCount = await context.LibraryCards.CountAsync();

                Assert.True(startCount < endCount);
            }
        }

        [Fact]
        public async void InsertAsync_Default_ArgumentNullException()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var libraryCardService = new LibraryCardService(context);
                LibraryCardEntity libraryCard = null;

                await Assert.ThrowsAsync<ArgumentNullException>(async () => await libraryCardService.InsertAsync(libraryCard));
            }
        }

        [Theory]
        [InlineData(1, 5, 7)]
        public async void UpdateAsync_Default_IsUpdated(int bookId, int personId, double days)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var libraryCardService = new LibraryCardService(context);
                var libraryCard = new LibraryCardEntity() { BookId = bookId, PersonId = personId, TimeReturn = default(DateTimeOffset).AddDays(days) };

                await libraryCardService.UpdateAsync(libraryCard);
                await libraryCardService.SaveAsync();
                var libraryCardUpdated = await libraryCardService.GetAsync(bookId, personId);

                Assert.True(libraryCardUpdated.TimeReturn - DateTimeOffset.Now > TimeSpan.FromDays(13));
                Assert.True(libraryCardUpdated.Version > 1);
            }
        }

        [Theory]
        [InlineData(10, 10)]
        public async void UpdateAsync_Default_InvalidOperationException(int bookId, int personId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var libraryCardService = new LibraryCardService(context);
                var libraryCard = new LibraryCardEntity() { BookId = bookId, PersonId = personId };

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await libraryCardService.UpdateAsync(libraryCard));
            }
        }

        [Fact]
        public async void UpdateAsync_Default_NullReferenceException()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var libraryCardService = new LibraryCardService(context);
                LibraryCardEntity libraryCard = null;

                await Assert.ThrowsAsync<NullReferenceException>(async () => await libraryCardService.UpdateAsync(libraryCard));
            }
        }

        [Theory]
        [InlineData(1, 5)]
        public async void DeleteAsync_ByIds_IsDecreased(int bookId, int personId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var libraryCardService = new LibraryCardService(context);
                var startCount = await context.LibraryCards.CountAsync();

                await libraryCardService.DeleteAsync(bookId, personId);
                await libraryCardService.SaveAsync();
                var endCount = await context.LibraryCards.CountAsync();

                Assert.True(startCount > endCount);
            }
        }

        [Theory]
        [InlineData(10, 10)]
        public async void DeleteAsync_ByIds_InvalidOperationException(int bookId, int personId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var libraryCardService = new LibraryCardService(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await libraryCardService.DeleteAsync(bookId, personId));
            }
        }

        [Theory]
        [InlineData(1, 5, true)]
        [InlineData(2, 4, true)]
        [InlineData(3, 3, true)]
        [InlineData(4, 2, true)]
        [InlineData(5, 1, true)]
        [InlineData(6, 6, false)]
        public async void ContainsAsync_ByIds_IsContains(int bookId, int personId, bool expected)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var libraryCardService = new LibraryCardService(context);

                var actual = await libraryCardService.ContainsAsync(bookId, personId);

                Assert.Equal(actual, expected);
            }
        }
    }
}
