using System;
using System.Collections.Generic;
using SimpleLibraryWithBooks.Models.Person;

namespace SimpleLibraryWithBooks.Services
{
    public static class PeopleRepository
    {
        public static readonly List<PersonDetailDto> People = new List<PersonDetailDto>()
        {
            new PersonDetailDto() { LastName = "Кармазин", FirstName = "Лев", Patronymic = "Олегович", Birthday = DateTime.Parse("12/03/1983")},
            new PersonDetailDto() { LastName = "Тихомиров", FirstName = "Филипп", Patronymic = "Михайлович", Birthday = DateTime.Parse("17/09/1980")},
            new PersonDetailDto() { LastName = "Травникова", FirstName = "Мариетта", Patronymic = "Платоновна", Birthday = DateTime.Parse("03/07/2001")},
        };
    }
}
