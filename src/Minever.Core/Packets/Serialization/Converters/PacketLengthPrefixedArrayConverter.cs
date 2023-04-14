using Minever.Core.IO;

namespace Minever.Core.Packets.Serialization.Converters;

// todo: add PacketLengthPrefixedByteArrayConverter ???
public class PacketLengthPrefixedArrayConverter<TPrefix, TElement> : PacketConverter<TElement[]>
    where TPrefix : notnull
    where TElement : notnull
{
    private readonly PacketConverter _prefixConverter;
    private readonly PacketConverter _elementConverter;

    public PacketLengthPrefixedArrayConverter() : this(GetConverter(typeof(TPrefix)), GetConverter(typeof(TElement)))
    {
    }

    public PacketLengthPrefixedArrayConverter(PacketConverter prefixConverter, PacketConverter elementConverter)
    {
        _prefixConverter  = prefixConverter ?? throw new ArgumentNullException(nameof(prefixConverter));
        _elementConverter = elementConverter ?? throw new ArgumentNullException(nameof(elementConverter));
    }

    public override TElement[] Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        var arrayLength = Convert.ToInt32(_prefixConverter.Read(reader, typeof(TPrefix)));
        var array       = new TElement[arrayLength];

        for (int i = 0; i < arrayLength; i++)
        {
            array[i] = (TElement)_elementConverter.Read(reader, typeof(TElement));
        }

        return array;
    }

    public override void Write(MinecraftWriter writer, TElement[] value)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(writer);

        _prefixConverter.Write(writer, Convert.ChangeType(value.Length, typeof(TPrefix)));

        foreach (var element in value)
        {
            _elementConverter.Write(writer, element);
        }
    }
}

public sealed class PacketLengthPrefixedArrayConverter<TPrefix, TElement, TPrefixConverter, TElementConverter>
    : PacketLengthPrefixedArrayConverter<TPrefix, TElement>
    where TPrefix : notnull
    where TElement : notnull
    where TPrefixConverter : PacketConverter, new()
    where TElementConverter : PacketConverter, new()
{
    public PacketLengthPrefixedArrayConverter() : base(new TPrefixConverter(), new TElementConverter()) { }
}
