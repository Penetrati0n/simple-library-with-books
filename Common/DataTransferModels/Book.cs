using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.DataTransferModels
{
    public class Book
    {
        public class Request
        {
            public class Create
            {
                [Required]
                [MaxLength(100)]
                public string Name { get; set; }

                [Required]
                public int AuthorId { get; set; }

                [Required]
                [MinLength(1)]
                public IEnumerable<Genre.Request.ForBook> Genres { get; set; }

                public Create()
                {
                    Genres = new List<Genre.Request.ForBook>();
                }

                public class WithoutAutor
                {
                    [Required]
                    [MaxLength(100)]
                    public string Name { get; set; }

                    [Required]
                    [MinLength(1)]
                    public IEnumerable<Genre.Request.ForBook> Genres { get; set; }

                    public WithoutAutor()
                    {
                        Genres = new List<Genre.Request.ForBook>();
                    }
                }
            }

            public class Update
            {
                [Required]
                public int Id { get; set; }

                [Required]
                [MaxLength(100)]
                public string Name { get; set; }

                [Required]
                public int AuthorId { get; set; }

                public IEnumerable<Genre.Request.ForBook> AddGenres { get; set; }

                public IEnumerable<Genre.Request.ForBook> DeleteGenres { get; set; }

                public Update()
                {
                    AddGenres = new List<Genre.Request.ForBook>();
                    DeleteGenres = new List<Genre.Request.ForBook>();
                }
            }
        }

        public class Response
        {
            public class Without
            {
                public class People
                {
                    public int Id { get; set; }
                    public string Name { get; set; }
                    public Author.Response Author { get; set; }
                    public IEnumerable<Genre.Response.WithoutBooks> Genres { get; set; }
                    public People()
                    {
                        Genres = new List<Genre.Response.WithoutBooks>();
                    }
                }

                public class AuthorAndPeople
                {
                    public int Id { get; set; }
                    public string Name { get; set; }
                    public IEnumerable<Genre.Response.WithoutBooks> Genres { get; set; }
                    public AuthorAndPeople()
                    {
                        Genres = new List<Genre.Response.WithoutBooks>();
                    }
                }

                public class All
                {
                    public int Id { get; set; }
                    public string Name { get; set; }
                }
            }
        }
    }
}
