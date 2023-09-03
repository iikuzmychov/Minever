using System.Buffers.Binary;
using Minever.LowLevel.Core.IO;
using Minever.LowLevel.Core.Packets.Serialization.Converters;

namespace Minever.LowLevel.Java.Protocols.V5.Packets.Serialization.Converters;

public class PacketFloat255Converter : PacketConverter<float>
{
    public override float Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);
        
        return reader.ReadFloat() * 250f;
    }

    public override void Write(MinecraftWriter writer, float value)
        => throw new NotImplementedException();
}