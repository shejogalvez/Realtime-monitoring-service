using System.Reflection;

namespace app.parsing;

public class OmniParser : Parser {

    private readonly static Lazy<List<Parser>> AllParsers = new (GenerateParsers);

    private static List<Parser> GenerateParsers()
    {
        IEnumerable<Parser> instances = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(Parser)) && !type.IsAbstract && type != typeof(OmniParser))
            .Select(parserType => Activator.CreateInstance(parserType)!)
            .Cast<Parser>();
        return [.. instances];
    }

    public override Result<T>? Parse<T>(string trimmedInput) where T : default
    {
        Result<T>? result = null;
        foreach (Parser parser in AllParsers.Value)
        {
            result = parser.Parse<T>(trimmedInput);
            if (result is not null) break;
        }
        return result;
    }
}