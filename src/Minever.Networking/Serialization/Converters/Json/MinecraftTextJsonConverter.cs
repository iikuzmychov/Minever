using Minever.Networking.DataTypes.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minever.Networking.Serialization.Converters.Json;

internal class MinecraftTextJsonConverter : JsonConverter<MinecraftText>
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (typeToConvert is null)
            throw new ArgumentNullException(nameof(typeToConvert));

        return new[] { typeof(MinecraftText), typeof(MinecraftStringText) }.Contains(typeToConvert);
    }

    public override MinecraftText Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert is null)
            throw new ArgumentNullException(nameof(typeToConvert));

        if (typeToConvert is null)
            throw new ArgumentNullException(nameof(typeToConvert));

        if (!CanConvert(typeToConvert))
            throw new NotSupportedException("Converter does not support the type.");

        var jsonElement = JsonElement.ParseValue(ref reader);

        if (jsonElement.ValueKind == JsonValueKind.String)
        {
            if (typeToConvert != typeof(MinecraftText) && typeToConvert != typeof(MinecraftStringText))
                throw new InvalidDataException($"Current JSON-data can not be parsed to object of type '{typeToConvert}'.");
            else
                return new MinecraftStringText(jsonElement.GetString()!);
        }

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
        if (writer is null)
            throw new ArgumentNullException(nameof(writer));

        if (value is null)
            throw new ArgumentNullException(nameof(value));

        writer.WriteStringValue(JsonSerializer.Serialize(value, options)); // временная заглушка
    }
}
