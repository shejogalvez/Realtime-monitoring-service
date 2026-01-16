using app.IOAbstractions;
using app.parsing;
using FluentAssertions;
using Moq;
using Xunit.Abstractions;

namespace app.Test;
public class IntegrationTest(ITestOutputHelper output)
{
    private readonly Mock<ILogger> mockLogger = new();
    private readonly Mock<IInputReader> mockReader = new();
    [Fact]
    public void HappyPath()
    {
        var dir = Directory.GetCurrentDirectory();
        output.WriteLine(dir);
        mockReader.SetupSequence(x => x.ReadLine())
        .Returns("""{"Location": "Test Name", "Temperature": 32, "Humidity": 80}""")
        .Returns("exit");
        var configPath = "../../../TestConfiguration.json";
        string Message = "Wow, it's a scorcher test out there!";

        Program.Run([configPath], new OmniParser(), mockLogger.Object, mockReader.Object, new JsonFileDeserializer());

        mockReader.Verify(x => x.ReadLine(), Times.Exactly(2));
        mockLogger.Verify(x => x.WriteLine("\ninput weather data: "), Times.Exactly(2));
        mockLogger.Verify(x => x.WriteLine($"SunBot activated!"), Times.Once);
        mockLogger.Verify(x => x.WriteLine($"SunBot: {Message}"), Times.Once);
        mockLogger.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(4));
    }
}