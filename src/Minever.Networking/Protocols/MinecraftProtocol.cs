using BidirectionalMap;
using Minever.Networking.Packets;

namespace Minever.Networking.Protocols;

public abstract partial class MinecraftProtocol
{
    protected Dictionary<MinecraftPacketKind, BiMap<int, Type>> SupportedPackets;

    public abstract int Version { get; }

    public MinecraftProtocol(Dictionary<MinecraftPacketKind, BiMap<int, Type>> supportedPackets)
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

    public bool IsPacketSupported(int packetId, MinecraftPacketKind packetKind) =>
        SupportedPackets.TryGetValue(packetKind, out var packets) &&
        packets.Forward.ContainsKey(packetId);

    public bool IsPacketSupported(Type packetDataType, MinecraftPacketKind packetKind)
    {
        ArgumentNullException.ThrowIfNull(packetDataType);

        return SupportedPackets.TryGetValue(packetKind, out var packets) &&
            packets.Reverse.ContainsKey(packetDataType);
    }

    public bool IsPacketSupported<TData>(MinecraftPacketKind packetKind)
        where TData : notnull
    {
        return IsPacketSupported(typeof(TData), packetKind);
    }

    public bool IsPacketSupported<TData>(MinecraftPacket<TData> packet)
        where TData : notnull
    {
        ArgumentNullException.ThrowIfNull(packet);

        return SupportedPackets.TryGetValue(packet.Kind, out var packets) &&
            packets.Forward.ContainsKey(packet.Id) &&
            packets.Reverse.ContainsKey(typeof(TData));
    }

    public Type GetPacketDataType(int packetId, MinecraftPacketKind packetKind)
    {
        try
        {
            return SupportedPackets[packetKind].Forward[packetId];
        }
        catch (KeyNotFoundException exception)
        {
            throw new NotSupportedException("The packet is not supported by the protocol version.", exception);
        }
    }

    public int GetPacketId(Type packetDataType, MinecraftPacketKind packetKind)
    {
        ArgumentNullException.ThrowIfNull(packetKind);

        try
        {
            return SupportedPackets[packetKind].Reverse[packetDataType];
        }
        catch (KeyNotFoundException exception)
        {
            throw new NotSupportedException("The packet is not supported by the protocol version.", exception);
        }
    }

    public int GetPacketId<TData>(MinecraftPacketKind packetKind)
        where TData : notnull
    {
        return GetPacketId(typeof(TData), packetKind);
    }

    public abstract MinecraftConnectionState GetNewState<TData>(MinecraftPacket<TData> lastPacket)
        where TData : notnull;
}
