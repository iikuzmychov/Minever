namespace Minever.Core;

public interface IControllerProvider
{
    public IController GetController(Type controllerType);

    public IController? GetControllerOrDefault(Type controllerType);
}
