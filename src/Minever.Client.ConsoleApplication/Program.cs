using Microsoft.Extensions.Logging;
using Minever.Client;
using Minever.Client.ConsoleApplication;
using Minever.Networking.Packets;
using Minever.Networking.Protocols;
using System.Diagnostics;

const string Hostname = "localhost";
const ushort Port = 51090;

ConcurrentConsole.ForegroundColor = ConsoleColor.Magenta;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Information)
        .AddConsole();
});

//var (status, ping) = await MinecraftClient.PingServerAsync("play.paradise-city.ir", 25565, loggerFactory);
var (status, ping) = await MinecraftClient.PingServerAsync("mc.musteryworld.ru", 25565, loggerFactory);
Debugger.Break();

var protocol = new JavaProtocol0();
var timeout = TimeSpan.FromSeconds(15);

await using var client = new JavaPacketClient(protocol, loggerFactory);
client.OnPacket<KeepAlive>(keepAlive => client.SendPacket(keepAlive));
client.OnPacket<JoinGame>(joinGame => ConcurrentConsole.WriteLine($"Max players count: {joinGame.MaxPlayersCount}."));
client.OnPacket<SpawnPosition>(position => ConcurrentConsole.WriteLine($"Spawn position: {position.BlockPosition}."));
client.OnPacket<Respawn>(_ =>
{
    ConcurrentConsole.WriteLine("Respawn.");
    client.SendPacket(ClientStatus.PerformRespawn);
});
client.OnPacket<PlayerAbilities>(abilities => ConcurrentConsole.WriteLine($"Flying speed: {abilities.FlyingSpeed}, walking speed: {abilities.WalkingSpeed}."));
client.OnPacket<PlayerPositionAndLook>(positionAndLook =>
{
    ConcurrentConsole.WriteLine($"Player position: {positionAndLook.Position}");
    client.SendPacket(new PlayerPositionAndLookWithStance(positionAndLook, 1.65d));
    client.SendPacket(ClientStatus.PerformRespawn);
});
client.OnPacket<HeldItemChange>(_ => ConcurrentConsole.WriteLine("HeldItemChange."));
//client.OnPacket<TimeUpdate>(data => ConcurrentConsole.WriteLine($"Age of world: {data.WorldAge}, time of day: {data.DayTime}."));
//client.OnPacket<PlayerListItem>(data => ConcurrentConsole.WriteLine($"Player: {data.PlayerName}, is connected: {data.IsConnected}, ping: {data.Ping}."));
client.OnPacket<UpdateHealth>(healthAndFood =>
{
    if (healthAndFood.Health < 0)
        client.SendPacket(ClientStatus.PerformRespawn);

    ConcurrentConsole.WriteLine($"Health: {healthAndFood.Health}, food: {healthAndFood.Food}, food saturation: {healthAndFood.FoodSaturation}.");
});
client.OnPacket<PluginMessage>(message => ConcurrentConsole.WriteLine($"Plugin message from channel '{message.ChannelName}'"));
client.OnPacket<Statistics>(statistics => ConcurrentConsole.WriteLine($"Statistics ({statistics.Entries.Length} entries)."));
//client.OnPacket<CollectItem>(data => ConcurrentConsole.WriteLine($"{data.CollectorEntityId} collects {data.CollectedEntityId}."));
//client.OnPacket<DestroyEntities>(data => ConcurrentConsole.WriteLine($"{data.EntityIds.Length} entities destroyed."));
client.OnPacket<Entity>(entity => ConcurrentConsole.WriteLine($"Entity {entity.Id}."));
//client.OnPacket<EntityRelativeMove>(data => ConcurrentConsole.WriteLine($"Entity {data.EntityId} moves ({data.DeltaX:+#;-#;0}; {data.DeltaY:+#;-#;0}; {data.DeltaZ:+#;-#;0})."));
//client.OnPacket<EntityLook>(data => ConcurrentConsole.WriteLine($"Entity {data.EntityId} look changed ({data.Pitch}; {data.Yaw})."));
//client.OnPacket<EntityLookAndRelativeMove>(data => ConcurrentConsole.WriteLine($"Entity {data.EntityId} look ({data.Pitch}; {data.Yaw}) and position ({data.DeltaX:+#;-#;0}; {data.DeltaY:+#;-#;0}; {data.DeltaZ:+#;-#;0}) changed."));
//client.OnPacket<EntityTeleport>(data => ConcurrentConsole.WriteLine($"Entity {data.EntityId} teleported: look ({data.Pitch}; {data.Yaw}), position ({data.X}; {data.Y}; {data.Z})."));
//client.OnPacket<EntityHeadLook>(data => ConcurrentConsole.WriteLine($"Entity {data.EntityId} head look changed ({data.HeadYaw})."));
//client.OnPacket<EntityEffect>(data => ConcurrentConsole.WriteLine($"Effect {data.EffectId} on entity {data.EntityId} for {data.Duration}."));
//client.OnPacket<RemoveEntityEffect>(data => ConcurrentConsole.WriteLine($"Effect {data.EffectId} on entity {data.EntityId} removed."));
client.OnPacket<ServerToClientChatMessage>(message => ConcurrentConsole.WriteLine($"Chat: {message.Text}", ConcurrentConsole.BackgroundColor, ConsoleColor.Cyan));
client.OnPacket<SetExperience>(experience => ConcurrentConsole.WriteLine($"Experience updated: level: {experience.Level}, total: {experience.TotalAmount}, bar value: {experience.BarValue}."));
client.OnPacket<SpawnExperienceOrb>(orb =>
{
    ConcurrentConsole.WriteLine($"Experience orb spawned: id: {orb.EntityId}, experience amount: {orb.ExperienceAmount}.");
    //client.SendPacket(new UseEntity(data.EntityId, UseEntityAction.RigthClick));
});
client.OnPacket<Disconnect>(disconnectInfo => ConcurrentConsole.WriteLine($"Disconneted. Reason: {disconnectInfo.Reason}."));

await client.ConnectAsync(Hostname, Port, new CancellationTokenSource(timeout).Token);

var handshake = new Handshake(client.Protocol.Version, Hostname, Port, HandshakeNextState.Login);
client.SendPacket(handshake);

var loginRequest = await client.SendRequestAsync<LoginSuccess>(new LoginStart("KuzCode23"), new CancellationTokenSource(timeout).Token);
ConcurrentConsole.WriteLine($"Login success! {loginRequest.Name} ({loginRequest.Uuid}).");

await Task.Delay(TimeSpan.FromSeconds(1));
client.SendPacket(new ClientToServerChatMessage(@"раз"));
await Task.Delay(TimeSpan.FromSeconds(1));
client.SendPacket(new ClientToServerChatMessage(@"два"));
await Task.Delay(TimeSpan.FromSeconds(1));
client.SendPacket(new ClientToServerChatMessage(@"три"));
await Task.Delay(TimeSpan.FromSeconds(1));
client.SendPacket(new ClientToServerChatMessage(@"/setblock ~0 ~5 ~0 minecraft:grass"));

while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) ;