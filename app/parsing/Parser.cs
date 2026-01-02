namespace app.parsing;

abstract class Parser
{
    public T? TryToParse<T>(string input) where T : notnull
    {
        if (string.IsNullOrWhiteSpace(input)) return default;
        // Trim leading/trailing whitespace for accurate first-char check and parsing
        string trimmedInput = input.Trim();
        try
        {
            return Parse<T>(trimmedInput);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Parsing failed with exception: {e.Message}");
            return default;
        }
    }
    protected abstract T? Parse<T>(string trimmedInput);

    public static T? TryToParseAllFormats<T>(string input) where T : notnull
    {
        if (string.IsNullOrWhiteSpace(input)) return default;
        string trimmedInput = input.Trim();
        var parsers = ParserFactory.GetAllParsers();
        T? result = default;
        try
        {
            foreach (Parser parser in parsers)
            {
                result = parser.Parse<T>(trimmedInput);
                if (result is not null) break;
            }
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Parsing failed with exception: {e.Message}");
            return default;
        }
    }
}