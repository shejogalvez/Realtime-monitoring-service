
using app;
using app.IOAbstractions;
using app.parsing;
using FluentAssertions;
using Moq;

public class OmniParserShould
{
    private readonly Mock<ILogger> mockLogger = new ();
    private IParser _parser = new OmniParser();
    public OmniParserShould()
    {
        
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void FailOnEmptyString(string? input)
    {
        Result res = _parser.TryToParse<WeatherData>(input);

        res.IsFailure.Should().BeTrue();
        res.Error.Should().Be("empty string is not parsable");
    }
    [Theory]
    [InlineData("{}")]
    [InlineData("""{"data": 32}""")]
    [InlineData("""{"Location": "string", "Temperature": 32}""")]
    public void FailOnIncompleteJson(string? input)
    {
        Result res = _parser.TryToParse<WeatherData>(input);

        res.IsFailure.Should().BeTrue();
        res.Error.Should().Contain("JSON deserialization for type 'app.WeatherData' was missing required properties");
    }
    [Theory]
    [InlineData("<WeatherData></WeatherData>")]
    [InlineData("""<WeatherData><Location>City Name</Location></WeatherData>""")]
    [InlineData("""<WeatherData><Temperature>32</Temperature><Humidity>40</Humidity></WeatherData>""")]
    public void FailOnIncompleteXml(string? input)
    {
        Result res = _parser.TryToParse<WeatherData>(input);

        res.IsFailure.Should().BeTrue();
        res.Error.Should().MatchRegex("The .* field is required.", AtLeast.Once());
    }

    [Theory]
    [InlineData("""<WeatherData><Location>City Name</Location><Temperature>32</Temperature><Humidity>40</Humidity></WeatherData>""")]
    [InlineData("""{"Location": "City Name", "Temperature": 32, "Humidity": 40}""")]
    public void ReturnCorrectJsonResult(string? input)
    {
        Result<WeatherData> res = _parser.TryToParse<WeatherData>(input);

        res.IsSuccess.Should().BeTrue();
        res.Value.Should().BeEquivalentTo(new WeatherData(){Location= "City Name", Temperature= 32, Humidity= 40});
    }

    [Fact]
    public void FailOnBadSyntaxXML()
    {
        var input = "<Data> Hello <d>";

        Result<WeatherData> res = _parser.TryToParse<WeatherData>(input);

        res.IsFailure.Should().BeTrue();
        res.Error.Should().Contain("There is an error in XML document");
    }
}