namespace app;
public interface IWeatherDataReceiver
{
    event Action<WeatherData>? WeatherDataReceived;

    void ReadFromUser();
}
