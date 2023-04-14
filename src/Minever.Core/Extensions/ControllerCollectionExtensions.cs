using Minever.Core.Controllers;

namespace Minever.Core.Extensions;

public static class ControllerCollectionExtensions
{
    public static IControllerCollection Add<TController>(this IControllerCollection controllers, Func<IClientDataProvider, TController> factory)
        where TController : IController
        => controllers.Add(new ControllerDescriptor(factory));

    public static IControllerProvider BuildServiceProvider(this IControllerCollection controllers, IClientDataProvider client)
        => new ControllerProvider(controllers, client);
}
