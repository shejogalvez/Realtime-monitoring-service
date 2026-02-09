// See https://aka.ms/new-console-template for more information
using app.IOAbstractions;

namespace app.bots;

public class RainBot : WeatherBot
{
    public required int HumidityThreshold {get; init;}
    protected override string BotName => nameof(RainBot);

    protected override bool IsActivationConditionTrue(WeatherData data)
    {
        return data.Humidity > HumidityThreshold;
    }
    public RainBot(ILogger logger) : base(logger) {}
    public RainBot() : base() {}
}