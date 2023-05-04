using System.Buffers.Binary;
using System.Net;
using System.Text;

namespace Minever.LowLevel.Core.IO;

public class MinecraftWriter : IAsyncDisposable, IDisposable
{
    private readonly BinaryWriter _baseWriter;

    public MinecraftWriter(Stream stream)
    {
        _baseWriter = new(stream, Encoding.UTF8);
    }

    public MinecraftWriter(Stream stream, bool leaveOpen)
    {
        _baseWriter = new(stream, Encoding.UTF8, leaveOpen);
    }

    public void Write(bool value) => _baseWriter.Write(value);

    public void Write(byte value) => _baseWriter.Write(value);

    public void Write(sbyte value) => _baseWriter.Write(value);

    public void Write(byte[] value) => _baseWriter.Write(value);

    public void WriteVarInt(int value) => _baseWriter.Write7BitEncodedInt(value);

    public void WriteVarUInt(uint value) => WriteVarInt(unchecked((int)value));

    public void WriteVarLong(long value) => _baseWriter.Write7BitEncodedInt64(value);

    public void WriteVarULong(ulong value) => WriteVarLong(unchecked((long)value));

    public void Write(ushort value)
        => _baseWriter.Write(BitConverter.IsLittleEndian ? BinaryPrimitives.ReverseEndianness(value) : value);

    public void Write(uint value)
        => _baseWriter.Write(BitConverter.IsLittleEndian ? BinaryPrimitives.ReverseEndianness(value) : value);

    public void Write(ulong value)
        => _baseWriter.Write(BitConverter.IsLittleEndian ? BinaryPrimitives.ReverseEndianness(value) : value);

    public void Write(short value) => _baseWriter.Write(IPAddress.HostToNetworkOrder(value));

    public void Write(int value) => _baseWriter.Write(IPAddress.HostToNetworkOrder(value));

    public void Write(long value) => _baseWriter.Write(IPAddress.HostToNetworkOrder(value));

    public void Write(float value)
    {
        var bytes = BitConverter.GetBytes(value);

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }

        _baseWriter.Write(bytes);
    }

    public void Write(double value)
    {
        var bytes = BitConverter.GetBytes(value);

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }

        _baseWriter.Write(bytes);
    }

    public void Write(string value) => _baseWriter.Write(value);

    public void Write(Guid guid) => Write(guid.ToString());

    public ValueTask DisposeAsync() => _baseWriter.DisposeAsync();

    public void Dispose() => _baseWriter.Dispose();
}
