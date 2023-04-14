using Minever.Networking.IO;
using System.Text.Json;

namespace Minever.Networking.Serialization;

public class JsonDataPacketConverter : PacketConverter
{
    public override bool CanConvert(Type type) => true;

    public override object Read(MinecraftReader reader, Type targetType)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(targetType);

        var json = reader.ReadString();

        return JsonSerializer.Deserialize(json, targetType)!;
    }

    public override void Write(MinecraftWriter writer, object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(writer);

        var json = JsonSerializer.Serialize(value)!;

        writer.Write(json);
    }
}
