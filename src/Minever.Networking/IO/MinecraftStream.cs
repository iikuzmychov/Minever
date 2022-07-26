namespace Minever.Networking.IO;

public class MinecraftStream : Stream, IDisposable
{
    private readonly bool _leaveOpen;

    public Stream BaseStream { get; }
    public override bool CanRead => BaseStream.CanRead;
    public override bool CanSeek => BaseStream.CanSeek;
    public override bool CanWrite => BaseStream.CanWrite;
    public override long Length => BaseStream.Length;
    public override long Position
    {
        get => BaseStream.Position;
        set => BaseStream.Position = value;
    }

    public int TotalReadedBytesCount { get; private set; } = 0;

    public MinecraftStream(Stream baseStream, bool leaveOpen = true)
    {
        BaseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));
        _leaveOpen = leaveOpen;
    }

    public override void Flush() => BaseStream.Flush();

    public override int Read(byte[] buffer, int offset, int count)
    {
        TotalReadedBytesCount += count;

        return BaseStream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin) => BaseStream.Seek(offset, origin);

    public override void SetLength(long value) => BaseStream.SetLength(value);

    public override void Write(byte[] buffer, int offset, int count) => BaseStream.Write(buffer, offset, count);

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!_leaveOpen)
            BaseStream?.Dispose();
    }
}
