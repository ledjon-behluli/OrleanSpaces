using Orleans;
using OrleanSpaces.Types;

namespace OrleanSpaces.Internals;

internal class VolumeOscillationNotifier : IIncomingGrainCallFilter
{
    private readonly ObserverManager manager;

    public VolumeOscillationNotifier(ObserverManager manager)
    {
        this.manager = manager ?? throw new ArgumentNullException(nameof(manager));
    }

    public async Task Invoke(IIncomingGrainCallContext context)
    {
        await context.Invoke();

        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceProvider.Write)))
        {
            manager.Broadcast(observer => observer.OnExpansion());
            return;
        }

        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceProvider.Extract)))
        {
            manager.Broadcast(observer => observer.OnContraction());
            return;
        }

        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceProvider.TryExtract)))
        {
            if (context.Result is TupleResult spaceResult)
            {
                if (spaceResult.Result)
                {
                    manager.Broadcast(observer => observer.OnContraction());
                }
            }
        }
    }
}
