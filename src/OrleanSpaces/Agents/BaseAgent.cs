using OrleanSpaces.Channels;
using OrleanSpaces.Helpers;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;

namespace OrleanSpaces.Agents;

internal class BaseAgent<T, TTuple, TTemplate> : ISpaceAgent<T, TTuple, TTemplate>, ISpaceRouter<TTuple, TTemplate>
    where T : unmanaged
    where TTuple : struct, ISpaceTuple<T>
    where TTemplate : struct, ISpaceTemplate<T>, ISpaceMatchable<T, TTuple>
{
    private readonly static object lockObj = new();
    private readonly Guid agentId = Guid.NewGuid();
    private readonly SpaceOptions options;
    private readonly EvaluationChannel<TTuple> evaluationChannel;
    private readonly ObserverRegistry<TTuple> observerRegistry;
    private readonly CallbackRegistry<T, TTuple, TTemplate> callbackRegistry;

    [AllowNull] private ITupleStore<TTuple> tupleStore;
    private Channel<TTuple>? streamChannel;
    private readonly TupleCollection<T, TTuple, TTemplate> tuples = new();

    public BaseAgent(
        SpaceOptions options,
        EvaluationChannel<TTuple> evaluationChannel,
        ObserverRegistry<TTuple> observerRegistry,
        CallbackRegistry<T, TTuple, TTemplate> callbackRegistry)
    {
        this.options = options;
        this.evaluationChannel = evaluationChannel ?? throw new ArgumentNullException(nameof(evaluationChannel));
        this.observerRegistry = observerRegistry ?? throw new ArgumentNullException(nameof(observerRegistry));
        this.callbackRegistry = callbackRegistry ?? throw new ArgumentNullException(nameof(callbackRegistry));
    }

    #region ISpaceRouter

    void ISpaceRouter<TTuple, TTemplate>.RouteStore(ITupleStore<TTuple> tupleStore)
        => this.tupleStore = tupleStore;

    async ValueTask ISpaceRouter<TTuple, TTemplate>.RouteAction(TupleAction<TTuple> action)
    {
        if (action.AgentId != agentId)
        {
            switch (action.Type)
            {
                case TupleActionType.Insert:
                    {
                        tuples.Add(action.Tuple);
                        await streamChannel.WriteIfNotNull(action.Tuple);
                    }
                    break;
                case TupleActionType.Remove:
                    tuples.Remove(action.Tuple);
                    break;
                case TupleActionType.Clear:
                    tuples.Clear();
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }

    Task ISpaceRouter<TTuple, TTemplate>.RouteTuple(TTuple tuple) => WriteAsync(tuple);
    async ValueTask ISpaceRouter<TTuple, TTemplate>.RouteTemplate(TTemplate template) => await PopAsync(template);

    #endregion

    #region ISpaceAgent

    public Guid Subscribe(ISpaceObserver<TTuple> observer)
        => observerRegistry.Add(observer);

    public void Unsubscribe(Guid observerId)
        => observerRegistry.Remove(observerId);

    public async Task WriteAsync(TTuple tuple)
    {
        ThrowHelpers.EmptyTuple(tuple);

        await tupleStore.Insert(new(agentId, tuple, TupleActionType.Insert));
        await streamChannel.WriteIfNotNull(tuple);

        tuples.Add(tuple);
    }

    public ValueTask EvaluateAsync(Func<Task<TTuple>> evaluation)
    {
        if (evaluation == null) throw new ArgumentNullException(nameof(evaluation));
        return evaluationChannel.Writer.WriteAsync(evaluation);
    }

    public ValueTask<TTuple> PeekAsync(TTemplate template)
    {
        TTuple tuple = tuples.Find(template);
        return new(tuple);
    }

    public async ValueTask PeekAsync(TTemplate template, Func<TTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        TTuple tuple = tuples.Find(template);

        if (tuple.IsEmpty)
        {
            callbackRegistry.Add(template, new(callback, false));
            return;
        }

        await callback(tuple);
    }

    public async ValueTask<TTuple> PopAsync(TTemplate template)
    {
        TTuple tuple = tuples.Find(template);

        if (!tuple.IsEmpty)
        {
            await tupleStore.Remove(new(agentId, tuple, TupleActionType.Remove));
            tuples.Remove(tuple);
        }

        return tuple;
    }

    public async ValueTask PopAsync(TTemplate template, Func<TTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        TTuple tuple = tuples.Find(template);

        if (tuple.IsEmpty)
        {
            callbackRegistry.Add(template, new(callback, true));
            return;
        }

        await callback(tuple);
        await tupleStore.Remove(new(agentId, tuple, TupleActionType.Remove));

        tuples.Remove(tuple);
    }

    public ValueTask<IEnumerable<TTuple>> ScanAsync(TTemplate template)
    {
        List<TTuple> result = tuples.FindAll(template);
        return new(result);
    }

    public async IAsyncEnumerable<TTuple> PeekAsync()
    {
        lock (lockObj)
        {
            if (streamChannel is null)
            {
                streamChannel = Channel.CreateUnbounded<TTuple>(new()
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

        await foreach (TTuple tuple in streamChannel.Reader.ReadAllAsync())
        {
            yield return tuple;
        }
    }

    public ValueTask<int> CountAsync() => new(tuples.Count);

    public async Task ClearAsync()
    {
        await tupleStore.RemoveAll(agentId);
        tuples.Clear();
    }

    #endregion
}
