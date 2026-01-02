// See https://aka.ms/new-console-template for more information
namespace app.bots;

abstract class WeatherBot
{
    protected string BotName;
    public required bool Enabled {get; init;}
    public required string Message {get; init;}

    protected WeatherBot()
    {
        BotName = GetType().Name;
    }
    protected virtual bool IsActivationConditionTrue(WeatherData data) => false;
    void Activate()
    {
        Console.WriteLine($"{BotName} activated!");
        Console.WriteLine($"{BotName}: {Message}");
    }
    public bool IsEnabled() => Enabled;

    public void OnWeatherDataReceived(WeatherData data)
    {
        if (IsActivationConditionTrue(data)) Activate();
    }
}