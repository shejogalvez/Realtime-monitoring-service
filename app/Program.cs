// See https://aka.ms/new-console-template for more information
using app.bots;
using app;

// obtain filepath as command line argument or use default
string ConfigurationFilePath = args.ElementAtOrDefault(0) ?? "configuration.json";

// initialize weather bots from configuration file
var configReader = new ConfigurationReader(ConfigurationFilePath);
List<WeatherBot> bots = configReader.GetBotsFromConfiguration();

// subscribe bots to incoming weather data
var DataManager = new WeatherDataReceiver();
foreach (var bot in bots)
{
    DataManager.WeatherDataReceived += bot.OnWeatherDataReceived;
}

// read weather data
while (true)
{
    Console.WriteLine("\ninput weather data: ");
    DataManager.ReadFromUser();
}