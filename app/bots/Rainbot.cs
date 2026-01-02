// See https://aka.ms/new-console-template for more information
namespace app.bots;

class RainBot : WeatherBot
{
    public required int HumidityThreshold {get; init;}
    protected override bool IsActivationConditionTrue(WeatherData data)
    {
        return data.Humidity > HumidityThreshold;
    }
}