using Orleans;
using System;
using System.Threading.Tasks;

namespace OrleanSpaces.Observables;

internal sealed class SpaceNotificationTrigger : IIncomingGrainCallFilter
{
    private readonly SpaceObserverManager manager;

    public SpaceNotificationTrigger(SpaceObserverManager manager)
    {
        this.manager = manager ?? throw new ArgumentNullException(nameof(manager));
    }

    public async Task Invoke(IIncomingGrainCallContext context)
    {
        await context.Invoke();

        if (string.Equals(context.InterfaceMethod.Name, nameof(ITupleSpace.Put)))
        {
            manager.Broadcast(observer => observer.OnExpansion());
        }
        else if (string.Equals(context.InterfaceMethod.Name, nameof(ITupleSpace.Read)))
        {
            manager.Broadcast(observer => observer.OnContraction());
        }
        else if (string.Equals(context.InterfaceMethod.Name, nameof(ITupleSpace.TryRead)))
        {
            if (context.Result is SpaceResult resultValue)
            {
                if (resultValue.Result)
                {
                    manager.Broadcast(observer => observer.OnContraction());
                }
            }
        }
    }
}
