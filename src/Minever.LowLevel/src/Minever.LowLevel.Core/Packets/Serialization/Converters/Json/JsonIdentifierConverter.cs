using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minever.LowLevel.Core.Packets.Serialization.Converters.Json;

public class JsonParsableConverter<TParsable> : JsonConverter<TParsable>
    where TParsable : IParsable<TParsable>
{
    public override TParsable? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => TParsable.Parse(reader.GetString()!, CultureInfo.CurrentCulture);

    public override void Write(Utf8JsonWriter writer, TParsable value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString());
}
