﻿using BidirectionalMap;
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

    public bool IsPacketSupported(int packetId, PacketContext context) =>
        SupportedPackets.TryGetValue(context, out var packets) &&
        packets.Forward.ContainsKey(packetId);

    public bool IsPacketSupported(Type packetDataType, PacketContext context)
    {
        ArgumentNullException.ThrowIfNull(packetDataType);

        return SupportedPackets.TryGetValue(context, out var packets) &&
            packets.Reverse.ContainsKey(packetDataType);
    }

    public bool IsPacketSupported<TData>(PacketContext context)
        where TData : notnull
    {
        return IsPacketSupported(typeof(TData), context);
    }

    public bool IsPacketSupported<TData>(MinecraftPacket<TData> packet, PacketContext context)
        where TData : notnull
    {
        ArgumentNullException.ThrowIfNull(packet);

        return SupportedPackets.TryGetValue(context, out var packets) &&
            packets.Forward.ContainsKey(packet.Id) &&
            packets.Reverse.ContainsKey(typeof(TData));
    }

    public Type GetPacketDataType(int packetId, PacketContext context)
    {
        try
        {
            return SupportedPackets[context].Forward[packetId];
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
            return SupportedPackets[context].Reverse[packetDataType];
        }
        catch (KeyNotFoundException exception)
        {
            throw new NotSupportedException("The packet is not supported by the protocol.", exception);
        }
    }

    public int GetPacketId<TData>(PacketContext context)
        where TData : notnull
    {
        return GetPacketId(typeof(TData), context);
    }

    public abstract ConnectionState GetNewState<TData>(MinecraftPacket<TData> lastPacket, PacketContext context)
        where TData : notnull;
}
