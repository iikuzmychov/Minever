using BidirectionalMap;
using Minever.Networking.Packets;

namespace Minever.Networking.Protocols;

public class Protocol0 : MinecraftProtocol
{

    private readonly static Dictionary<MinecraftPacketKind, BiMap<int, Type>> s_supportedPackets;

    public override int Version => 0;

    static Protocol0()
    {
        s_supportedPackets = new()
        {
            // Handshake
            {
                new(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Handshake),
                new()
                {
                    { 0x00, typeof(Handshake) },
                }
            },

            // Status
            {
                new(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Status),
                new()
                {
                    { 0x00, typeof(ServerStatusRequest) },
                    { 0x01, typeof(Ping) },
                }
            },

            {
                new(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Status),
                new()
                {
                    { 0x00, typeof(ServerStatusResponse) },
                    { 0x01, typeof(Ping) },
                }
            },

            // Login
            {
                new(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Login),
                new()
                {
                    { 0x00, typeof(LoginStart) },
                    //{ 0x01, typeof(EncryptionRequest) },
                }
            },

            {
                new(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Login),
                new()
                {
                    { 0x02, typeof(LoginSuccess) },
                    //{ 0x01, typeof(EncryptionResponse) },
                }
            },

            // Play
            {
                new(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play),
                new()
                {
                    { 0x00, typeof(KeepAlive) },
                    
                    { 0x06, typeof(PlayerPositionAndLookWithStance) },
                    
                    { 0x16, typeof(ClientStatus) },
                    { 0x17, typeof(PluginMessage) },
                }
            },

            {
                new(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play),
                new()
                {
                    { 0x00, typeof(KeepAlive) },
                    { 0x01, typeof(JoinGame) },
                    { 0x03, typeof(TimeUpdate) },

                    { 0x05, typeof(SpawnPosition) },
                    { 0x06, typeof(UpdateHealth) },                    
                    { 0x07, typeof(Respawn) },
                    { 0x08, typeof(PlayerPositionAndLook) },
                    { 0x09, typeof(HeldItemChange) },

                    { 0x38, typeof(PlayerListItem) },
                    { 0x39, typeof(PlayerAbilities) },

                    { 0x3F, typeof(PluginMessage) },
                }
            },
        };
    }

    public Protocol0() : base(s_supportedPackets) { }

    public override MinecraftConnectionState GetNewState<TData>(MinecraftPacket<TData> lastPacket)
    {
        ArgumentNullException.ThrowIfNull(lastPacket);
        
        if (IsPacketSupported(lastPacket))
            throw new NotSupportedException();

        return (lastPacket.Kind.ConnectionState, lastPacket.Data) switch
        {
            (MinecraftConnectionState.Handshake, Handshake handshake) => handshake.NextState.ToConnectionState(),
            (MinecraftConnectionState.Login, LoginSuccess)            => MinecraftConnectionState.Play,
            _ => lastPacket.Kind.ConnectionState,
        };
    }
}
