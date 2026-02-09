using System.Xml;
using System.Xml.Serialization;
using app.Validation;

namespace app.parsing;

public class XmlParser : Parser
{
    public override Result<T>? Parse<T>(string trimmedInput) where T : default
    {
        if (!trimmedInput.StartsWith('<')) return null;
        var serializer = new XmlSerializer(typeof(T));
        var reader = XmlReader.Create(new StringReader(trimmedInput));
        try
        {
            var result = (T?) serializer.Deserialize(reader);
            if (result is null) return Result<T>.Fail("Null object encountered on deserialization");
            var validationResults = ValidationUtils.Validate(result);
            if (validationResults.Count > 0)
            {
                return Result<T>.Fail(string.Join(" ", validationResults.Select(x => x.ErrorMessage)));
            }
            return Result<T>.Ok(result);
        }
        catch (Exception e)
        {
            return Result<T>.Fail(e.Message);
        }
    }
}