using Minever.Core.Controllers;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Minever.Core;

public sealed class ControllerProvider : IControllerProvider
{
    private readonly ImmutableDictionary<Type, IController> _controllers;

    internal ControllerProvider(IControllerCollection controllers, IClientDataProvider client)
    {
        Debug.Assert(controllers is not null);
        Debug.Assert(controllers.All(controller => controller is not null));
        Debug.Assert(client is not null);

        _controllers = controllers.ToImmutableDictionary(controller => controller.Type, controller => controller.CreateInstance(client));
    }
}
