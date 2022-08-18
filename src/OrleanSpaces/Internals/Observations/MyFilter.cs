using Orleans;
using OrleanSpaces.Types;

namespace OrleanSpaces.Internals.Observations;

// TODO: Maybe add more stuff, like blovking read's here?
internal class MyFilter : IIncomingGrainCallFilter
{
    private readonly ISpaceFluctuationNotifier notifier;

    public MyFilter(ISpaceFluctuationNotifier notifier)
    {
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

        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceProvider.ExtractAsync)))
        {
            notifier.Broadcast(observer => observer.OnContraction());
            return;
        }

        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceProvider.TryExtractAsync)))
        {
            if (context.Result is SpaceTuple)
            {
                notifier.Broadcast(observer => observer.OnContraction());
            }
        }
    }
}
