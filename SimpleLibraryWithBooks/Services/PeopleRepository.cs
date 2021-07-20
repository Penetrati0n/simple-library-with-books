using System;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Models.Person;

namespace SimpleLibraryWithBooks.Services
{
    public static class PeopleRepository
    {
        public static readonly List<PersonModel> People = new List<PersonModel>()
        {
            new PersonModel() { LastName = "Кармазин", FirstName = "Лев", Patronymic = "Олегович", Birthday = DateTimeOffset.Parse("12/03/1983")},
            new PersonModel() { LastName = "Тихомиров", FirstName = "Филипп", Patronymic = "Михайлович", Birthday = DateTimeOffset.Parse("17/09/1980")},
            new PersonModel() { LastName = "Травникова", FirstName = "Мариетта", Patronymic = "Платоновна", Birthday = DateTimeOffset.Parse("03/07/2001")},
        };
    }
}
