using Minever.Core;

namespace Minever.Java.Core;

public interface IJavaProtocol : IProtocol
{
    public int GetPacketId(Type type, JavaPacketContext context);
    public Type GetPacketType(int id, JavaPacketContext context);
    public bool IsPacketSupported(object packet, JavaPacketContext context);
    public JavaConnectionState GetNextConnectionState(object packet, JavaPacketContext context);
}