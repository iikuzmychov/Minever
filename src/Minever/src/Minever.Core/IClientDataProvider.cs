using Minever.LowLevel.Core;

namespace Minever.Core;

public interface IClientDataProvider
{
    public bool Connected { get; }
    public IPacketTransceiver PacketTransceiver { get; }
    public IControllerProvider Controllers { get; }
}