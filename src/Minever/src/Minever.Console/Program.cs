using Minever.Core;
using Minever.Core.Extensions;
using Minever.Java.Universal;
using Minever.Universal;

const string Host = "localhost";
const int Port    = 51899;

var protocol = await MinecraftProtocol.DetectAsync(Host, Port);

var builder = MinecraftClientBuilder.ForProtocol(protocol);
builder.Controllers.Add<IChatController>(client => new Minever.Java.Protocols.V5.Controllers.ChatController(client));
//builder.Controllers.AddChatController();

using var client = builder.Build();

var chat = client.Controllers.GetController<IChatController>();

await client.ConnectAsync(Host, Port);
chat.SendMessage("hi");