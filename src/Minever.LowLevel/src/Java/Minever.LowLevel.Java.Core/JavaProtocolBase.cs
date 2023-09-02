using Minever.LowLevel.Java.Core.Extensions;
using System.Collections.Immutable;
using System.Reflection;

namespace Minever.LowLevel.Java.Core;

public abstract class JavaProtocolBase : IJavaProtocol
{
    public abstract int Version { get; }
    public abstract IReadOnlyDictionary<JavaPacketContext, IReadOnlyBidirectionalDictionary<int, Type>> Packets { get; }

    protected static IReadOnlyDictionary<JavaPacketContext, IReadOnlyBidirectionalDictionary<int, Type>> FindPacketsInAssembly<TProtocol>(Assembly assembly)
        where TProtocol : IJavaProtocol
    {
        ArgumentNullException.ThrowIfNull(assembly);

        var packetTypes = assembly.GetTypes().Where(static type => type.GetCustomAttributes(false).OfType<JavaPacketAttribute<TProtocol>>().Any());
        
        return packetTypes
            .Select(static type => (Type: type, Attribute: type.GetCustomAttributes(false).OfType<JavaPacketAttribute<TProtocol>>().Single()))
            .GroupBy(static packet => packet.Attribute.Context)
            .ToImmutableDictionary(
                static group => group.Key,
                static group => group.ToReadOnlyBidirectionalDictionary(packet => packet.Attribute.Id, packet => packet.Type));
    }

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
            throw new NotSupportedException($"The packet with id 0x{id:x} is not supported by the protocol for specified context.", exception);
        }
    }

    public bool IsPacketSupported(object packet, JavaPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(packet);

        return Packets.TryGetValue(context, out var packets) && packets.ContainsValue(packet.GetType());
    }

    public abstract JavaConnectionState GetNextConnectionState(object packet, JavaPacketContext context);
}
