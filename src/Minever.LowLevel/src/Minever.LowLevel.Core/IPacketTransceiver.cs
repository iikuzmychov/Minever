namespace Minever.LowLevel.Core;

public interface IPacketTransceiver
{
    public IProtocol Protocol { get; }
    public event Action<object, DateTime> PacketReceived;

    public void SendPacket(object packet);

    public Action<object, DateTime> OnPacket<TPacket>(Action<TPacket, DateTime> handler);
}
