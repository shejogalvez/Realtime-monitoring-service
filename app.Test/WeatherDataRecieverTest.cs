using Moq;
using FluentAssertions;
using app.IOAbstractions;
using app.parsing;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace app.Test;
public class WeatherDataReceiverShould
{
    private readonly Mock<ILogger> mockLogger = new();
    private readonly Mock<IInputReader> mockReader = new();
    private readonly Mock<IParser> mockParser = new();

    [Fact]
    public void EmitSignalOnCorrectData()
    {
        List<WeatherData> DataSended = [];
        var data = new WeatherData(){Temperature=32, Humidity=50, Location="test location"};
        mockParser.Setup(x => x.TryToParse<WeatherData>(It.IsAny<string>()))
        .Returns(Result<WeatherData>.Ok(data));
        var sut = new WeatherDataReceiver(mockParser.Object, mockReader.Object, mockLogger.Object);
        sut.WeatherDataReceived += DataSended.Add;

        sut.ReadFromUser();

        DataSended.Should().Contain(data);
        mockLogger.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Never);
    }
    [Fact]
    public void LogFailureOfParsing()
    {
        var errorMessage = "error";
        mockParser.Setup(x => x.TryToParse<WeatherData>(It.IsAny<string>()))
        .Returns(Result<WeatherData>.Fail(errorMessage));
        var sut = new WeatherDataReceiver(mockParser.Object, mockReader.Object, mockLogger.Object);

        sut.ReadFromUser();

        mockLogger.Verify(x => x.WriteLine($"Parsing failed with error: \n   {errorMessage}"));
    }
}