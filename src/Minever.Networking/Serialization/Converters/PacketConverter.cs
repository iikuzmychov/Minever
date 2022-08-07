using Minever.Networking.IO;

namespace Minever.Networking.Serialization;

public abstract class PacketConverter
{
    public abstract bool CanConvert(Type type);

    public abstract object Read(MinecraftReader reader, Type targetType);

    public abstract void Write(MinecraftWriter writer, object value);
}

public abstract class PacketConverter<T> : PacketConverter
    where T : notnull
{
    public override bool CanConvert(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return typeof(T) == type;
    }

    public abstract T Read(MinecraftReader reader);

    public abstract void Write(MinecraftWriter writer, T value);

    public sealed override object Read(MinecraftReader reader, Type targetType) => Read(reader);

    public sealed override void Write(MinecraftWriter writer, object value) => Write(writer, (T)value);
}
