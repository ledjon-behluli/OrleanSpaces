using Orleans;

namespace OrleanSpaces.Internals;

internal class TupleFunctionEvaluator : IOutgoingGrainCallFilter
{
    private readonly ITupleFunctionExecutor executor;
    private readonly TupleFunctionSerializer serializer;

    public TupleFunctionEvaluator(
        ITupleFunctionExecutor executor,
        TupleFunctionSerializer serializer)
    {
        this.executor = executor ?? throw new ArgumentNullException(nameof(executor));
        this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public async Task Invoke(IOutgoingGrainCallContext context)
    {
        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceProvider.EvaluateAsync)))
        {
            if (context.Arguments.Length > 0 &&
                context.Arguments[0] != null &&
                context.Arguments[0] is TupleFunction function)
            {
                var serializedFunction = serializer.Serialize(function);
                await executor.ExecuteAsync(serializedFunction);

                return;
            }
        }

        await context.Invoke();
    }
}
