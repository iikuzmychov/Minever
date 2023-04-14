using Minever.Core;
using Minever.Java.Protocols.V5.Packets;
using Minever.Networking.Packets;

namespace Minever.Java.Protocols.V5.Controllers;

public class ChatController : ControllerBase, IChatController
{
    public event Action<string, DateTime>? MessageReceived;

    public ChatController(IClientDataProvider client) : base(client)
    {
        // todo: if (Client.PacketTransceiver.Protocol is not JavaProtocol5) throw

        Client.PacketTransceiver.OnPacket<ChatMessageFromServer>((message, dateTime) => MessageReceived?.Invoke(message.Text, dateTime));
    }

    public void SendMessage(string message) => Client.PacketTransceiver.SendPacket(new ChatMessageToServer(message));
}