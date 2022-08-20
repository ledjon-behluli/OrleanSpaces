using Orleans;
using OrleanSpaces.Core;
using OrleanSpaces.Core.Primitives;
using OrleanSpaces.Core.Utils;

namespace OrleanSpaces.Hosts.Internals;

internal class Interceptor : IIncomingGrainCallFilter
{
    private readonly LambdaSerializer serializer;
    private readonly IGrainFactory factory;
    private readonly IObserverNotifier notifier;

    public Interceptor(
        LambdaSerializer serializer,
        IGrainFactory factory,
        IObserverNotifier notifier)
    {
        this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        this.notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
    }

    public async Task Invoke(IIncomingGrainCallContext context)
    {
        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceGrain.WriteAsync)))
        {
            await context.Invoke();
            notifier.Broadcast(agent => agent.Receive((SpaceTuple)context.Arguments[0]));

            return;
        }

        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceGrain.EvaluateAsync)))
        {
            Func<SpaceTuple> function = serializer.Deserialize((byte[])context.Arguments[0]);
            object result = function.DynamicInvoke();

            if (result is SpaceTuple tuple)
            {
                await factory.GetSpaceGrain().WriteAsync(tuple);
                notifier.Broadcast(agent => agent.Receive(tuple));
            }

            return;
        }

        await context.Invoke();
    }
}
