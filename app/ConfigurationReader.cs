using System.Reflection;
using System.Text.Json;
using app.bots;
using app.IOAbstractions;

namespace app;

public class ConfigurationReader(string ConfigurationFilePath, ILogger logger, IJsonFileDeserializer deserializer)
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
        // extract the class name and the initializer object as json
        var result = deserializer.Deserialize<Dictionary<string, JsonElement>>(ConfigurationFilePath);
        if (result is null)
        {
            throw new Exception("Unknown error parsing configuration file");
        }
        return result;
    }
    private WeatherBot? CreateFromJson(JsonElement json, Type botType)
    {
        WeatherBot? bot = (WeatherBot?) deserializer.Deserialize(json, botType, JsonOptions);
        bot?.SetLogger(logger);
        return bot;
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
                logger.WriteLine($"Warning: Not recognized bot type \"{botClass}\", skipping entry...");
                continue;
            }
            WeatherBot? bot = CreateFromJson(json, type);
            if (bot is null)
            {
                logger.WriteLine($"Warning: unable to create {botClass} from configuration file, skipping entry...");
                continue;
            }
            res.Add(bot);
        }
        return res;
    }
}