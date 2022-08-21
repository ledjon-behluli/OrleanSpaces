using Orleans;
using OrleanSpaces.Core.Grains;
using OrleanSpaces.Core.Primitives;
using OrleanSpaces.Core.Utils;
using OrleanSpaces.Hosts.Observers;

namespace OrleanSpaces.Hosts.Grains;

internal class Interceptor : IIncomingGrainCallFilter
{
    private readonly IGrainFactory factory;
    private readonly IObserverNotifier notifier;

    public Interceptor(
        IGrainFactory factory,
        IObserverNotifier notifier)
    {
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
            Func<SpaceTuple> function = LambdaSerializer.Deserialize((byte[])context.Arguments[0]);
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
