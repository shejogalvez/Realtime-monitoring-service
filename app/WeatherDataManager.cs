using app.parsing;

namespace app;

class WeatherDataReceiver
{    
    public event Action<WeatherData>? WeatherDataReceived;

    public void ReadFromUser()
    {
        string? userInput = Console.ReadLine();
        if (userInput is null) return;
        var data = Parser.TryToParseAllFormats<WeatherData>(userInput);
        if (data is null)
        {
            Console.WriteLine($"was not able to parse the input: {userInput}");
            return;
        }
        WeatherDataReceived?.Invoke(data);
    }
}