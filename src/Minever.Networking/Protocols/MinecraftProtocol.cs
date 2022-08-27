using Minever.Networking.Packets;

namespace Minever.Networking.Protocols;

public abstract partial class MinecraftProtocol
{
    protected Dictionary<PacketContext, BidirectionalDictionary<int, Type>> SupportedPackets { get; }

    public abstract int Version { get; }

    public MinecraftProtocol(Dictionary<PacketContext, BidirectionalDictionary<int, Type>> supportedPackets)
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

    public bool IsPacketSupported(int packetId, PacketContext context) =>
        SupportedPackets.TryGetValue(context, out var packets) &&
        packets.ContainsKey(packetId);

    public bool IsPacketSupported(Type packetDataType, PacketContext context)
    {
        ArgumentNullException.ThrowIfNull(packetDataType);

        return SupportedPackets.TryGetValue(context, out var packets) &&
            packets.ContainsValue(packetDataType);
    }

    public Type GetPacketDataType(int packetId, PacketContext context)
    {
        try
        {
            return SupportedPackets[context][packetId];
        }
        catch (KeyNotFoundException exception)
        {
            throw new NotSupportedException("The packet is not supported by the protocol.", exception);
        }
    }

    public int GetPacketId(Type packetDataType, PacketContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            return SupportedPackets[context].Inverse[packetDataType];
        }
        catch (KeyNotFoundException exception)
        {
            throw new NotSupportedException("The packet is not supported by the protocol.", exception);
        }
    }

    public abstract ConnectionState GetNewState<TData>(TData lastPacketData, PacketContext context);
}
