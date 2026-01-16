// See https://aka.ms/new-console-template for more information
using app.bots;
using app;
using app.parsing;
using app.IOAbstractions;

public class Program
{
    public static void Run(string ConfigurationFilePath, Parser parser, ILogger logger, IInputReader reader)
    {
        // initialize weather bots from configuration file
        var configReader = new ConfigurationReader(ConfigurationFilePath);
        List<WeatherBot> bots = configReader.GetBotsFromConfiguration();

        // subscribe bots to incoming weather data
        var DataManager = new WeatherDataReceiver(parser, reader, logger);
        foreach (var bot in bots)
        {
            bot.SubscribeToWeatherData(DataManager);
        }

        // read weather data
        do
        {
            logger.WriteLine("\ninput weather data: ");
        } 
        while (DataManager.ReadFromUser());
    }

    public static void Main(string[] args) {
        // Create Dependencies object
        var parser = new OmniParser(); 
        var logger = new Logger(); 
        var reader = new InputReader(); 

        // obtain filepath as command line argument or use default
        string ConfigurationFilePath = args.ElementAtOrDefault(0) ?? "configuration.json";

        // Run Program
        Run(ConfigurationFilePath, parser, logger, reader);
    }
}


