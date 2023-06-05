using Minever.LowLevel.Core;

namespace Minever.LowLevel.Java.Core;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class JavaPacketAttribute<TProtocol> : Attribute
    where TProtocol : IProtocol
{
    public int Id { get; }
    public JavaPacketContext Context { get; }

    public JavaPacketAttribute(int id, JavaConnectionState connectionState, PacketDirection direction)
    {
        Id      = id;
        Context = new JavaPacketContext(connectionState, direction);
    }
}
