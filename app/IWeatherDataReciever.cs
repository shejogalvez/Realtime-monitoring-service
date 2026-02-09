namespace app;
public interface IWeatherDataReceiver
{
    event Action<WeatherData>? WeatherDataReceived;

    bool ReadFromUser();
}
