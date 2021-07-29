using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class PersonBookEntity
    {
        [Key]
        public int Id { get; set; }

        public PersonEntity Person { get; set; }
        
        public BookEntity Book { get; set; }
        
        public DateTimeOffset DateTimeReceipt { get; set; }
    }
}
