using Minever.Core;

namespace Minever.Java.Core;

internal class JavaDetectionProtocol : IProtocol
{
    public int Version => -1;

    public int GetPacketId(Type packetType) => throw new NotSupportedException();
}
