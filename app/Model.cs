namespace app;

public record WeatherData
{
    public required string Location {get; init;}
    public required int Temperature {get; init;}
    public required int Humidity {get; init;}
}