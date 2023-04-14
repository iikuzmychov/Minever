namespace Minever.Core.Extensions;

public static class ControllerProviderExtensions
{
    public static TController GetController<TController>(this IControllerProvider provider)
        where TController : IController
    {
        ArgumentNullException.ThrowIfNull(provider);

        return (TController)provider.GetController(typeof(TController));
    }
}
