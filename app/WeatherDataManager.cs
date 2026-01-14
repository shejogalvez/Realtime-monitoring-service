using app.IOAbstractions;
using app.parsing;

namespace app;

public class WeatherDataReceiver(Parser parser, IInputReader reader, ILogger logger) : IWeatherDataReceiver
{
    public event Action<WeatherData>? WeatherDataReceived;

    public void ReadFromUser()
    {
        string? userInput = reader.ReadLine();
        Result<WeatherData> data = parser.TryToParse<WeatherData>(userInput);
        if (data.IsFailure)
        {
            logger.WriteLine($"Parsing failed with error: \n   {data.Error}");
            return;
        }
        WeatherDataReceived?.Invoke(data.Value);
    }
}