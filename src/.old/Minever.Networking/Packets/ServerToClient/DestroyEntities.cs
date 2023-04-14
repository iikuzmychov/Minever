using Minever.Networking.Serialization;

namespace Minever.Networking.Packets;

public sealed record DestroyEntities
{
    private int[] _entityIds = Array.Empty<int>();

    [PacketPropertyOrder(1)]
    [PacketConverter(typeof(PacketPrefixedArrayConverter<sbyte, int>))]
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
