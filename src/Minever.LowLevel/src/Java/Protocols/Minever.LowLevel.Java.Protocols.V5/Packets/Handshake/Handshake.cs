﻿using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using Minever.LowLevel.Core.Packets.Serialization.Converters;
using Minever.LowLevel.Java.Core;

namespace Minever.LowLevel.Java.Protocols.V5.Packets.Handshake;

/// <summary>
/// Id: 0x00 <br/>
/// State: <see cref="JavaConnectionState.Handshake"/> <br/>
/// Direction: <see cref="PacketDirection.ToServer"/> <br/>
/// <br/>
/// See <see href="https://wiki.vg/index.php?title=Protocol&amp;oldid=6003#Handshake">Handshake/Serverbound/Handshake</see> on wiki.vg.
/// </summary>
[JavaPacket<JavaProtocol5>(0x00, JavaConnectionState.Handshake, PacketDirection.ToServer)]
public sealed record Handshake
{
    //private const int MaxHostLength = 255; // todo: is it needed?

    private string _host = string.Empty;

    [PacketPropertyOrder(1)]
    [PacketConverter<PacketVarIntConverter>]
    public required int ProtocolVersion { get; init; }

    [PacketPropertyOrder(2)]
    public string Host
    {
        get => _host;
        init
        {
            ArgumentNullException.ThrowIfNull(value);

            //if (value.Length > MaxHostLength)
            //{
            //    throw new ArgumentOutOfRangeException(nameof(value), $"Server address length greater {MaxHostLength}.");
            //}

            _host = value;
        }
    }

    [PacketPropertyOrder(3)]
    public ushort Port { get; init; }

    [PacketPropertyOrder(4)]
    public required HandshakeNextConnectionState NextConnectionState { get; init; }
}
