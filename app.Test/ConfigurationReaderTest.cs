
using System.Text.Json;
using app.bots;
using app.IOAbstractions;
using AutoFixture.Kernel;
using FluentAssertions;
using Moq;

namespace app.Test;
public class ConfigurationReaderTest
{
    Mock<IJsonFileDeserializer> _mockDeserializer = new ();
    Mock<ILogger> _mockLogger = new ();

    private void SetupExtractingBotNamesWithJson(Dictionary<string, JsonElement> dummy) =>
        _mockDeserializer.Setup(x => x.Deserialize<Dictionary<string, JsonElement>>(It.IsAny<string>()))
        .Returns(dummy).Verifiable();
    private void SetupExtractingBots(IEnumerable<WeatherBot> dummies)
    {
        var setup = 
        _mockDeserializer.SetupSequence(x => x.Deserialize(
            It.IsAny<JsonElement>(), 
            It.Is<Type>(type => type.IsAssignableTo(typeof(WeatherBot))), 
            It.IsAny<JsonSerializerOptions>()));
        foreach (WeatherBot dummy in dummies)
        {
            setup = setup.Returns(dummy);
        }
    }
    private void VerifyExtractingBots(int times)
    {
        _mockDeserializer.Verify(x => x.Deserialize(It.IsAny<JsonElement>(), It.Is<Type>(type => type.IsAssignableTo(typeof(WeatherBot))), It.IsAny<JsonSerializerOptions>()), Times.Exactly(times));
    }
    [Fact]
    public void ReadCorrectFile()
    {
        ConfigurationReader sut = new ("testPath", _mockLogger.Object, _mockDeserializer.Object);
        SetupExtractingBotNamesWithJson(new ()
        {
            {"SunBot", new JsonElement()},
            {"SnowBot", new JsonElement()},
            {"RainBot", new JsonElement()}
        });
        SunBot dummyBot =   new () {Enabled=true, Message="", TemperatureThreshold=0};
        SnowBot dummyBot2 = new () {Enabled=true, Message="", TemperatureThreshold=0};
        RainBot dummyBot3 = new () {Enabled=true, Message="", HumidityThreshold=0};
        SetupExtractingBots([dummyBot, dummyBot2, dummyBot3]);

        var result = sut.GetBotsFromConfiguration();

        result.Should().Contain(dummyBot).And.AllBeAssignableTo<WeatherBot>();
        _mockDeserializer.Verify();
        VerifyExtractingBots(times: 3);
    }
    [Fact]
    public void SkipNonExistentBotClass()
    {
        ConfigurationReader sut = new ("testPath", _mockLogger.Object, _mockDeserializer.Object);
        SetupExtractingBotNamesWithJson(new ()
        {
            {"SunBot", new JsonElement()},
            {"ThunderBot", new JsonElement()},
        });
        SunBot dummyBot =   new () {Enabled=true, Message="", TemperatureThreshold=0};
        SnowBot dummyBot2 = new () {Enabled=true, Message="", TemperatureThreshold=0};
        RainBot dummyBot3 = new () {Enabled=true, Message="", HumidityThreshold=0};
        SetupExtractingBots([dummyBot, dummyBot2, dummyBot3]);

        var result = sut.GetBotsFromConfiguration();

        result.Should().Contain(dummyBot).And.AllBeAssignableTo<WeatherBot>();
        _mockDeserializer.Verify();
        _mockLogger.Verify(x => x.WriteLine(It.Is<string>(x => x.Contains("Not recognized bot type"))));
        VerifyExtractingBots(times: 1);
    }
}