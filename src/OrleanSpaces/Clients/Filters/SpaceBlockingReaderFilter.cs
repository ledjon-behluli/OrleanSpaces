using Orleans;
using OrleanSpaces.Core;
using OrleanSpaces.Core.Observers;
using OrleanSpaces.Core.Primitives;
using OrleanSpaces.Core.Utils;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace OrleanSpaces.Clients.Filters;

internal class SpaceBlockingReaderFilter : ISpaceObserver, IOutgoingGrainCallFilter
{
    private readonly Channel<SpaceTuple> channel = Channel.CreateUnbounded<SpaceTuple>();
    private readonly ConcurrentDictionary<SpaceTemplate, NullTuple> templates = new();

    private readonly IGrainFactory factory;

    public SpaceBlockingReaderFilter(IGrainFactory factory)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public void Receive(SpaceTuple tuple)
    {
        foreach (var pair in templates.Where(x => x.Key.Length == tuple.Length))
        {
            if (TupleMatcher.IsMatch(tuple, pair.Key))
            {
                if (channel.Writer.TryWrite(tuple))
                {
                    templates.TryRemove(pair.Key, out _);
                }

                break;
            }
        }
    }

    public async Task Invoke(IOutgoingGrainCallContext context)
    {
        if (await TryInvoke(nameof(ISpaceBlockingReader.PeekAsync),
            context, async (reader, template) => await reader.PeekAsync(template)))
        {
            return;
        }

        if (await TryInvoke(nameof(ISpaceBlockingReader.PopAsync),
            context, async (reader, template) => await reader.PopAsync(template)))
        {
            return;
        }

        await context.Invoke();
    }

    private async ValueTask<bool> TryInvoke(
        string targetMethodName,
        IOutgoingGrainCallContext context,
        Func<ISpaceReader, SpaceTemplate, ValueTask<SpaceTuple?>> func)
    {
        if (string.Equals(context.InterfaceMethod.Name, targetMethodName))
        {
            if (context.Arguments.Length == 2 && 
                context.Arguments[0] is SpaceTemplate template &&
                context.Arguments[1] is Action<SpaceTuple> callback)
            {
                ValueTask<SpaceTuple?> task = func(factory.GetSpaceReader(), template);

                if (await task is SpaceTuple tuple)
                {
                    callback(tuple);
                    context.Result = new ValueTask();
                }
                else
                {
                    templates.TryAdd(template, NullTuple.Value);
                }

                return true;
            }
            else
            {
                throw new Exception("Invalid arguments passed to space method.");
            }
        }

        return false;
    }
}