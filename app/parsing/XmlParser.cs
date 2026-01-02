using System.Xml.Serialization;

namespace app.parsing;

class XmlDeserializer : Parser
{

    protected override T? Parse<T>(string trimmedInput) where T : default
    {
        if (!trimmedInput.StartsWith('<')) return default;
        var serializer = new XmlSerializer(typeof(T));
        return (T?) serializer.Deserialize(new StringReader(trimmedInput));
    }
}