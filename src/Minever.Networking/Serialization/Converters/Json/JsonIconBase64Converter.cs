using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minever.Networking.Serialization.Converters.Json;

public class JsonIconBase64Converter : JsonConverter<string?>
{
    private const string Prefix = "data:image/png;base64,";

    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert is null)
            throw new ArgumentNullException(nameof(typeToConvert));

        var iconBase64 = reader.GetString();

        if (iconBase64 is not null && iconBase64.StartsWith(Prefix))
            iconBase64 = iconBase64[22..];

        return iconBase64;
    }

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        if (writer is null)
            throw new ArgumentNullException(nameof(writer));

        if (value is null)
            return;

        if (!value.StartsWith(Prefix))
            value = Prefix + value;

        writer.WriteStringValue(value);
    }
}
