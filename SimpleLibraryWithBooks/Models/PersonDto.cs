using System;

namespace SimpleLibraryWithBooks.Models
{
    public class PersonDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public DateTime Birthday { get; set; }
    }
}
