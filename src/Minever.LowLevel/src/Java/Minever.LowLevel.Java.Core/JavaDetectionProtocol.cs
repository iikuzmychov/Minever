using Minever.LowLevel.Core;

namespace Minever.LowLevel.Java.Core;

internal class JavaDetectionProtocol : IProtocol
{
    public int Version => -1;

    public int GetPacketId(Type packetType) => throw new NotSupportedException();
}
