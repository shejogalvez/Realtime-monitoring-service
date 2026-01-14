// See https://aka.ms/new-console-template for more information
using app.IOAbstractions;

namespace app.bots;

public abstract class WeatherBot(ILogger logger)
{
    protected abstract string BotName {get;}
    public required bool Enabled {get; init;}
    public required string Message {get; init;}
    private ILogger _logger = logger;

    protected WeatherBot() : this(new Logger()) {}
    protected virtual bool IsActivationConditionTrue(WeatherData data) => false;
    void Activate()
    {
        _logger.WriteLine($"{BotName} activated!");
        _logger.WriteLine($"{BotName}: {Message}");
    }
    public bool IsEnabled() => Enabled;

    public void OnWeatherDataReceived(WeatherData data)
    {
        if (IsActivationConditionTrue(data)) Activate();
    }

    public void SubscribeToWeatherData(IWeatherDataReceiver dataManager)
    {
        if (Enabled) dataManager.WeatherDataReceived += OnWeatherDataReceived;
    }
}