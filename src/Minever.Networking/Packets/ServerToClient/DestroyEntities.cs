using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;

namespace Minever.Networking.Packets;

public sealed record DestroyEntities
{
    private int[] _entityIds = Array.Empty<int>();

    [PacketPropertyOrder(1)]
    [PacketConverter(typeof(PrefixedArrayPacketConverter<byte, int>))]
    public int[] EntityIds
    {
        get => _entityIds;
        init => _entityIds = value ?? throw new ArgumentNullException(nameof(value));
    }

    public DestroyEntities() { }

    public DestroyEntities(int[] entityIds)
    {
        EntityIds = entityIds;
    }
}
