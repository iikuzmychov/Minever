﻿using Minever.Networking.Packets;

namespace Minever.Networking.Protocols;

public class JavaProtocol0 : JavaProtocol
{
    private readonly static Dictionary<PacketContext, BidirectionalDictionary<int, Type>> s_supportedPackets;

    public override int Version => 0;

    static JavaProtocol0()
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
                    { 0x00, typeof(ServerStatus) },
                    { 0x01, typeof(Ping) },
                }
            },

            // Login
            {
                new(PacketDirection.ClientToServer, ConnectionState.Login),
                new()
                {
                    { 0x00, typeof(LoginStart) },
                    { 0x01, typeof(EncryptionRequest) },
                }
            },

            {
                new(PacketDirection.ServerToClient, ConnectionState.Login),
                new()
                {
                    { 0x00, typeof(Disconnect) },
                    { 0x01, typeof(EncryptionResponse) },
                    { 0x02, typeof(LoginSuccess) },
                }
            },

            // Play
            {
                new(PacketDirection.ClientToServer, ConnectionState.Play),
                new()
                {
                    { 0x00, typeof(KeepAlive) },
                    { 0x01, typeof(ClientToServerChatMessage) },
                    { 0x02, typeof(UseEntity) },
                    
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
                    { 0x02, typeof(ServerToClientChatMessage) },
                    { 0x03, typeof(TimeUpdate) },

                    { 0x05, typeof(SpawnPosition) },
                    { 0x06, typeof(UpdateHealth) },
                    { 0x07, typeof(Respawn) },
                    { 0x08, typeof(PlayerPositionAndLook) },
                    { 0x09, typeof(HeldItemChange) },

                    { 0x0D, typeof(CollectItem) },

                    { 0x11, typeof(SpawnExperienceOrb) },

                    { 0x13, typeof(DestroyEntities) },
                    { 0x14, typeof(Entity) },
                    { 0x15, typeof(EntityRelativeMove) },
                    { 0x16, typeof(EntityLook) },
                    { 0x17, typeof(EntityLookAndRelativeMove) },
                    { 0x18, typeof(EntityTeleport) },
                    { 0x19, typeof(EntityHeadLook) },
                    
                    { 0x1D, typeof(EntityEffect) },
                    { 0x1E, typeof(RemoveEntityEffect) },
                    { 0x1F, typeof(SetExperience) },

                    { 0x37, typeof(Statistics) },
                    { 0x38, typeof(PlayerListItem) },
                    { 0x39, typeof(PlayerAbilities) },

                    { 0x3F, typeof(PluginMessage) },
                    { 0x40, typeof(Disconnect) },
                }
            },
        };
    }

    public JavaProtocol0() : base(s_supportedPackets) { }

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
