namespace Minever.Networking;

internal static class Int64Extensions
{
    internal static byte[] Get7BitEncodedInt64Bytes(this long value)
    {
        using var memoryStream = new MemoryStream();
        using var writer       = new BinaryWriter(memoryStream);

        writer.Write7BitEncodedInt64(value);

        return memoryStream.ToArray();
    }
}
