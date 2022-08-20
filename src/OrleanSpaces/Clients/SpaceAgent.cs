using Orleans;
using OrleanSpaces.Core;
using OrleanSpaces.Core.Observers;
using OrleanSpaces.Core.Primitives;
using OrleanSpaces.Core.Utils;
using System.Collections.Concurrent;

namespace OrleanSpaces.Clients;

internal interface IPromisesBuffer
{
    void Buffer(TuplePromise promise);
}

internal class SpaceAgent : ISpaceObserver, IOutgoingGrainCallFilter
{
    private readonly PromisesChannel channel;
    private readonly ConcurrentDictionary<SpaceTemplate, List<TuplePromise>> templatesDict = new();

    private readonly IGrainFactory factory;

    public SpaceAgent(IGrainFactory factory, PromisesChannel channel)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
    }

    public void Receive(SpaceTuple tuple)
    {
        foreach (var pair in templatesDict.Where(x => x.Key.Length == tuple.Length))
        {
            if (TupleMatcher.IsMatch(tuple, pair.Key))
            {
                foreach (var promise in templatesDict[pair.Key])
                {
                    channel.Writer.TryWrite(promise);
                }

                templatesDict.TryRemove(pair.Key, out _);
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
                context.Arguments[1] is TuplePromise promise)
            {
                ValueTask<SpaceTuple?> task = func(factory.GetSpaceReader(), template);

                if (await task is SpaceTuple tuple)
                {
                    promise(tuple);
                }
                else
                {
                    if (!templatesDict.ContainsKey(template))
                    {
                        templatesDict.TryAdd(template, new());
                    }

                    templatesDict[template].Add(promise);
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