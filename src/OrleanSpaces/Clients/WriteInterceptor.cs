using Orleans;
using OrleanSpaces.Core;
using OrleanSpaces.Core.Primitives;
using OrleanSpaces.Core.Utils;

namespace OrleanSpaces.Clients;

internal class WriteInterceptor : IOutgoingGrainCallFilter
{
    private readonly IGrainFactory factory;
    private readonly FuncSerializer serializer;

    public WriteInterceptor(
        IGrainFactory factory,
        FuncSerializer serializer)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public async Task Invoke(IOutgoingGrainCallContext context)
    {
        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceWriter.EvaluateAsync)))
        {
            if (context.Arguments.Length == 1 && context.Arguments[0] is Func<SpaceTuple> func)
            {
                await factory.GetSpaceWriter().EvaluateAsync(serializer.Serialize(func));
                return;
            }
            else
            {
                throw new Exception("Invalid arguments passed to space method.");
            }
        }
    }
}