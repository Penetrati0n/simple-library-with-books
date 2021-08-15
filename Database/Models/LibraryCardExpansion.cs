using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class LibraryCardExpansion : Expansion
    {
        [Column("time_return")]
        public DateTimeOffset TimeReturn { get; set; }
    }
}
