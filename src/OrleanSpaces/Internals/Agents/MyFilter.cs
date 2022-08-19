using Orleans;
using OrleanSpaces.Core;
using OrleanSpaces.Core.Primitives;

namespace OrleanSpaces.Internals.Agents;

internal class MyFilter : IIncomingGrainCallFilter
{
    private readonly ISpaceAgent agent;
    private readonly ISpaceAgentNotifier notifier;

    public MyFilter(
        ISpaceAgent agent,
        ISpaceAgentNotifier notifier)
    {
        this.agent = agent ?? throw new ArgumentNullException(nameof(agent));
        this.notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
    }

    public async Task Invoke(IIncomingGrainCallContext context)
    {
        await context.Invoke();

        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceGrain.WriteAsync)))
        {
            if (context.Arguments.Length > 0 && context.Arguments[0] is SpaceTuple tuple)
            {
                notifier.Broadcast(agent => agent.OnTuple(tuple));
                return;
            }
        }

        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceGrain.EvaluateAsync)))
        {
            if (context.Arguments.Length > 0 && context.Arguments[0] is Func<SpaceTuple> func)
            {
                notifier.Broadcast(agent => agent.OnTuple(func()));
                return;
            }
        }
    }
}
