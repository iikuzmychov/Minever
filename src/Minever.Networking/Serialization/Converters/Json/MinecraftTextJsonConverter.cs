using Minever.Networking.DataTypes.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minever.Networking.Serialization.Converters.Json;

public class MinecraftTextJsonConverter : JsonConverter<MinecraftText>
{
    public override MinecraftText Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);
        ArgumentNullException.ThrowIfNull(options);

        if (!CanConvert(typeToConvert))
            throw new NotSupportedException("The type not supported by the converter.");

        var jsonElement = JsonElement.ParseValue(ref reader);

        if (jsonElement.ValueKind == JsonValueKind.String)
            return new MinecraftStringText(jsonElement.GetString()!);

        return jsonElement.Deserialize<MinecraftStringText>()!; // временная заглушка

        /*if (jsonDocument.RootElement.TryGetProperty("extra", out JsonElement siblings))
        {
            foreach (var element in siblings.EnumerateArray())
            {
                if (jsonDocument.RootElement)
            }
        }*/
    }

    public override void Write(Utf8JsonWriter writer, MinecraftText value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(options);

        writer.WriteStringValue(JsonSerializer.Serialize(value, options)); // временная заглушка
    }
}
