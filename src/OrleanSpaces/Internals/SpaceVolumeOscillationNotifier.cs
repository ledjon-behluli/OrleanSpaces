using Orleans;
using OrleanSpaces.Types;

namespace OrleanSpaces.Internals;

internal class SpaceVolumeOscillationNotifier : IIncomingGrainCallFilter
{
    private readonly SpaceObserverManager manager;

    public SpaceVolumeOscillationNotifier(SpaceObserverManager manager)
    {
        this.manager = manager ?? throw new ArgumentNullException(nameof(manager));
    }

    public async Task Invoke(IIncomingGrainCallContext context)
    {
        await context.Invoke();

        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceProvider.WriteAsync)))
        {
            manager.Broadcast(observer => observer.OnExpansion());
            return;
        }

        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceProvider.ExtractAsync)))
        {
            manager.Broadcast(observer => observer.OnContraction());
            return;
        }

        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceProvider.TryExtractAsync)))
        {
            if (context.Result is TupleResult spaceResult)
            {
                if (spaceResult.Success)
                {
                    manager.Broadcast(observer => observer.OnContraction());
                }
            }
        }
    }
}
