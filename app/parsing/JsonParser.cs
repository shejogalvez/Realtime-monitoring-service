using System.Text.Json;

namespace app.parsing;
public class JsonParser : Parser
{
    public override Result<T>? Parse<T>(string trimmedInput) where T : default
    {
        // Trim leading/trailing whitespace for accurate first-char check and parsing
        if (!trimmedInput.StartsWith('{')) return null;
        try
        {
            var result = JsonSerializer.Deserialize<T>(trimmedInput);
            if (result is null) return Result<T>.Fail("Null object encountered on deserialization");
            return Result<T>.Ok(result);
        }
        catch (Exception e)
        {
            return Result<T>.Fail(e.Message);
        }
    }
}