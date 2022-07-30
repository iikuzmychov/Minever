using Minever.Networking.IO;

namespace Minever.Networking.Serialization.Converters;

public class PrefixedArrayPacketConverter<TPrefix, TElement> : PacketConverter<TElement[]>
    where TPrefix : notnull
    where TElement : notnull
{
    private readonly PacketConverter _prefixConverter;
    private readonly PacketConverter _elementConverter;

    public PrefixedArrayPacketConverter(PacketConverter prefixConverter, PacketConverter elementConverter)
    {
        _prefixConverter  = prefixConverter ?? throw new ArgumentNullException(nameof(prefixConverter));
        _elementConverter = elementConverter ?? throw new ArgumentNullException(nameof(elementConverter));
    }

    public PrefixedArrayPacketConverter() : this(DefaultPacketConverter.Shared, DefaultPacketConverter.Shared) { }

    public override TElement[] Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        var arrayLength = Convert.ToInt32(_prefixConverter.Read(reader, typeof(TPrefix)));
        var array       = new TElement[arrayLength];

        for (int i = 0; i < arrayLength; i++)
            array[i] = (TElement)_elementConverter.Read(reader, typeof(TElement));

        return array;
    }

    public override void Write(TElement[] value, MinecraftWriter writer)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(writer);

        _prefixConverter.Write(Convert.ChangeType(value.Length, typeof(TPrefix)), writer);

        foreach (var element in value)
            _elementConverter.Write(element, writer);
    }
}

public sealed class PrefixedArrayPacketConverter<TPrefix, TElement, TPrefixConverter, TElementConverter>
    : PrefixedArrayPacketConverter<TPrefix, TElement>
    where TPrefix : notnull
    where TElement : notnull
    where TPrefixConverter : PacketConverter, new()
    where TElementConverter : PacketConverter, new()
{
    public PrefixedArrayPacketConverter() : base(new TPrefixConverter(), new TElementConverter()) { }
}
