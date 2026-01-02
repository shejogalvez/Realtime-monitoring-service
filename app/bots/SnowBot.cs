// See https://aka.ms/new-console-template for more information
namespace app.bots;

class SnowBot: WeatherBot
{
    public required int TemperatureThreshold {get; init;}
    protected override bool IsActivationConditionTrue(WeatherData data)
    {
        return data.Temperature < TemperatureThreshold;
    }
}