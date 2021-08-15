using SimpleLibraryWithBooks.Options;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleLibraryWithBooks.Extensions
{
    public static class OptionsInitializer
    {
        public static IServiceCollection InitOptions(this IServiceCollection services)
        {
            MapperConfigs.Init();

            return services;
        }
    }
}
