using OrleanSpaces.Channels;
using OrleanSpaces.Helpers;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;
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
    private readonly SpaceClientOptions options;
    private readonly EvaluationChannel<TTuple> evaluationChannel;
    private readonly ObserverRegistry<TTuple> observerRegistry;
    private readonly CallbackRegistry<T, TTuple, TTemplate> callbackRegistry;
    private readonly ImmutableArray<StoreTuple<TTuple>> tuples = ImmutableArray<StoreTuple<TTuple>>.Empty; //  chosen for thread safety

    [AllowNull] private IStoreDirector<TTuple> director;
    private Channel<TTuple>? streamChannel;

    public BaseAgent(
        SpaceClientOptions options,
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

    async ValueTask ISpaceRouter<TTuple, TTemplate>.RouteDirector(IStoreDirector<TTuple> director)
    {
        if (options.LoadSpaceContentsUponStartup)
        {
            var tuples = await director.GetAll();
            foreach (var tuple in tuples)
            {
                this.tuples.Add(tuple);
            }
        }

        this.director = director;
    }

    async ValueTask ISpaceRouter<TTuple, TTemplate>.RouteAction(TupleAction<TTuple> action)
    {
        if (action.AgentId != agentId)
        {
            switch (action.Type)
            {
                case TupleActionType.Insert:
                    {
                        tuples.Add(action.StoreTuple);
                        await streamChannel.WriteIfNotNull(action.StoreTuple.Tuple);
                    }
                    break;
                case TupleActionType.Remove:
                    tuples.Remove(action.StoreTuple);
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

        Guid storeId = await director.Insert(new(agentId, new(Guid.Empty, tuple), TupleActionType.Insert));
        await streamChannel.WriteIfNotNull(tuple);

        tuples.Add(new(storeId, tuple));
    }

    public ValueTask EvaluateAsync(Func<Task<TTuple>> evaluation)
    {
        if (evaluation == null) throw new ArgumentNullException(nameof(evaluation));
        return evaluationChannel.Writer.WriteAsync(evaluation);
    }

    public ValueTask<TTuple> PeekAsync(TTemplate template)
    {
        var tuple = tuples.FirstOrDefault(x => template.Matches(x.Tuple));
        return new(tuple.Tuple);
    }

    public async ValueTask PeekAsync(TTemplate template, Func<TTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        var tuple = tuples.FirstOrDefault(x => template.Matches(x.Tuple));
        if (tuple.Tuple.IsEmpty)
        {
            callbackRegistry.Add(template, new(callback, false));
            return;
        }

        await callback(tuple.Tuple);
    }

    public async ValueTask<TTuple> PopAsync(TTemplate template)
    {
        var tuple = tuples.FirstOrDefault(x => template.Matches(x.Tuple));
        if (!tuple.Tuple.IsEmpty)
        {
            await director.Remove(new(agentId, tuple, TupleActionType.Remove));
            tuples.Remove(tuple);
        }

        return tuple.Tuple;
    }

    public async ValueTask PopAsync(TTemplate template, Func<TTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        var tuple = tuples.FirstOrDefault(x => template.Matches(x.Tuple));
        if (tuple.Tuple.IsEmpty)
        {
            callbackRegistry.Add(template, new(callback, true));
            return;
        }

        await callback(tuple.Tuple);
        await director.Remove(new(agentId, tuple, TupleActionType.Remove));

        tuples.Remove(tuple);
    }

    public ValueTask<IEnumerable<TTuple>> ScanAsync(TTemplate template)
    {
        List<TTuple> result = new();

        foreach (var tuple in tuples)
        {
            if (template.Matches(tuple.Tuple))
            {
                result.Add(tuple.Tuple);
            }
        }

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
                    _ = streamChannel.Writer.TryWrite(tuple.Tuple);  // will always be able to write to the channel
                }
            }
        }

        await foreach (TTuple tuple in streamChannel.Reader.ReadAllAsync())
        {
            yield return tuple;
        }
    }

    public ValueTask<int> CountAsync() => new(tuples.Length);

    public async Task ClearAsync()
    {
        await director.RemoveAll(agentId);
        tuples.Clear();
    }

    public async Task ReloadAsync()
    {
        var tuples = await director.GetAll();
        this.tuples.Clear();

        foreach (var tuple in tuples)
        {
            this.tuples.Add(tuple);
        }
    }

    #endregion
}
