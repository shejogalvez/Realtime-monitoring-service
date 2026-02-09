namespace app.parsing;
public interface IParser
{
    Result<T>? Parse<T>(string trimmedInput);
    Result<T> TryToParse<T>(string? input) where T : notnull;
}