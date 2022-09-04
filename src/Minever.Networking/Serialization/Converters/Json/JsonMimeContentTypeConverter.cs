using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minever.Networking.Serialization.Json;

public class JsonMimeContentTypeConverter : JsonConverter<ContentType>
{
    public override ContentType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        new(reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, ContentType value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString());
}
