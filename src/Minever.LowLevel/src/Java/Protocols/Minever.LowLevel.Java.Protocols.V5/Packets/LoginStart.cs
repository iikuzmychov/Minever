using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using Minever.LowLevel.Java.Core;

namespace Minever.LowLevel.Java.Protocols.V5.Packets;

/// <summary>
/// Id: 0x00 <br/>
/// State: <see cref="JavaConnectionState.Login"/> <br/>
/// Direction: <see cref="PacketDirection.ToServer"/> <br/>
/// <br/>
/// See <see href="https://wiki.vg/index.php?title=Protocol&amp;oldid=6003#Login_Start">Login/Serverbound/LoginStart</see> on wiki.vg.
/// </summary>
public sealed record LoginStart
{
    private readonly string _name = string.Empty;

    [PacketPropertyOrder(1)]
    public string Name
    {
        get => _name;
        init => _name = value ?? throw new ArgumentNullException(nameof(value));
    }
}
