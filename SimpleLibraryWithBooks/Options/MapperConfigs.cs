using System;
using Mapster;
using Database.Models;
using Common.DataTransferModels;

namespace SimpleLibraryWithBooks.Options
{
    public static class MapperConfigs
    {
        public static TypeAdapterConfig ForGenreStatistic { get; private set; }
        public static TypeAdapterConfig ForLibraryCard { get; private set; }
        public static TypeAdapterConfig ForDebetor { get; set; }

        public static void Init()
        {
            ForGenreStatistic = new();
            ForGenreStatistic
                .NewConfig<GenreEntity, Genre.Response.Statistic>()
                .Map(dest => dest.CountBooks, src => src.Books.Count);

            ForLibraryCard = new();
            ForLibraryCard
                .NewConfig<LibraryCardEntity, LibraryCard.Response.WithOutPerson>()
                .Map(dest => dest.Book, src => src.Book.Adapt<Book.Response.Without.All>());

            ForDebetor = new();
            ForDebetor
                .NewConfig<LibraryCardEntity, LibraryCard.Response.Debetor>()
                .Map(dest => dest.Person, src => src.Person.Adapt<Person.Response>())
                .Map(dest => dest.Book, src => src.Book.Adapt<Book.Response.Without.All>())
                .Map(dest => dest.DaysDelay, src => (DateTimeOffset.Now - src.TimeReturn).TotalDays);
        }
    }
}
