namespace Minever.Core;

public interface IChatController : IController
{
    public event Action<string, DateTime>? MessageReceived; // todo: replace string with MinecraftText

    public void SendMessage(string text);
}
