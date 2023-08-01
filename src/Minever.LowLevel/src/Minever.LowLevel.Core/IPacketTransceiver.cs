namespace Minever.LowLevel.Core;

public interface IPacketTransceiver
{
    public IProtocol Protocol { get; }
    public event Action<object> PacketReceived;

    public void SendPacket(object packet);

    public Action<object> OnPacket<TPacket>(Action<TPacket> handler);
}
