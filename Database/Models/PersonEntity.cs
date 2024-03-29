﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("person")]
    public class PersonEntity : Expansion
    {
        [Column("person_id")]
        public int Id { get; set; }

        [Required]
        [Column("first_name")]
        public string FirstName { get; set; }

        [Required]
        [Column("last_name")]
        public string LastName { get; set; }

        [Column("middle_name")]
        public string MiddleName { get; set; }

        [Column("birth_date")]
        public DateTimeOffset Birthday { get; set; }

        public ICollection<LibraryCardEntity> LibraryCards { get; set; }

        public PersonEntity()
        {
            LibraryCards = new List<LibraryCardEntity>();
        }
    }
}
