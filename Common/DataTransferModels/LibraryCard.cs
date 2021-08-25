using System;
using System.ComponentModel.DataAnnotations;

namespace Common.DataTransferModels
{
    public class LibraryCard
    {
        public class Request
        {
            public class Create
            {
                [Required]
                public int BookId { get; set; }
                
                [Required]
                public int PersonId { get; set; }
            }

            public class Update
            {
                [Required]
                public int BookId { get; set; }

                [Required]
                public int PersonId { get; set; }

                [Required]
                public double AddedDays { get; set; }
            }
        }

        public class Response
        {
            public class Debetor
            {
                public Person.Response Person { get; set; }
                public Book.Response.Without.All Book { get; set; }
                public double DaysDelay { get; set; }
            }

            public class WithOutPerson
            {
                public Book.Response.Without.All Book { get; set; }
                public DateTimeOffset TimeReturn { get; set; }
            }
        }
    }
}
