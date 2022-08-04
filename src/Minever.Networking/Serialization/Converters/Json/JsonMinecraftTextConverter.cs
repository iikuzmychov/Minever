using Minever.Networking.DataTypes.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minever.Networking.Serialization.Converters.Json;

public class JsonMinecraftTextConverter : JsonConverter<MinecraftText>
{
    public override MinecraftText Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);
        ArgumentNullException.ThrowIfNull(options);

        var jsonElement = JsonElement.ParseValue(ref reader);

        if (jsonElement.ValueKind == JsonValueKind.String)
            return new StringText(jsonElement.GetString()!);
        
        if (jsonElement.ValueKind != JsonValueKind.Object)
            throw new InvalidDataException("JSON data is invalid to convert.");

        if (jsonElement.TryGetProperty("text", out _))
            return jsonElement.Deserialize<StringText>()!;
        else if (jsonElement.TryGetProperty("translate", out _))
            return jsonElement.Deserialize<TranslationText>()!;
        else
            throw new NotImplementedException();

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

        throw new NotImplementedException();

        //writer.WriteStringValue(JsonSerializer.Serialize(value, options));
    }
}
