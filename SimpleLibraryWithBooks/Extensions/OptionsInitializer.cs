using SimpleLibraryWithBooks.Options;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleLibraryWithBooks.Extensions
{
    public static class OptionsInitializer
    {
        public static IServiceCollection InitOptions(this IServiceCollection services)
        {
            SerializerOptions.Init();
            MapperConfigs.Init();

            return services;
        }
    }
}
