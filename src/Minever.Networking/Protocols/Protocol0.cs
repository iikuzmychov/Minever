using BidirectionalMap;
using Minever.Networking.Packets;

namespace Minever.Networking.Protocols;

public class Protocol0 : MinecraftProtocol
{
    private readonly static Dictionary<PacketContext, BiMap<int, Type>> s_supportedPackets;

    public override int Version => 0;

    static Protocol0()
    {
        s_supportedPackets = new()
        {
            // Handshake
            {
                new(PacketDirection.ClientToServer, ConnectionState.Handshake),
                new()
                {
                    { 0x00, typeof(Handshake) },
                }
            },

            // Status
            {
                new(PacketDirection.ClientToServer, ConnectionState.Status),
                new()
                {
                    { 0x00, typeof(ServerStatusRequest) },
                    { 0x01, typeof(Ping) },
                }
            },

            {
                new(PacketDirection.ServerToClient, ConnectionState.Status),
                new()
                {
                    { 0x00, typeof(ServerStatusResponse) },
                    { 0x01, typeof(Ping) },
                }
            },

            // Login
            {
                new(PacketDirection.ClientToServer, ConnectionState.Login),
                new()
                {
                    { 0x00, typeof(LoginStart) },
                    //{ 0x01, typeof(EncryptionRequest) },
                }
            },

            {
                new(PacketDirection.ServerToClient, ConnectionState.Login),
                new()
                {
                    { 0x02, typeof(LoginSuccess) },
                    //{ 0x01, typeof(EncryptionResponse) },
                }
            },

            // Play
            {
                new(PacketDirection.ClientToServer, ConnectionState.Play),
                new()
                {
                    { 0x00, typeof(KeepAlive) },
                    
                    { 0x06, typeof(PlayerPositionAndLookWithStance) },
                    
                    { 0x16, typeof(ClientStatus) },
                    { 0x17, typeof(PluginMessage) },
                }
            },

            {
                new(PacketDirection.ServerToClient, ConnectionState.Play),
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
                    
                    { 0x13, typeof(DestroyEntities) },
                    { 0x14, typeof(Entity) },
                    
                    { 0x0D, typeof(CollectItem) },

                    { 0x37, typeof(Statistics) },
                    { 0x38, typeof(PlayerListItem) },
                    { 0x39, typeof(PlayerAbilities) },

                    { 0x3F, typeof(PluginMessage) },
                }
            },
        };
    }

    public Protocol0() : base(s_supportedPackets) { }

    public override ConnectionState GetNewState<TData>(TData lastPacketData, PacketContext context)
    {
        ArgumentNullException.ThrowIfNull(lastPacketData);
        
        if (IsPacketSupported(typeof(TData), context))
            throw new NotSupportedException("The packet is not supported by the protocol.");

        return (context.ConnectionState, lastPacketData) switch
        {
            (ConnectionState.Handshake, Handshake handshake) => handshake.NextState.ToConnectionState(),
            (ConnectionState.Login, LoginSuccess)            => ConnectionState.Play,
            _ => context.ConnectionState,
        };
    }
}
