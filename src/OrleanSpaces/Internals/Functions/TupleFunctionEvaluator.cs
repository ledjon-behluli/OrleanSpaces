using Orleans;
using OrleanSpaces.Types;
using System.Diagnostics;

namespace OrleanSpaces.Internals.Functions;

internal class TupleFunctionEvaluator : IOutgoingGrainCallFilter
{
    private readonly IGrainFactory factory;
    private readonly TupleFunctionSerializer serializer;

    public TupleFunctionEvaluator(
        IGrainFactory factory,
        TupleFunctionSerializer serializer)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public async Task Invoke(IOutgoingGrainCallContext context)
    {
        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceProvider.EvaluateAsync)))
        {
            if (context.Arguments.Length > 0 && context.Arguments[0] is Func<SpaceTuple> func)
            {
                byte[] serializedFunc = serializer.Serialize(func);
                ISpaceProvider provider = factory.GetSpaceProvider();
                await provider.EvaluateAsync(serializedFunc);

                return;
            }
        }

        if (await TryRunNonBlockingVariants(nameof(ISpaceProvider.PeekAsync),
            context, async (sp, st) => await sp.TryPeekAsync(st)))
        {
            return;
        }

        if (await TryRunNonBlockingVariants(nameof(ISpaceProvider.PopAsync),
            context, async (sp, st) => await sp.TryPopAsync(st)))
        {
            return;
        }

        await context.Invoke();
    }

    private async ValueTask<bool> TryRunNonBlockingVariants(
        string targetMethodName,
        IOutgoingGrainCallContext context,
        Func<ISpaceProvider, SpaceTemplate, ValueTask<SpaceTuple?>> func)
    {
        if (string.Equals(context.InterfaceMethod.Name, targetMethodName))
        {
            if (context.Arguments.Length > 0 && context.Arguments[0] is SpaceTemplate template)
            {
                ISpaceProvider provider = factory.GetSpaceProvider();
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
