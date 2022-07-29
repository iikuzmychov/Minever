using Microsoft.Extensions.Logging;
using Minever.Client;
using Minever.Client.ConsoleApplication;
using Minever.Networking.Packets;
using Minever.Networking.Protocols;

const string ServerAddress = "localhost";
const ushort ServerPort    = 50641;

ThreadSafeConsole.ForegroundColor = ConsoleColor.Magenta;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Debug)
        .AddConsole();
});

var logger         = loggerFactory.CreateLogger<MinecraftPacketClient>();
var protocol       = MinecraftProtocol.FromVersion(0);
var requestTimeout = TimeSpan.FromSeconds(15);

await using var client = new MinecraftPacketClient(protocol, logger);
client.OnPacket<KeepAlive>(packet => client.SendPacket(new KeepAlive(packet.Data.Id)));
client.OnPacket<JoinGame>(packet => ThreadSafeConsole.WriteLine($"Max players count: {packet.Data.MaxPlayersCount}."));
client.OnPacket<SpawnPosition>(packet => ThreadSafeConsole.WriteLine($"Spawn position: {packet.Data.Position}."));
client.OnPacket<Respawn>(packet => ThreadSafeConsole.WriteLine("Respawn."));
client.OnPacket<PlayerAbilities>(packet => ThreadSafeConsole.WriteLine($"Flying speed: {packet.Data.FlyingSpeed}, walking speed: {packet.Data.WalkingSpeed}."));
client.OnPacket<PlayerPositionAndLook>(packet =>
{
    client.SendPacket(new PlayerPositionAndLookWithStance(packet.Data, 1d));
    client.SendPacket(new ClientStatus(ClientStatusAction.PerformRespawn));
    ThreadSafeConsole.WriteLine($"Player position: {packet.Data.Position}");
});
client.OnPacket<HeldItemChange>(packet => ThreadSafeConsole.WriteLine("HeldItemChange."));
client.OnPacket<TimeUpdate>(packet => ThreadSafeConsole.WriteLine($"Age of world: {packet.Data.WorldAge}, time of day: {packet.Data.DayTime}."));
client.OnPacket<PlayerListItem>(packet => ThreadSafeConsole.WriteLine($"Player: {packet.Data.PlayerName}, is connected: {packet.Data.IsConnected}, ping: {packet.Data.Ping}."));
client.OnPacket<UpdateHealth>(packet =>
{
    if (packet.Data.Health < 0)
        client.SendPacket(new ClientStatus(ClientStatusAction.PerformRespawn));

    ThreadSafeConsole.WriteLine($"Health: {packet.Data.Health}, food: {packet.Data.Food}, food saturation: {packet.Data.FoodSaturation}.");
});
client.OnPacket<PluginMessage>(packet => ThreadSafeConsole.WriteLine($"Plugin message from channel '{packet.Data.ChannelName}'"));

await client.ConnectAsync(ServerAddress, ServerPort);

var handshake = new Handshake(client.Protocol.Version, ServerAddress, ServerPort, HandshakeNextState.Login);
client.SendPacket(handshake);

var loginSuccess = (await client.SendRequestAsync<LoginSuccess>(new LoginStart("KuzCode23")).WaitAsync(requestTimeout)).Data;
ThreadSafeConsole.WriteLine($"Login success! {loginSuccess.Name} ({loginSuccess.Uuid}).");

Console.ReadKey();