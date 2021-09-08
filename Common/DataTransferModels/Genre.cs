using System.ComponentModel.DataAnnotations;

namespace Common.DataTransferModels
{
    public class Genre
    {
        public class Request
        {
            public class Create
            {
                [Required]
                [MaxLength(100)]
                public string Name { get; set; }
            }

            public class Update
            {
                [Required]
                public int Id { get; set; }

                [Required]
                [MaxLength(100)]
                public string Name { get; set; }
            }

            public class ForBook
            {
                [Required]
                public int Id { get; set; }
            }
        }

        public class Response
        {
            public class WithoutBooks
            {
                public int Id { get; set; }
                public string Name { get; set; }
            }

            public class Statistic
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public int CountBooks { get; set; }
            }
        }
    }
}
