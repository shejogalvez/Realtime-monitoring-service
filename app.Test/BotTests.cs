using app.bots;
using app.IOAbstractions;
using app.parsing;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using Moq;
using Xunit.Abstractions;

namespace app.Test;
public class WeatherBotShould
{
    private readonly ITestOutputHelper _output;
    private Mock<ILogger> _mockLogger = new ();
    private Fixture _fixture;
    private WeatherData _data = new () {Location="test city", Humidity=50, Temperature=20};
    public WeatherBotShould(ITestOutputHelper output)
    {
        _output = output;
        _fixture = new Fixture ();
    }
    // public static IEnumerable<object[]> ActivatingBotsData()
    // {
    //     Mock<ILogger> _mockLogger = new ();
    //     return
    //     [
    //         [new Mock<ILogger>(), new SnowBot (_mockLogger.Object) {Enabled=true, Message="SnowBot message", TemperatureThreshold=100}, "SnowBot"],
    //         [new Mock<ILogger>(), new RainBot (_mockLogger.Object) {Enabled=true, Message="RainBot message", HumidityThreshold=0}, "RainBot"],
    //         [new Mock<ILogger>(), new SunBot  (_mockLogger.Object) {Enabled=true, Message="SunBot message", TemperatureThreshold=0}, "SunBot"]
    //     ];
    // }
    // public static IEnumerable<object[]> NonActivatingBotsData()
    // {
    //     Mock<ILogger> _mockLogger = new ();
    //     return
    //     [
    //         [new Mock<ILogger>(), new SnowBot (_mockLogger.Object) {Enabled=true, Message="SnowBot message", TemperatureThreshold=100}, "SnowBot"],
    //         [new Mock<ILogger>(), new RainBot (_mockLogger.Object) {Enabled=true, Message="RainBot message", HumidityThreshold=0}, "RainBot"],
    //         [new Mock<ILogger>(), new SunBot  (_mockLogger.Object) {Enabled=true, Message="SunBot message", TemperatureThreshold=0}, "SunBot"]
    //     ];
    // }

    [Fact]
    public void ActivateSunBot()
    {
        WeatherBot bot = new SunBot (_mockLogger.Object) {Enabled=true, Message="SunBot message", TemperatureThreshold=0};
        bot.OnWeatherDataReceived(_data);
        _mockLogger.Verify(x => x.WriteLine(It.IsRegex($".*{nameof(SunBot)}.*")), Times.Exactly(2));
    }

    [Fact]
    public void ActivateRainBot()
    {
        WeatherBot bot = new RainBot (_mockLogger.Object) {Enabled=true, Message="SunBot message", HumidityThreshold=0};
        bot.OnWeatherDataReceived(_data);
        _mockLogger.Verify(x => x.WriteLine(It.IsRegex($".*{nameof(RainBot)}.*")), Times.Exactly(2));
    }

    [Fact]
    public void ActivateSnowBot()
    {
        WeatherBot bot = new SnowBot (_mockLogger.Object) {Enabled=true, Message="SunBot message", TemperatureThreshold=30};
        bot.OnWeatherDataReceived(_data);
        _mockLogger.Verify(x => x.WriteLine(It.IsRegex($".*{nameof(SnowBot)}.*")), Times.Exactly(2));
    }

    [Fact]
    public void IsEnabledBeFalseIfNotEnabled()
    {
        var _mockLogger = new Mock<ILogger>();
        WeatherBot bot = new SunBot (_mockLogger.Object) {Enabled=false, Message="SunBot message", TemperatureThreshold=0};
        
        var result = bot.IsEnabled();
        result.Should().BeFalse();
    }

    // semi-integration Test (?)
    [Fact]
    public void NotSubscribeIfNotEnabled()
    {
        Mock<IWeatherDataReceiver> mock = new ();
        WeatherBot bot = new SunBot (_mockLogger.Object) {Enabled=false, Message="SunBot message", TemperatureThreshold=0};

        bot.SubscribeToWeatherData(mock.Object);
        
        mock.Raise(e => e.WeatherDataReceived += null, new WeatherData() {Location="City Name", Humidity=40, Temperature=32});
        _mockLogger.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Never);

    }
    [Fact]
    public void SubscribeIfEnabled()
    {
        Mock<IWeatherDataReceiver> mock = new ();
        mock.SetupAdd(e => e.WeatherDataReceived += null).Verifiable();
        WeatherBot bot = new SunBot (_mockLogger.Object) {Enabled=true, Message="SunBot message", TemperatureThreshold=0};

        bot.SubscribeToWeatherData(mock.Object);

        mock.VerifyAdd(e => e.WeatherDataReceived += It.IsAny<Action<WeatherData>>(), Times.Once);

    }
}
