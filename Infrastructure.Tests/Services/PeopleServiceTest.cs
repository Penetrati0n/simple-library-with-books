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
    public class PeopleServiceTest : ServiceTest
    {
        public PeopleServiceTest()
            : base(new DbContextOptionsBuilder<DatabaseContext>()
                    .UseInMemoryDatabase("people.db")
                    .Options)
        { }

        [Fact]
        public async void GetAllAsync_Default_NotNull()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);

                var actionResult = await peopleService.GetAllAsync();

                Assert.NotNull(actionResult);
            }
        }

        [Fact]
        public async void GetAllAsync_Default_ExactNumberOfPersons()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);
                var peopleCount = await context.People.CountAsync();

                var people = await peopleService.GetAllAsync();

                Assert.Equal(peopleCount, people.Count());
            }
        }

        [Theory]
        [InlineData(3)]
        public async void GetAllAsync_WithRule_NotNull(int minId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                Expression<Func<PersonEntity, bool>> rule = (person) => person.Id >= minId;
                var peopleService = new PeopleService(context);

                var actionResult = await peopleService.GetAllAsync(rule);

                Assert.NotNull(actionResult);
            }
        }

        [Theory]
        [InlineData(3)]
        public async void GetAllAsync_WithRule_ExactNumberOfPersons(int minId)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                Expression<Func<PersonEntity, bool>> rule = (person) => person.Id >= minId;
                var peopleService = new PeopleService(context);
                var peopleCount = await context.People.Where(rule).CountAsync();

                var people = await peopleService.GetAllAsync(rule);

                Assert.Equal(peopleCount, people.Count());
            }
        }

        [Theory]
        [InlineData("Лютов", "Дорофей", "Валентинович")]
        public async void GetAllAsync_ByNames_NotNull(string firstName, string middleName, string lastName)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);

                var actionResult = await peopleService.GetAllAsync(firstName, middleName, lastName);

                Assert.NotNull(actionResult);
            }
        }

        [Theory]
        [InlineData("Лютов", "Дорофей", "Валентинович")]
        public async void GetAllAsync_ByNames_ExactNumberOfPersons(string firstName, string middleName, string lastName)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);
                var peopleCount = await context.People.Where(p => p.FirstName == firstName && p.MiddleName == middleName && p.LastName == lastName).CountAsync();

                var people = await peopleService.GetAllAsync(firstName, middleName, lastName);

                Assert.Equal(peopleCount, people.Count());
            }
        }

        [Theory]
        [InlineData(3)]
        public async void GetAsync_ById_IsPersonEntity(int id)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);

                var person = await peopleService.GetAsync(id);

                Assert.IsType<PersonEntity>(person);
            }
        }

        [Theory]
        [InlineData(10)]
        public async void GetAsync_ById_InvalidOperationException(int id)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await peopleService.GetAsync(id));
            }
        }

        [Theory]
        [InlineData("Лютов", "Дорофей", "Валентинович", 1973, 7, 17)]
        public async void GetAsync_ByNamesAndBirthday_IsPersonEntity(string firstName, string middleName, string lastName, int year, int mounth, int day)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);

                var person = await peopleService.GetAsync(firstName, middleName, lastName, new DateTimeOffset(year, mounth, day, 0, 0, 0, TimeSpan.Zero));

                Assert.IsType<PersonEntity>(person);
            }
        }

        [Theory]
        [InlineData("Лютов", "Дорофей", "Валентинович", 2000, 7, 17)]
        public async void GetAsync_ByNamesAndBirthday_InvalidOperationException(string firstName, string middleName, string lastName, int year, int mounth, int day)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                    await peopleService.GetAsync(firstName, middleName, lastName, new DateTimeOffset(year, mounth, day, 0, 0, 0, TimeSpan.Zero)));
            }
        }

        [Fact]
        public async void InsertAsync_Default_IsIncreased()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);
                var startCount = await context.People.CountAsync();
                var person = new PersonEntity();

                await peopleService.InsertAsync(person);
                await peopleService.SaveAsync();
                var endCount = await context.People.CountAsync();

                Assert.True(startCount < endCount);
            }
        }

        [Fact]
        public async void InsertAsync_Default_ArgumentNullException()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);
                PersonEntity person = null;

                await Assert.ThrowsAsync<ArgumentNullException>(async () => await peopleService.InsertAsync(person));
            }
        }

        [Theory]
        [InlineData(1, "qwerty", "qwerty", "qwerty", 2000, 7, 17)]
        public async void UpdateAsync_Default_IsUpdated(int id, string firstName, string middleName, string lastName, int year, int mounth, int day)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);
                var person = new PersonEntity()
                {
                    Id = id,
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    Birthday = new DateTimeOffset(year, mounth, day, 0, 0, 0, TimeSpan.Zero)
                };

                await peopleService.UpdateAsync(person);
                await peopleService.SaveAsync();
                var personUpdated = await peopleService.GetAsync(id);

                Assert.True(personUpdated.FirstName == person.FirstName &&
                            personUpdated.MiddleName == person.MiddleName &&
                            personUpdated.LastName == person.LastName &&
                            personUpdated.Birthday == person.Birthday);
                Assert.True(personUpdated.Version > 1);
            }
        }

        [Theory]
        [InlineData(10)]
        public async void UpdateAsync_Default_InvalidOperationException(int id)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);
                var person = new PersonEntity() { Id = id };

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await peopleService.UpdateAsync(person));
            }
        }

        [Fact]
        public async void UpdateAsync_Default_NullReferenceException()
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);
                PersonEntity person = null;

                await Assert.ThrowsAsync<NullReferenceException>(async () => await peopleService.UpdateAsync(person));
            }
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteAsync_ById_IsDecreased(int id)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);
                var startCount = await context.People.CountAsync();

                await peopleService.DeleteAsync(id);
                await peopleService.SaveAsync();
                var endCount = await context.People.CountAsync();

                Assert.True(startCount > endCount);
            }
        }

        [Theory]
        [InlineData(10)]
        public async void DeleteAsync_ById_InvalidOperationException(int id)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await peopleService.DeleteAsync(id));
            }
        }

        [Theory]
        [InlineData("Лютов", "Дорофей", "Валентинович")]
        public async void DeleteAsync_ByNames_IsDecreased(string firstName, string middleName, string lastName)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);
                var startCount = await context.People.CountAsync();

                await peopleService.DeleteAsync(firstName, middleName, lastName);
                await peopleService.SaveAsync();
                var endCount = await context.People.CountAsync();

                Assert.True(startCount > endCount);
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
                var peopleService = new PeopleService(context);

                var actual = await peopleService.ContainsAsync(id);

                Assert.Equal(actual, expected);
            }
        }

        [Theory]
        [InlineData("qwerty", "qwerty", "qwerty", false)]
        [InlineData("Лютов", "Дорофей", "Валентинович", true)]
        public async void ContainsAsync_ByNames_IsContains(string firstName, string middleName, string lastName, bool expected)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);

                var actual = await peopleService.ContainsAsync(firstName, middleName, lastName);

                Assert.Equal(actual, expected);
            }
        }

        [Theory]
        [InlineData("qwerty", "qwerty", "qwerty", 1973, 7, 17, false)]
        [InlineData("Лютов", "Дорофей", "Валентинович", 1973, 7, 17, true)]
        public async void ContainsAsync_ByNamesAndBirthday_IsContains(string firstName, string middleName, string lastName, int year, int mounth, int day, bool expected)
        {
            using (var context = new DatabaseContext(ContextOptions))
            {
                var peopleService = new PeopleService(context);

                var actual = await peopleService.ContainsAsync(firstName, middleName, lastName, new DateTimeOffset(year, mounth, day, 0, 0, 0, TimeSpan.Zero));

                Assert.Equal(actual, expected);
            }
        }
    }
}
