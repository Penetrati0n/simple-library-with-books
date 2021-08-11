using System;

namespace SimpleLibraryWithBooks.Models.Person
{
    public class PersonResponseDto
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        public DateTimeOffset Birthday { get; set; }
    }
}
