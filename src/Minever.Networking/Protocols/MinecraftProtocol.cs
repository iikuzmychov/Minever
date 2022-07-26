using BidirectionalMap;
using Minever.Networking.Packets;

namespace Minever.Networking.Protocols;

public abstract partial class MinecraftProtocol
{
    protected Dictionary<PacketContext, BiMap<int, Type>> SupportedPackets;

    public abstract int Version { get; }

    public MinecraftProtocol(Dictionary<PacketContext, BiMap<int, Type>> supportedPackets)
    {
        SupportedPackets = supportedPackets ?? throw new ArgumentNullException(nameof(supportedPackets));
    }

    public static MinecraftProtocol FromVersion(int protocolVersion)
        => protocolVersion switch
        {
            0 => new Protocol0(),
            _ => throw new NotSupportedException($"Protocol version {protocolVersion} is not supported.")
        };

    public static MinecraftProtocol FromVersion(string versionName)
        => versionName switch
        {
            "13w41b" => new Protocol0(),
            _ => throw new NotSupportedException($"Version '{versionName}' is not supported.")
        };

    public bool IsPacketSupported(int packetId, PacketContext packetContext) =>
        SupportedPackets.TryGetValue(packetContext, out var packets) &&
        packets.Forward.ContainsKey(packetId);

    public bool IsPacketSupported(Type packetDataType, PacketContext packetContext)
    {
        ArgumentNullException.ThrowIfNull(packetDataType);

        return SupportedPackets.TryGetValue(packetContext, out var packets) &&
            packets.Reverse.ContainsKey(packetDataType);
    }

    public bool IsPacketSupported<TData>(PacketContext packetContext)
        where TData : notnull
    {
        return IsPacketSupported(typeof(TData), packetContext);
    }

    public bool IsPacketSupported<TData>(MinecraftPacket<TData> packet)
        where TData : notnull
    {
        ArgumentNullException.ThrowIfNull(packet);

        return SupportedPackets.TryGetValue(packet.Context, out var packets) &&
            packets.Forward.ContainsKey(packet.Id) &&
            packets.Reverse.ContainsKey(typeof(TData));
    }

    public Type GetPacketDataType(int packetId, PacketContext packetContext)
    {
        try
        {
            return SupportedPackets[packetContext].Forward[packetId];
        }
        catch (KeyNotFoundException exception)
        {
            throw new NotSupportedException("The packet is not supported by the protocol.", exception);
        }
    }

    public int GetPacketId(Type packetDataType, PacketContext packetContext)
    {
        ArgumentNullException.ThrowIfNull(packetContext);

        try
        {
            return SupportedPackets[packetContext].Reverse[packetDataType];
        }
        catch (KeyNotFoundException exception)
        {
            throw new NotSupportedException("The packet is not supported by the protocol.", exception);
        }
    }

    public int GetPacketId<TData>(PacketContext packetContext)
        where TData : notnull
    {
        return GetPacketId(typeof(TData), packetContext);
    }

    public abstract ConnectionState GetNewState<TData>(MinecraftPacket<TData> lastPacket)
        where TData : notnull;
}
