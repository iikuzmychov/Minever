using Minever.Bedrock.Universal;
using Minever.Core;
using Minever.Java.Universal;

namespace Minever.Universal;

public static class MinecraftProtocol
{
    public static async Task<IProtocol> DetectAsync(string host, int port, CancellationToken cancellationToken = default)
    {
        var exceptions = new List<Exception>(capacity: 2);

        try
        {
            return await JavaProtocol.DetectAsync(host, port, cancellationToken);
        }
        catch (Exception exception)
        {
            exceptions.Add(exception);
        }

        try
        {
            return await BedrockProtocol.DetectAsync(host, port, cancellationToken);
        }
        catch (Exception exception)
        {
            exceptions.Add(exception);
        }

        throw new AggregateException(exceptions);
    }
}
