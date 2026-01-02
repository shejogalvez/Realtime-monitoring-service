using System.Text.Json;

namespace app.parsing;
class JsonDeserializer : Parser
{
    protected override T? Parse<T>(string trimmedInput) where T : default
    {
        // Trim leading/trailing whitespace for accurate first-char check and parsing
        if (!trimmedInput.StartsWith('{')) return default;
        return JsonSerializer.Deserialize<T>(trimmedInput);
    }
}