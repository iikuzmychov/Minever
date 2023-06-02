using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using Minever.LowLevel.Java.Core;

namespace Minever.LowLevel.Java.Protocols.V5.Packets;

/// <summary>
/// Id: 0x01 <br/>
/// State: <see cref="JavaConnectionState.Login"/> <br/>
/// Direction: <see cref="PacketDirection.FromServer"/> <br/>
/// <br/>
/// See <see href="https://wiki.vg/index.php?title=Protocol&amp;oldid=6003#Login_Success">Login/Clientbound/LoginSuccess</see> on wiki.vg.
/// </summary>
public sealed record LoginSuccess
{
    private string _name = default!;

    [PacketPropertyOrder(1)]
    public required Guid Uuid { get; init; }

    [PacketPropertyOrder(2)]
    public required string Name
    {
        get => _name;
        init
        {
            ArgumentException.ThrowIfNullOrEmpty(value);

            _name = value;
        }
    }
}
