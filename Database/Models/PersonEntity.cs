﻿using System;

namespace Database.Models
{
    public class PersonEntity
    {
        public int Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        public DateTimeOffset Birthday { get; set; }
    }
}
