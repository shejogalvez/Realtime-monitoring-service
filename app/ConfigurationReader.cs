// See https://aka.ms/new-console-template for more information
using System.Reflection;
using System.Text.Json;
using app.bots;
class ConfigurationReader(string ConfigurationFilePath)
{
    private readonly static Lazy<IEnumerable<Type>> botTypes = new (GetBotTypes);
    private static JsonSerializerOptions JsonOptions {get; set;} = new ()
    {
        PropertyNameCaseInsensitive = true
    };

    private static IEnumerable<Type> GetBotTypes() => 
        Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(type => type.IsAssignableTo(typeof(WeatherBot)));
    private Dictionary<string, JsonElement> ExtractClassnameWithJsonInitializers()
    {
        using var file = File.Open(ConfigurationFilePath, FileMode.Open, FileAccess.Read);
        // extract the class name and the initializer object as json
        var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(file);
        if (result is null)
        {
            Console.WriteLine("Unknown error parsing configuration file");
            return [];
        }
        return result;
    }
    public List<WeatherBot> GetBotsFromConfiguration()
    {
        var botsJsonDictionary = ExtractClassnameWithJsonInitializers();

        List<WeatherBot> res = [];
        foreach (var (botClass, json) in botsJsonDictionary)
        {
            Type? type = botTypes.Value.FirstOrDefault(type => type.Name.Equals(botClass, StringComparison.InvariantCultureIgnoreCase));
            if (type is null)
            {
                Console.WriteLine($"Warning: Not recognized bot type \"{botClass}\", skipping entry...");
                continue;
            }
            WeatherBot? bot = (WeatherBot?) JsonSerializer.Deserialize(json, type, JsonOptions);
            if (bot is null)
            {
                Console.WriteLine($"Warning: unable to create {botClass} from configuration file, skipping entry...");
                continue;
            }
            res.Add(bot);
        }
        return res;
    }
}