using Microsoft.Extensions.Logging;
using Minever.Client;
using Minever.Client.ConsoleApplication;
using Minever.Networking.DataTypes.Text;
using Minever.Networking.Packets;
using Minever.Networking.Protocols;

const string ServerAddress = "localhost";
const ushort ServerPort    = 50477;

ConcurrentConsole.ForegroundColor = ConsoleColor.Magenta;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Debug)
        .AddConsole();
});

var protocol       = new Protocol0();
var requestTimeout = TimeSpan.FromSeconds(15);

await using var client = new MinecraftPacketClient(protocol);
client.OnPacket<KeepAlive>(packet => client.SendPacket(new KeepAlive(packet.Data.Id)));
client.OnPacket<JoinGame>(packet => ConcurrentConsole.WriteLine($"Max players count: {packet.Data.MaxPlayersCount}."));
client.OnPacket<SpawnPosition>(packet => ConcurrentConsole.WriteLine($"Spawn position: {packet.Data.Position}."));
client.OnPacket<Respawn>(packet =>
{
    client.SendPacket(ClientStatus.PerformRespawn);
    ConcurrentConsole.WriteLine("Respawn.");
});
client.OnPacket<PlayerAbilities>(packet => ConcurrentConsole.WriteLine($"Flying speed: {packet.Data.FlyingSpeed}, walking speed: {packet.Data.WalkingSpeed}."));
client.OnPacket<PlayerPositionAndLook>(packet =>
{
    client.SendPacket(new PlayerPositionAndLookWithStance(packet.Data, 1d));
    client.SendPacket(ClientStatus.PerformRespawn);
    ConcurrentConsole.WriteLine($"Player position: {packet.Data.Position}");
});
client.OnPacket<HeldItemChange>(packet => ConcurrentConsole.WriteLine("HeldItemChange."));
//client.OnPacket<TimeUpdate>(packet => ConcurrentConsole.WriteLine($"Age of world: {packet.Data.WorldAge}, time of day: {packet.Data.DayTime}."));
client.OnPacket<PlayerListItem>(packet => ConcurrentConsole.WriteLine($"Player: {packet.Data.PlayerName}, is connected: {packet.Data.IsConnected}, ping: {packet.Data.Ping}."));
client.OnPacket<UpdateHealth>(packet =>
{
    if (packet.Data.Health < 0)
        client.SendPacket(ClientStatus.PerformRespawn);

    ConcurrentConsole.WriteLine($"Health: {packet.Data.Health}, food: {packet.Data.Food}, food saturation: {packet.Data.FoodSaturation}.");
});
client.OnPacket<PluginMessage>(packet => ConcurrentConsole.WriteLine($"Plugin message from channel '{packet.Data.ChannelName}'"));
client.OnPacket<Statistics>(packet => ConcurrentConsole.WriteLine($"Statistics ({packet.Data.Entries.Length} entries)."));
//client.OnPacket<CollectItem>(packet => ConcurrentConsole.WriteLine($"{packet.Data.CollectorEntityId} collects {packet.Data.CollectedEntityId}."));
//client.OnPacket<DestroyEntities>(packet => ConcurrentConsole.WriteLine($"{packet.Data.EntityIds.Length} entities destroyed."));
client.OnPacket<Entity>(packet => ConcurrentConsole.WriteLine($"Entity {packet.Data.EntityId}."));
//client.OnPacket<EntityRelativeMove>(packet => ConcurrentConsole.WriteLine($"Entity {packet.Data.EntityId} moves ({packet.Data.DeltaX:+#;-#;0}; {packet.Data.DeltaY:+#;-#;0}; {packet.Data.DeltaZ:+#;-#;0})."));
//client.OnPacket<EntityLook>(packet => ConcurrentConsole.WriteLine($"Entity {packet.Data.EntityId} look changed ({packet.Data.Pitch}; {packet.Data.Yaw})."));
//client.OnPacket<EntityLookAndRelativeMove>(packet => ConcurrentConsole.WriteLine($"Entity {packet.Data.EntityId} look ({packet.Data.Pitch}; {packet.Data.Yaw}) and position ({packet.Data.DeltaX:+#;-#;0}; {packet.Data.DeltaY:+#;-#;0}; {packet.Data.DeltaZ:+#;-#;0}) changed."));
//client.OnPacket<EntityTeleport>(packet => ConcurrentConsole.WriteLine($"Entity {packet.Data.EntityId} teleported: look ({packet.Data.Pitch}; {packet.Data.Yaw}), position ({packet.Data.X}; {packet.Data.Y}; {packet.Data.Z})."));
//client.OnPacket<EntityHeadLook>(packet => ConcurrentConsole.WriteLine($"Entity {packet.Data.EntityId} head look changed ({packet.Data.HeadYaw})."));
client.OnPacket<EntityEffect>(packet => ConcurrentConsole.WriteLine($"Effect {packet.Data.EffectId} on entity {packet.Data.EntityId} for {packet.Data.Duration}."));
client.OnPacket<ChatMessageFromServer>(packet => ConcurrentConsole.WriteLine($"Chat: {packet.Data.Text}", ConcurrentConsole.BackgroundColor, ConsoleColor.Cyan));
client.OnPacket<Disconnect>(packet => ConcurrentConsole.WriteLine($"Disconneted. Reason: {packet.Data.Reason}."));

await client.ConnectAsync(ServerAddress, ServerPort).WaitAsync(requestTimeout);

var handshake = new Handshake(client.Protocol.Version, ServerAddress, ServerPort, HandshakeNextState.Login);
client.SendPacket(handshake);

var loginSuccess = (await client.SendRequestAsync<LoginSuccess>(new LoginStart("KuzCode23")).WaitAsync(requestTimeout)).Data;
ConcurrentConsole.WriteLine($"Login success! {loginSuccess.Name} ({loginSuccess.Uuid}).");

await Task.Delay(TimeSpan.FromSeconds(1));
client.SendPacket(new ChatMessageFromClient(@"раз"));
await Task.Delay(TimeSpan.FromSeconds(1));
client.SendPacket(new ChatMessageFromClient(@"два"));
await Task.Delay(TimeSpan.FromSeconds(1));
client.SendPacket(new ChatMessageFromClient(@"три"));
await Task.Delay(TimeSpan.FromSeconds(1));
client.SendPacket(new ChatMessageFromClient(@"/setblock ~0 ~5 ~0 minecraft:grass"));

Console.ReadKey(true);