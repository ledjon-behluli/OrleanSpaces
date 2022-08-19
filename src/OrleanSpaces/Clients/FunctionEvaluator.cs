using Orleans;
using OrleanSpaces.Core;
using OrleanSpaces.Core.Primitives;
using OrleanSpaces.Core.Utils;

namespace OrleanSpaces.Clients;

internal class FunctionEvaluator : IOutgoingGrainCallFilter
{
    private readonly IGrainFactory factory;
    private readonly FunctionSerializer serializer;

    public FunctionEvaluator(
        IGrainFactory factory,
        FunctionSerializer serializer)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public async Task Invoke(IOutgoingGrainCallContext context)
    {
        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceGrain.EvaluateAsync)))
        {
            if (context.Arguments.Length > 0 && context.Arguments[0] is Func<SpaceTuple> func)
            {
                byte[] serializedFunc = serializer.Serialize(func);
                ISpaceGrain provider = factory.GetSpaceProvider();
                await provider.EvaluateAsync(serializedFunc);

                return;
            }
        }

        if (await TryRunNonBlockingVariants(nameof(ISpaceGrain.PeekAsync),
            context, async (sp, st) => await sp.TryPeekAsync(st)))
        {
            return;
        }

        if (await TryRunNonBlockingVariants(nameof(ISpaceGrain.PopAsync),
            context, async (sp, st) => await sp.TryPopAsync(st)))
        {
            return;
        }

        await context.Invoke();
    }

    private async ValueTask<bool> TryRunNonBlockingVariants(
        string targetMethodName,
        IOutgoingGrainCallContext context,
        Func<ISpaceGrain, SpaceTemplate, ValueTask<SpaceTuple?>> func)
    {
        if (string.Equals(context.InterfaceMethod.Name, targetMethodName))
        {
            if (context.Arguments.Length > 0 && context.Arguments[0] is SpaceTemplate template)
            {
                ISpaceGrain provider = factory.GetSpaceProvider();
                SpaceTuple? tuple = await func(provider, template);

                if (tuple is SpaceTuple)
                {
                    context.Result = tuple;
                    return true;
                }
                else
                {
                    // TODO: ISpaceAgent
                }
            }
        }

        return false;
    }
}
