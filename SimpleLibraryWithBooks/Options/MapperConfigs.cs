using Mapster;
using Database.Models;
using SimpleLibraryWithBooks.Models;

namespace SimpleLibraryWithBooks.Options
{
    public static class MapperConfigs
    {
        public static TypeAdapterConfig ForGenreStatistic { get; private set; }

        public static void Init()
        {
            ForGenreStatistic = new();
            ForGenreStatistic
                .NewConfig<GenreEntity, Genre.Response.Statistic>()
                .Map(dest => dest.CountBooks, src => src.Books.Count);
        }
    }
}
