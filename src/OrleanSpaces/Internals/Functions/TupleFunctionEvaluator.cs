using Orleans;
using OrleanSpaces.Types;

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

        await context.Invoke();
    }
}
