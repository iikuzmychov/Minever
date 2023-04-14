using Minever.Core.IO;

namespace Minever.Core.Packets.Serialization.Converters;

public class PacketTimeSpanFromToMinecraftTicksConverter<TSeconds> : PacketConverter<TimeSpan>
{
    private readonly PacketConverter _secondsConverter = GetConverter(typeof(TSeconds));

    public override TimeSpan Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        var minecraftTicks = (TSeconds)_secondsConverter.Read(reader, typeof(TSeconds));
        var seconds        = Convert.ToDouble(minecraftTicks) / 20d;

        return TimeSpan.FromSeconds(seconds);
    }

    public override void Write(MinecraftWriter writer, TimeSpan value)
    {
        ArgumentNullException.ThrowIfNull(writer);

        var minecraftTicks = value.TotalSeconds * 20d;
        var seconds        = (TSeconds)Convert.ChangeType(minecraftTicks, typeof(TSeconds));

        _secondsConverter.Write(writer, seconds);
    }
}
