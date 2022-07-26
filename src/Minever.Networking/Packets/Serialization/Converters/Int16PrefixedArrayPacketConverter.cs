using Minever.Networking.IO;

namespace Minever.Networking.Packets.Serialization.Converters;

public class Int16PrefixedArrayPacketConverter<TElement, TElementConverter> : MinecraftPacketConverter<TElement[]>
    where TElement : notnull
    where TElementConverter : MinecraftPacketConverter, new()
{
    private TElementConverter _elementConverter = new();

    public Int16PrefixedArrayPacketConverter()
    {
        if (!_elementConverter.CanConvert(typeof(TElement)))
            throw new NotSupportedException($"Сonverter '{typeof(TElementConverter)} does not support type '{typeof(TElement)}'.");
    }

    public override TElement[] Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        var arrayLength = reader.ReadInt16();
        var array       = new TElement[arrayLength];

        for (int i = 0; i < arrayLength; i++)
            array[i] = (TElement)_elementConverter.Read(reader, typeof(TElement));

        return array;
    }

    public override void Write(TElement[] value, MinecraftWriter writer)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(writer);

        writer.Write((short)value.Length);

        foreach (var element in value)
            _elementConverter.Write(element, writer);
    }
}
