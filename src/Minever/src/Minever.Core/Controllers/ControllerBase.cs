namespace Minever.Core;

public abstract class ControllerBase : IController
{
    protected IClientDataProvider Client { get; }

    public ControllerBase(IClientDataProvider client)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
    }

    //protected TController GetController<TController>()
    //    where TController : IController
    //{
    //    if (Client!.Controllers.TryGetController<TController>(out var controller))
    //    {
    //        return controller;
    //    }

    //    // todo: update message
    //    throw new AggregateException($@"{typeof(TController).FullName} controller required. Add the next code to the client builder pipeline: .AddController<{typeof(TController).FullName}>");
    //}
}
