using System.Reflection;

namespace app.parsing;

static class ParserFactory{

    private readonly static Lazy<List<Parser>> AllParsers = new (GenerateParsers);

    private static List<Parser> GenerateParsers()
    {
        IEnumerable<Parser> instances = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(Parser)) && !type.IsAbstract)
            .Select(parserType => Activator.CreateInstance(parserType)!)
            .Cast<Parser>();
        return [.. instances];
    }

    public static List<Parser> GetAllParsers() => AllParsers.Value;
}