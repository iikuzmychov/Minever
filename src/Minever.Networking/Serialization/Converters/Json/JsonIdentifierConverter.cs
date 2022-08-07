using Minever.Networking.DataTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minever.Networking.Serialization.Json;

public class JsonIdentifierConverter : JsonConverter<Identifier>
{
    public override Identifier? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        Identifier.Parse(reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, Identifier value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString());
}
