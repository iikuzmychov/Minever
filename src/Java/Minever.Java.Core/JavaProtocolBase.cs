using System.Collections.ObjectModel;

namespace Minever.Java.Core;

public abstract class JavaProtocolBase : IJavaProtocol
{
    protected abstract ReadOnlyDictionary<JavaPacketContext, ReadOnlyBidirectionalDictionary<int, Type>> Packets { get; }

    public abstract int Version { get; }

    public int GetPacketId(Type type, JavaPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(type);

        try
        {
            return Packets[context].Inverse[type];
        }
        catch (KeyNotFoundException exception)
        {
            throw new NotSupportedException($"The packet of type {type} is not supported by the protocol for specified context.", exception);
        }
    }

    public Type GetPacketType(int id, JavaPacketContext context)
    {
        try
        {
            return Packets[context][id];
        }
        catch (KeyNotFoundException exception)
        {
            throw new NotSupportedException($"The packet with id {id} is not supported by the protocol for specified context.", exception);
        }
    }

    public bool IsPacketSupported(object packet, JavaPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(packet);
        return Packets.TryGetValue(context, out var packets) && packets.ContainsValue(packet.GetType());
    }

    public abstract JavaConnectionState GetNextConnectionState(object packet, JavaPacketContext context);
}
