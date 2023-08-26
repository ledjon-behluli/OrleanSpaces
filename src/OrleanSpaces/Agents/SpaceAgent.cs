using OrleanSpaces.Tuples;
using OrleanSpaces.Helpers;
using System.Threading.Channels;
using System.Collections.Immutable;
using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Agents;

internal sealed class SpaceAgent : ISpaceAgent, ISpaceRouter<SpaceTuple, SpaceTemplate>
{
    private readonly static object lockObj = new();
    private readonly Guid agentId = Guid.NewGuid();
    private readonly SpaceOptions options;
    private readonly EvaluationChannel<SpaceTuple> evaluationChannel;
    private readonly ObserverRegistry<SpaceTuple> observerRegistry;
    private readonly CallbackRegistry callbackRegistry;

    [AllowNull] private ITupleStore<SpaceTuple> tupleStore;
    private Channel<SpaceTuple>? streamChannel;
    private ImmutableArray<SpaceTuple> tuples = ImmutableArray<SpaceTuple>.Empty; // chosen for thread safety reasons

    public SpaceAgent(
        SpaceOptions options,
        EvaluationChannel<SpaceTuple> evaluationChannel,
        ObserverRegistry<SpaceTuple> observerRegistry,
        CallbackRegistry callbackRegistry)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
        this.evaluationChannel = evaluationChannel ?? throw new ArgumentNullException(nameof(evaluationChannel));
        this.observerRegistry = observerRegistry ?? throw new ArgumentNullException(nameof(observerRegistry));
        this.callbackRegistry = callbackRegistry ?? throw new ArgumentNullException(nameof(callbackRegistry));
    }

    #region ISpaceRouter

    void ISpaceRouter<SpaceTuple, SpaceTemplate>.RouteStore(ITupleStore<SpaceTuple> tupleStore) 
        => this.tupleStore = tupleStore;

    async ValueTask ISpaceRouter<SpaceTuple, SpaceTemplate>.RouteAction(TupleAction<SpaceTuple> action)
    {
        if (action.AgentId != agentId)
        {
            switch (action.Type)
            {
                case TupleActionType.Insert:
                    {
                        tuples = tuples.Add(action.Tuple);
                        await streamChannel.WriteIfNotNull(action.Tuple);
                    }
                    break;
                case TupleActionType.Remove:
                    tuples = tuples.Remove(action.Tuple);
                    break;
                case TupleActionType.Clear:
                    tuples = ImmutableArray<SpaceTuple>.Empty;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }

    Task ISpaceRouter<SpaceTuple, SpaceTemplate>.RouteTuple(SpaceTuple tuple) => WriteAsync(tuple);
    async ValueTask ISpaceRouter<SpaceTuple, SpaceTemplate>.RouteTemplate(SpaceTemplate template) => await PopAsync(template);

    #endregion

    #region ISpaceAgent

    public Guid Subscribe(ISpaceObserver<SpaceTuple> observer)
        => observerRegistry.Add(observer);

    public void Unsubscribe(Guid observerId)
        => observerRegistry.Remove(observerId);

    public async Task WriteAsync(SpaceTuple tuple)
    {
        ThrowHelpers.EmptyTuple(tuple);

        await tupleStore.Insert(new(agentId, tuple, TupleActionType.Insert));
        await streamChannel.WriteIfNotNull(tuple);

        tuples = tuples.Add(tuple);
    }

    public ValueTask EvaluateAsync(Func<Task<SpaceTuple>> evaluation)
    {
        if (evaluation == null) throw new ArgumentNullException(nameof(evaluation));
        return evaluationChannel.Writer.WriteAsync(evaluation);
    }

    public ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template)
    {
        SpaceTuple tuple = tuples.FirstOrDefault(template.Matches);
        return new(tuple);
    }

    public async ValueTask PeekAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        SpaceTuple tuple = tuples.FirstOrDefault(template.Matches);

        if (tuple.IsEmpty)
        {
            callbackRegistry.Add(template, new(callback, false));
            return;
        }

        await callback(tuple);
    }

    public async ValueTask<SpaceTuple> PopAsync(SpaceTemplate template)
    {
        SpaceTuple tuple = tuples.FirstOrDefault(template.Matches);

        if (!tuple.IsEmpty)
        {
            await tupleStore.Remove(new(agentId, tuple, TupleActionType.Remove));
            tuples = tuples.Remove(tuple);
        }

        return tuple;
    }

    public async ValueTask PopAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        SpaceTuple tuple = tuples.FirstOrDefault(template.Matches);

        if (tuple.IsEmpty)
        {
            callbackRegistry.Add(template, new(callback, true));
            return;
        }

        await callback(tuple);
        await tupleStore.Remove(new(agentId, tuple, TupleActionType.Remove));

        tuples = tuples.Remove(tuple);
    }

    public ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template)
    {
        List<SpaceTuple> result = new();

        foreach (var tuple in tuples)
        {
            if (template.Matches(tuple))
            {
                result.Add(tuple);
            }
        }

        return new(result);
    }

    public async IAsyncEnumerable<SpaceTuple> PeekAsync()
    {
        lock (lockObj)
        {
            if (streamChannel is null)
            {
                streamChannel = Channel.CreateUnbounded<SpaceTuple>(new()
                {
                    SingleReader = !options.AllowMultipleAgentStreamConsumers,
                    SingleWriter = true
                });


                foreach (var tuple in tuples)
                {
                    _ = streamChannel.Writer.TryWrite(tuple);  // will always be able to write to the channel
                }
            }
        }

        await foreach (SpaceTuple tuple in streamChannel.Reader.ReadAllAsync())
        {
            yield return tuple;
        }
    }

    public ValueTask<int> CountAsync() => new(tuples.Length);

    public async Task ClearAsync()
    {
        await tupleStore.RemoveAll(agentId);
        tuples = ImmutableArray<SpaceTuple>.Empty;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private async Task TryWriteToStream(SpaceTuple tuple)
    {
        if (streamChannel is not null)
        {
            await streamChannel.Writer.WriteAsync(tuple);
        }
    }

    #endregion
}