using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleLibraryWithBooks.Options
{
    public static class SerializerOptions
    {
        public static JsonSerializerOptions WhenWritingDefault { get; private set; }

        public static void Init()
        {
            WhenWritingDefault = new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault };
        }
    }
}
