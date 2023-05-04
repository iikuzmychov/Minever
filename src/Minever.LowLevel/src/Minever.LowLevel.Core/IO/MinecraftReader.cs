using System.Buffers.Binary;
using System.Text;

namespace Minever.LowLevel.Core.IO;

public class MinecraftReader : IDisposable
{
    private readonly BinaryReader _baseReader;

    public MinecraftReader(Stream baseStream)
    {
        _baseReader = new(baseStream, Encoding.UTF8);
    }

    public MinecraftReader(Stream baseStream, bool leaveOpen)
    {
        _baseReader = new(baseStream, Encoding.UTF8, leaveOpen);
    }

    public bool ReadBool() => _baseReader.ReadBoolean();

    public byte ReadByte() => _baseReader.ReadByte();

    public sbyte ReadSByte() => _baseReader.ReadSByte();

    public byte[] ReadBytes(int count) => _baseReader.ReadBytes(count);

    public int ReadVarInt() => _baseReader.Read7BitEncodedInt();

    public uint ReadVarUInt() => unchecked((uint)ReadVarInt());
    
    public long ReadVarLong() => _baseReader.Read7BitEncodedInt64();

    public ulong ReadVarULong() => unchecked((ulong)ReadVarLong());

    public ushort ReadUShort() => BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(2));

    public uint ReadUInt() => BinaryPrimitives.ReadUInt32BigEndian(ReadBytes(4));

    public ulong ReadULong() => BinaryPrimitives.ReadUInt64BigEndian(ReadBytes(8));

    public short ReadShort() => BinaryPrimitives.ReadInt16BigEndian(ReadBytes(2));

    public int ReadInt() => BinaryPrimitives.ReadInt32BigEndian(ReadBytes(4));

    public long ReadLong() => BinaryPrimitives.ReadInt64BigEndian(ReadBytes(8));

    public float ReadFloat() => BinaryPrimitives.ReadSingleBigEndian(ReadBytes(4));

    public double ReadDouble() => BinaryPrimitives.ReadDoubleBigEndian(ReadBytes(8));

    public string ReadString() => _baseReader.ReadString();

    public Guid ReadGuid() => new(_baseReader.ReadString());

    public void Dispose() => _baseReader.Dispose();
}
