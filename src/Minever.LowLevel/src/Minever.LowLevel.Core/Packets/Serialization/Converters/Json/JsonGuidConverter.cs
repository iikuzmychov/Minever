using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minever.LowLevel.Core.Packets.Serialization.Converters.Json;

public class JsonGuidConverter : JsonConverter<Guid>
{
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => Guid.Parse(reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString());
}
