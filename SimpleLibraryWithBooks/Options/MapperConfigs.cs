using System;
using Mapster;
using SimpleLibraryWithBooks.Models.Book;
using SimpleLibraryWithBooks.Models.Person;
using SimpleLibraryWithBooks.Models.PersonBook;

namespace SimpleLibraryWithBooks.Options
{
    public static class MapperConfigs
    {
        public static TypeAdapterConfig ForBooks { get; private set; }
        public static TypeAdapterConfig ForPeople { get; private set; }
        public static TypeAdapterConfig ForPersonBooks { get; private set; }

        public static void Init()
        {
            ForBooks = new();
            ForBooks
                .NewConfig<BookEntity, BookResponseDto>()
                .Map(dest => dest.Genre, src => default(string));

            ForPeople = new();
            ForPeople
                .NewConfig<PersonEntity, PersonResponseDto>()
                .Map(dest => dest.Birthday, src => default(DateTimeOffset));

            ForPersonBooks = new();
            ForPersonBooks
                .NewConfig<PersonBookEntity, PersonBookResponseDto>()
                .Map(dest => dest.Person, src => src.Person.Adapt<PersonResponseDto>(ForPeople))
                .Map(dest => dest.Book, src => src.Book.Adapt<BookResponseDto>(ForBooks));
        }
    }
}
