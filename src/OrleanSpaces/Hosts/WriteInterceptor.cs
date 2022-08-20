using Orleans;
using OrleanSpaces.Core;
using OrleanSpaces.Core.Observers;
using OrleanSpaces.Core.Primitives;
using OrleanSpaces.Hosts.Observers;

namespace OrleanSpaces.Hosts;

internal class WriteInterceptor : IIncomingGrainCallFilter
{
    private readonly ISpaceObserver agent;
    private readonly IObserverNotifier notifier;

    public WriteInterceptor(
        ISpaceObserver agent,
        IObserverNotifier notifier)
    {
        this.agent = agent ?? throw new ArgumentNullException(nameof(agent));
        this.notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
    }

    public async Task Invoke(IIncomingGrainCallContext context)
    {
        await context.Invoke();

        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceWriter.WriteAsync)))
        {
            if (context.Arguments.Length > 0 && context.Arguments[0] is SpaceTuple tuple)
            {
                notifier.Broadcast(agent => agent.Receive(tuple));
                return;
            }
        }

        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceWriter.EvaluateAsync)))
        {
            if (context.Arguments.Length > 0 && context.Arguments[0] is Func<SpaceTuple> func)
            {
                notifier.Broadcast(agent => agent.Receive(func()));
                return;
            }
        }
    }
}
