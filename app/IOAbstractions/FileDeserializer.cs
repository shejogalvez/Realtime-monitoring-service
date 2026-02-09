using System.Text.Json;

namespace app.IOAbstractions
{
    public class JsonFileDeserializer : IJsonFileDeserializer
    {
        public T? Deserialize<T>(string filePath)
        {
            using var file = File.Open(filePath, FileMode.Open, FileAccess.Read);
            // extract the class name and the initializer object as json
            return JsonSerializer.Deserialize<T>(file);
        }

        public object? Deserialize(JsonElement jsonElement, Type type, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize(jsonElement, type, options);
        }
    }
}