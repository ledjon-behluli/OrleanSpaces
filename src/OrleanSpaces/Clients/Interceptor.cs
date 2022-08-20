using Orleans;
using OrleanSpaces.Clients.Callbacks;
using OrleanSpaces.Core;
using OrleanSpaces.Core.Exceptions;
using OrleanSpaces.Core.Primitives;
using OrleanSpaces.Core.Utils;

namespace OrleanSpaces.Clients;

internal class Interceptor : IOutgoingGrainCallFilter
{
    private readonly IGrainFactory factory;
    private readonly ICallbackBuffer buffer;
    private readonly FuncSerializer serializer;

    public Interceptor(
        IGrainFactory factory,
        ICallbackBuffer buffer,
        FuncSerializer serializer)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        this.buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public async Task Invoke(IOutgoingGrainCallContext context)
    {
        if (await TryEvaluateSerialized(context))
        {
            return;
        }

        if (await TryNonBlockingRead(nameof(ISpaceBlockingReader.PeekAsync),
            context, async (reader, template) => await reader.PeekAsync(template)))
        {
            return;
        }

        if (await TryNonBlockingRead(nameof(ISpaceBlockingReader.PopAsync),
            context, async (reader, template) => await reader.PopAsync(template)))
        {
            return;
        }

        await context.Invoke();
    }

    private async ValueTask<bool> TryEvaluateSerialized(IOutgoingGrainCallContext context)
    {
        string methodName = nameof(ISpaceWriter.EvaluateAsync);

        if (string.Equals(context.InterfaceMethod.Name, methodName))
        {
            if (context.Arguments.Length == 1 && context.Arguments[0] is Func<SpaceTuple> func)
            {
                await factory.GetSpaceWriter().EvaluateAsync(serializer.Serialize(func));
                return true;
            }
            else
            {
                throw new SpaceArgumentException(methodName);
            }
        }

        return false;
    }

    private async ValueTask<bool> TryNonBlockingRead(
        string targetMethodName,
        IOutgoingGrainCallContext context,
        Func<ISpaceReader, SpaceTemplate, ValueTask<SpaceTuple?>> func)
    {
        if (string.Equals(context.InterfaceMethod.Name, targetMethodName))
        {
            if (context.Arguments.Length == 2 &&
                context.Arguments[0] is SpaceTemplate template &&
                context.Arguments[1] is Func<SpaceTuple, Task> callback)
            {
                ValueTask<SpaceTuple?> task = func(factory.GetSpaceReader(), template);

                if (await task is SpaceTuple tuple)
                {
                    await callback(tuple);
                }
                else
                {
                    buffer.Buffer(template, callback);
                }

                return true;
            }
            else
            {
                throw new SpaceArgumentException(targetMethodName);
            }
        }

        return false;
    }
}