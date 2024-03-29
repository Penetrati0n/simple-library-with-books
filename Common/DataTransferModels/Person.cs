﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.DataTransferModels
{
    public class Person
    {
        public class Request
        {
            public class Create
            {
                [Required]
                [MaxLength(100)]
                [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                    ErrorMessage = "The First Name field contains invalid characters.")]
                public string FirstName { get; set; }

                [Required]
                [MaxLength(100)]
                [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                    ErrorMessage = "The Patronymic field contains invalid characters.")]
                public string MiddleName { get; set; }

                [Required]
                [MaxLength(100)]
                [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                    ErrorMessage = "The Last Name field contains invalid characters.")]
                public string LastName { get; set; }

                [Required]
                [DataType(DataType.Date)]
                public DateTimeOffset Birthday { get; set; }
            }

            public class Update
            {
                [Required]
                public int Id { get; set; }

                [Required]
                [MaxLength(100)]
                [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                    ErrorMessage = "The First Name field contains invalid characters.")]
                public string FirstName { get; set; }

                [Required]
                [MaxLength(100)]
                [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                    ErrorMessage = "The Patronymic field contains invalid characters.")]
                public string MiddleName { get; set; }

                [Required]
                [MaxLength(100)]
                [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿\\/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",
                    ErrorMessage = "The Last Name field contains invalid characters.")]
                public string LastName { get; set; }

                [Required]
                [DataType(DataType.Date)]
                public DateTimeOffset Birthday { get; set; }
            }
        }

        public class Response
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public DateTimeOffset Birthday { get; set; }

            public class WithBooks
            {
                public int Id { get; set; }
                public string FirstName { get; set; }
                public string MiddleName { get; set; }
                public string LastName { get; set; }
                public DateTimeOffset Birthday { get; set; }
                public IEnumerable<LibraryCard.Response.WithOutPerson> Books { get; set; }
                public WithBooks()
                {
                    Books = new List<LibraryCard.Response.WithOutPerson>();
                }
            }

            public class WithBook
            {
                public int Id { get; set; }
                public string FirstName { get; set; }
                public string MiddleName { get; set; }
                public string LastName { get; set; }
                public DateTimeOffset Birthday { get; set; }
                public LibraryCard.Response.WithOutPerson Book { get; set; }
            }
        }
    }
}
