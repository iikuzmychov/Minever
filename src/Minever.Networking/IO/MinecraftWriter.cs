using Minever.Networking.DataTypes;
using Minever.Networking.Packets;
using Minever.Networking.Serialization;
using System.Net;

namespace Minever.Networking.IO;

public class MinecraftWriter : BinaryWriter, IAsyncDisposable, IDisposable
{
    public MinecraftWriter(Stream stream) : base(stream) { }

    public override void Write(ushort value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);

        Write(bytes);
    }

    public override void Write(uint value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);

        Write(bytes);
    }

    public override void Write(ulong value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);

        Write(bytes);
    }

    public override void Write(short value) => base.Write(IPAddress.HostToNetworkOrder(value));

    public override void Write(int value) => base.Write(IPAddress.HostToNetworkOrder(value));

    public override void Write(long value) => base.Write(IPAddress.HostToNetworkOrder(value));

    public override void Write(Half value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);

        Write(bytes);
    }

    public override void Write(float value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);

        Write(bytes);
    }

    public override void Write(double value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);

        Write(bytes);
    }

    public void Write(Guid uuid) => Write(uuid.ToString());

    public void Write(BlockPosition position)
    {
        Write(position.X);
        Write(position.Y);
        Write(position.Z);
    }

    public void Write(Position position)
    {
        Write(position.X);
        Write(position.Y);
        Write(position.Z);
    }

    /*private static byte[] GetPacketDataBytes<TData>(TData packetData)
    {
        var packetProperties = packetData!
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

        using var memoryStream = new MemoryStream();
        using var writer = new MinecraftWriter(memoryStream);

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

            var propertyValue = property.Info.GetValue(packetData)!;

            converter.Write(propertyValue, writer);
        }

        return memoryStream.ToArray();
    }*/

    /*public void WritePacket(int packetId, object packetData)
    {
        ArgumentNullException.ThrowIfNull(packetData);

        var idBytes = packetId.Get7BitEncodedInt32Bytes();
        var dataBytes = GetPacketDataBytes(packetData);
        var packetLength = idBytes.Length + dataBytes.Length;

        Write7BitEncodedInt(packetLength);
        Write(idBytes);
        Write(dataBytes);
    }*/

    /*public void WritePacket(int packetId, object packetData, int compressingThreshold)
    {
        ArgumentNullException.ThrowIfNull(packetData);

        var idBytes = packetId.Get7BitEncodedInt32Bytes();
        var dataBytes = GetPacketDataBytes(packetData);
        byte[] compressedBytes;

        using (var memoryStream = new MemoryStream())
        using (var zLibStream = new ZLibStream(memoryStream, CompressionMode.Compress))
        using (var zLibWriter = new BinaryWriter(zLibStream))
        {
            zLibWriter.Write(idBytes);
            zLibWriter.Write(dataBytes);

            compressedBytes = memoryStream.ToArray();
        }

        var shouldIgnoreCompression = compressedBytes.Length < compressingThreshold;
        var compressedLength = shouldIgnoreCompression ? 0 : compressedBytes.Length;
        var comressedLengthBytes = compressedLength.Get7BitEncodedInt32Bytes();
        int packetLength;

        if (shouldIgnoreCompression)
            packetLength = comressedLengthBytes.Length + compressedBytes.Length;
        else
            packetLength = comressedLengthBytes.Length + idBytes.Length + dataBytes.Length;

        Write7BitEncodedInt(packetLength);
        Write(comressedLengthBytes);

        if (shouldIgnoreCompression)
        {
            Write(idBytes);
            Write(dataBytes);
        }
        else
        {
            Write(compressedBytes);
        }
    }*/

    public void WritePacket<TData>(MinecraftPacket<TData> packet)
        where TData : notnull
    {
        ArgumentNullException.ThrowIfNull(packet);

        var packetBytes = PacketSerializer.Serialize(packet);

        Write7BitEncodedInt(packetBytes.Length);
        Write(packetBytes);
    }
}
