using Minever.Networking.DataTypes;
using Minever.Networking.Packets;
using Minever.Networking.Serialization;
using Minever.Networking.Protocols;
using System.Buffers.Binary;

namespace Minever.Networking.IO;

public class MinecraftReader : BinaryReader, IDisposable
{
    public MinecraftReader(Stream baseStream) : base(baseStream) { }

    public override ushort ReadUInt16() => BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(2));

    public override uint ReadUInt32() => BinaryPrimitives.ReadUInt32BigEndian(ReadBytes(4));

    public override ulong ReadUInt64() => BinaryPrimitives.ReadUInt64BigEndian(ReadBytes(8));

    public override short ReadInt16() => BinaryPrimitives.ReadInt16BigEndian(ReadBytes(2));

    public override int ReadInt32() => BinaryPrimitives.ReadInt32BigEndian(ReadBytes(4));

    public override long ReadInt64() => BinaryPrimitives.ReadInt64BigEndian(ReadBytes(8));

    public override Half ReadHalf() => BinaryPrimitives.ReadHalfBigEndian(ReadBytes(2));

    public override float ReadSingle() => BinaryPrimitives.ReadSingleBigEndian(ReadBytes(4));

    public override double ReadDouble() => BinaryPrimitives.ReadDoubleBigEndian(ReadBytes(8));

    public Guid ReadUuid() => new Guid(ReadString());

    public BlockPosition ReadBlockPosition()
    {
        var x = ReadInt32();
        var y = ReadInt32();
        var z = ReadInt32();

        return new(x, y, z);
    }

    public Position ReadPosition()
    {
        var x = ReadDouble();
        var y = ReadDouble();
        var z = ReadDouble();

        return new(x, y, z);
    }

    /*private object ReadPacketData(int dataLength, Type targetType)
    {
        var packet = Activator.CreateInstance(targetType)!;

        var packetProperties = packet!
            .GetType()
            .GetProperties()
            .Where(propertyInfo => propertyInfo.GetCustomAttribute<PacketIgnoreAttribute>() is null)
            .Select(propertyInfo => new
            {
                Info = propertyInfo,
                Order = propertyInfo.GetCustomAttribute<PacketPropertyOrderAttribute>()?.Order ?? int.MaxValue,
                propertyInfo.GetCustomAttribute<PacketConverterAttribute>()?.ConverterType,
            })
            .OrderBy(property => property.Order);

        foreach (var property in packetProperties)
        {
            PacketConverter converter;

            if (property.ConverterType is null)
            {
                var propertyTypeConverterType = property.Info.PropertyType.GetCustomAttribute<PacketConverterAttribute>()?.ConverterType;

                if (propertyTypeConverterType is not null)
                    converter = (PacketConverter)Activator.CreateInstance(propertyTypeConverterType)!;
                else
                    converter = DefaultPacketConverter.Shared;
            }
            else
            {
                converter = (PacketConverter)Activator.CreateInstance(property.ConverterType)!;
            }

            var value = converter.Read(this, property.Info.PropertyType);

            property.Info.SetValue(packet, value);
        }

        return packet;
    }*/

    /*public MinecraftPacket<object> ReadPacket(int packetLength, PacketContext packetContext,
        MinecraftProtocol protocol, bool isCompressed = false)
    {
        if (packetLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(packetLength));

        ArgumentNullException.ThrowIfNull(protocol);

        int packetId;
        int dataLength;
        MinecraftReader dataReader;

        if (isCompressed)
        {
            var compressedLength = Read7BitEncodedInt();

            if (compressedLength == 0) // if shoud ignore compression
            {
                packetId = Read7BitEncodedInt();
                dataLength = packetLength - compressedLength.Get7BitEncodedInt32Bytes().Length - packetId.Get7BitEncodedInt32Bytes().Length;
                dataReader = this;
            }
            else
            {
                using var memoryStream = new MemoryStream();
                using var zLibStream = new ZLibStream(memoryStream, CompressionMode.Decompress);
                using var zLibReader = new MinecraftReader(zLibStream);

                packetId = zLibReader.Read7BitEncodedInt();
                dataLength = compressedLength;
                dataReader = zLibReader;
            }
        }
        else
        {
            packetId = Read7BitEncodedInt();
            dataLength = packetLength - packetId.Get7BitEncodedInt32Bytes().Length;
            dataReader = this;
        }

        if (!protocol.IsPacketSupported(packetId, packetContext))
            throw new NotSupportedPacketException(new(packetId, dataReader.ReadBytes(dataLength)), packetContext);

        var packetType = protocol.GetPacketDataType(packetId, packetContext);
        var packetData = dataReader.ReadPacketData(dataLength, packetType);

        if (dataReader != this)
            dataReader.Dispose();

        var packet = new MinecraftPacket<object>(packetId, packetData);

        return packet;
    }*/

    public MinecraftPacket<object> ReadPacket(int packetLength, PacketContext context, MinecraftProtocol protocol)
    {
        if (packetLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(packetLength));

        ArgumentNullException.ThrowIfNull(protocol);

        var packetBytes = ReadBytes(packetLength);

        return PacketSerializer.Deserialize(packetBytes, context, protocol);
    }
}
