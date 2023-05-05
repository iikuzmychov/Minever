// todo: namespace

public interface IPacketClient : IDisposable, IAsyncDisposable
{
    public event Action? Disconnected;
    public event Action<object>? PacketReceived;

    //public abstract static Task<(TimeSpan Ping, ... Status)> PingServerAsync();

    public ValueTask ConnectAsync(string host, ushort port, CancellationToken cancellationToken = default);
    
    public Action<object> OnPacket<TData>(Action<TData> action) where TData : notnull;
    
    public void OnceOnPacket<TData>(Action<TData> action) where TData : notnull;
    
    public Task<TData> WaitPacketAsync<TData>(CancellationToken cancellationToken = default) where TData : notnull;
    
    public void SendPacket(object packetData);

    public Task<TResponseData> SendRequestAsync<TResponseData>(
        object requestPacketData, CancellationToken cancellationToken = default)
        where TResponseData : notnull;
}