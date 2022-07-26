﻿using Minever.Networking.IO;

namespace Minever.Networking.Packets.Serialization.Converters;

public class VarIntPacketConverter : MinecraftPacketConverter<int>
{
    public override int Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        return reader.Read7BitEncodedInt();
    }

    public override void Write(int value, MinecraftWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.Write7BitEncodedInt(value);
    }
}
