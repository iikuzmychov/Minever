using Microsoft.Extensions.DependencyInjection;

namespace Minever.Core.Controllers;


public class ControllerDescriptor
{


    public ControllerDescriptor(Type controllerType, Func<IClientDataProvider, IController> factory)
    {
        ArgumentNullException.ThrowIfNull(controllerType);


    }
}