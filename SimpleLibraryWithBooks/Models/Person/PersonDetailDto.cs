using System;

namespace SimpleLibraryWithBooks.Models.Person
{
    public class PersonDetailDto
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public DateTime Birthday { get; set; }
    }
}
