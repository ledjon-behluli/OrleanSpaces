using Orleans;
using OrleanSpaces.Internals.Agents;
using OrleanSpaces.Types;

namespace OrleanSpaces.Internals.Observations;

// TODO: Maybe add more stuff, like blovking read's here?
internal class MyFilter : IIncomingGrainCallFilter
{
    private readonly ISpaceAgent agent;
    private readonly ISpaceFluctuationNotifier notifier;

    public MyFilter(
        ISpaceAgent agent,
        ISpaceFluctuationNotifier notifier)
    {
        this.agent = agent ?? throw new ArgumentNullException(nameof(agent));
        this.notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
    }

    public async Task Invoke(IIncomingGrainCallContext context)
    {
        await context.Invoke();

        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceProvider.WriteAsync)))
        {
            notifier.Broadcast(observer => observer.OnExpansion());


            return;
        }

        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceProvider.PopAsync)))
        {
            notifier.Broadcast(observer => observer.OnContraction());
            return;
        }

        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceProvider.TryPopAsync)))
        {
            if (context.Result is SpaceTuple)
            {
                notifier.Broadcast(observer => observer.OnContraction());
            }
        }
    }
}
