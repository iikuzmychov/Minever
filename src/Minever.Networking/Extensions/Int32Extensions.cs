namespace Minever.Networking;

internal static class Int32Extensions
{
    internal static byte[] Get7BitEncodedInt32Bytes(this int value)
    {
        using var memoryStream = new MemoryStream();
        using var writer       = new BinaryWriter(memoryStream);

        writer.Write7BitEncodedInt(value);

        return memoryStream.ToArray();
    }
}
