using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class Expansion
    {
        [Column("time_create")]
        public DateTimeOffset TimeCreate { get; set; }

        [Column("time_edit")]
        public DateTimeOffset TimeEdit { get; set; }

        [Column("version")]
        public int Version { get; set; }
    }
}
