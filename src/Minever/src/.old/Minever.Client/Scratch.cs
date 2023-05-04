// var protocol               = new JavaProtocolX(); // or BedrockProtocolX
// IPacketClient packetClient = new JavaPacketClient(protocol, logger); // or BedrockPacketClient
// var client                 = new MinecraftClient(packetClient, logger);

// await MinecraftClient.PingAsync(address, port);
// client.Disconnected += ...;
// await client.ConnectAsync(address, port);
// client.Player.Health;
// client.World.Current.GetBlock<GrassBlock>();
// await client.DisconnectAsync();



// if (isBedrock)
//     return await BedrockPacketClient.PingAsync(address, port);
// else
//     return await JavaPacketClient.PingAsync(address, port);



// internal WorldHandler(IPacketClient packetClient)
// {
//     ArgumentNullException.ThrowIfNull(packetClient);
//
//     if (IPacketClient is JavaPacketClient)
//     {
//         packetClient.OnPacket<JavaWorldTime>(worldTime =>
//         {
//             Age       = worldTime.Age;
//             TimeOfDay = worldTime.TimeOfDay;
//         });
//     }
//     else if (IPacketClient is BedrockPacketClient)
//     {
//         packetClient.OnPacket<BedrockWorldTime>(worldTime =>
//         {
//             Age       = worldTime.Time;
//             TimeOfDay = TimeSpan.FromSeconds(worldTime.Time.TotalSeconds % 24000);
//         });
//     }
//     else
//     {
//         throw new NotSupportedException();
//     }
// }

// World = new WorldHandler(_packetClient);