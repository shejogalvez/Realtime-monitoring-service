// See https://aka.ms/new-console-template for more information
using app.IOAbstractions;

namespace app.bots;

public class SnowBot: WeatherBot
{
    public required int TemperatureThreshold {get; init;}
    protected override string BotName => nameof(SnowBot);
    protected override bool IsActivationConditionTrue(WeatherData data)
    {
        return data.Temperature < TemperatureThreshold;
    }
    public SnowBot(ILogger logger) : base(logger) {}
    public SnowBot() : base() {}
}