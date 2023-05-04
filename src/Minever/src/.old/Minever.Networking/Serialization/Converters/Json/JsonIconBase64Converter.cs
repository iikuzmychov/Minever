using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minever.Networking.Serialization.Json;

public class JsonIconBase64Converter : JsonConverter<byte[]>
{
    private const string Prefix = "data:image/png;base64,";

    public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var iconBase64 = reader.GetString()!;

        if (iconBase64.StartsWith(Prefix))
            iconBase64 = iconBase64[Prefix.Length..];

        return Convert.FromBase64String(iconBase64);
    }

    public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
    {
        var base64String = Convert.ToBase64String(value);
        writer.WriteStringValue(Prefix + base64String);
    }
}
