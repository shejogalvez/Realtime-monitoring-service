namespace app.parsing;

public abstract class Parser : IParser
{
    public Result<T> TryToParse<T>(string? input) where T : notnull
    {
        if (string.IsNullOrWhiteSpace(input)) return Result<T>.Fail("empty string is not parsable");
        // Trim leading/trailing whitespace for accurate first-char check and parsing
        string trimmedInput = input.Trim();
        var result = Parse<T>(trimmedInput);
        if (result is null) return Result<T>.Fail($"Invalid initial token {input.First()}");
        return result;
        
    }
    
    public abstract Result<T>? Parse<T>(string trimmedInput);
}