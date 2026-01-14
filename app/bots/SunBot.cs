// See https://aka.ms/new-console-template for more information
using app.IOAbstractions;

namespace app.bots;

public class SunBot : WeatherBot
{
    public required int TemperatureThreshold {get; init;}
    protected override string BotName => nameof(SunBot);
    protected override bool IsActivationConditionTrue(WeatherData data)
    {
        return data.Temperature > TemperatureThreshold;
    }
    public SunBot(ILogger logger) : base(logger) {}
    public SunBot() : base() {}
}