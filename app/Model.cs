using System.ComponentModel.DataAnnotations;

namespace app;

public record WeatherData
{
    [Required]
    public required string Location {get; init;}
    [Required]
    public required int? Temperature {get; init;}
    [Required]
    public required int? Humidity {get; init;}
}