using Orleans;
using OrleanSpaces.Observables;

namespace OrleanSpaces.Filters;

internal sealed class SpaceOscillationNotifier : IIncomingGrainCallFilter
{
    private readonly ObserverManager manager;

    public SpaceOscillationNotifier(ObserverManager manager)
    {
        this.manager = manager ?? throw new ArgumentNullException(nameof(manager));
    }

    public async Task Invoke(IIncomingGrainCallContext context)
    {
        await context.Invoke();

        if (string.Equals(context.InterfaceMethod.Name, nameof(ITupleSpace.Write)))
        {
            manager.Broadcast(observer => observer.OnExpansion());
            return;
        }

        if (string.Equals(context.InterfaceMethod.Name, nameof(ITupleSpace.Extract)))
        {
            manager.Broadcast(observer => observer.OnContraction());
            return;
        }

        if (string.Equals(context.InterfaceMethod.Name, nameof(ITupleSpace.TryExtract)))
        {
            if (context.Result is SpaceResult spaceResult)
            {
                if (spaceResult.Result)
                {
                    manager.Broadcast(observer => observer.OnContraction());
                }
            }
        }
    }
}