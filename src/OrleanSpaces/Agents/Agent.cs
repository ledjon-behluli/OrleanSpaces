using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Continuations;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Helpers;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Agents;

internal class AgentProvider<T, TTuple, TTemplate> : ISpaceAgentProvider<T, TTuple, TTemplate>
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
    where TTemplate : ISpaceTemplate<T>
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly ITupleStore<TTuple> store;
    private readonly ISpaceAgent<T, TTuple, TTemplate> agent;

    private bool initialized;

    public AgentProvider(
        ITupleStore<TTuple> store,
        ISpaceAgent<T, TTuple, TTemplate> agent)
    {
        this.store = store;
        this.agent = agent;
    }

    public async ValueTask<ISpaceAgent<T, TTuple, TTemplate>> GetAsync()
    {
        if (initialized)
        {
            return agent;
        }

        await semaphore.WaitAsync();

        try
        {
            await agent.InitializeAsync(store);
            initialized = true;
        }
        finally
        {
            semaphore.Release();
        }

        return agent;
    }
}

internal class Agent<T, TTuple, TTemplate> : 
    ISpaceAgent<T, TTuple, TTemplate>,
    ITupleRouter<TTuple, TTemplate>,
    IAsyncObserver<TupleAction<TTuple>>
    where T : unmanaged
    where TTuple : struct, ISpaceTuple<T>
    where TTemplate : struct, ISpaceTemplate<T>
{
    private readonly Guid agentId = Guid.NewGuid();
    private readonly IClusterClient client;
    private readonly EvaluationChannel<TTuple> evaluationChannel;
    private readonly ObserverChannel<TTuple> observerChannel;
    private readonly ObserverRegistry<TTuple> observerRegistry;
    private readonly CallbackChannel<TTuple> callbackChannel;
    private readonly CallbackRegistry<T, TTuple, TTemplate> callbackRegistry;

    [AllowNull] private ITupleStore<TTuple> store;
    private List<TTuple> tuples = new();

    public Agent(
        IClusterClient client,
        EvaluationChannel<TTuple> evaluationChannel,
        ObserverChannel<TTuple> observerChannel,
        ObserverRegistry<TTuple> observerRegistry,
        CallbackChannel<TTuple> callbackChannel,
        CallbackRegistry<T, TTuple, TTemplate> callbackRegistry)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.evaluationChannel = evaluationChannel ?? throw new ArgumentNullException(nameof(evaluationChannel));
        this.observerChannel = observerChannel ?? throw new ArgumentNullException(nameof(observerChannel));
        this.observerRegistry = observerRegistry ?? throw new ArgumentNullException(nameof(observerRegistry));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
        this.callbackRegistry = callbackRegistry ?? throw new ArgumentNullException(nameof(callbackRegistry));
    }

    async Task ISpaceAgent<T, TTuple, TTemplate>.InitializeAsync(ITupleStore<TTuple> store)
    {
        tuples = (await store.GetAll()).ToList();
        StreamId streamId = await store.GetStreamId();
        await client.SubscribeAsync(this, streamId);

        this.store = store;
    }

    #region IAsyncObserver

    async Task IAsyncObserver<TupleAction<TTuple>>.OnNextAsync(TupleAction<TTuple> action, StreamSequenceToken? token)
    {
        await observerChannel.Writer.WriteAsync(action);

        if (action.Type == TupleActionType.Insert)
        {
            if (action.AgentId != agentId)
            {
                tuples.Add(action.Tuple);
            }

            await callbackChannel.Writer.WriteAsync(action.Tuple);
        }
        else
        {
            if (action.AgentId != agentId)
            {
                tuples.Remove(action.Tuple);
            }
        }
    }

    Task IAsyncObserver<TupleAction<TTuple>>.OnCompletedAsync() => Task.CompletedTask;
    Task IAsyncObserver<TupleAction<TTuple>>.OnErrorAsync(Exception e) => Task.CompletedTask;

    #endregion

    #region ITupleRouter

    Task ITupleRouter<TTuple, TTemplate>.RouteAsync(TTuple tuple) => WriteAsync(tuple);
    async ValueTask ITupleRouter<TTuple, TTemplate>.RouteAsync(TTemplate template) => await PopAsync(template);

    #endregion

    #region ISpaceAgent

    public Guid Subscribe(ISpaceObserver<TTuple> observer)
        => observerRegistry.Add(observer);

    public void Unsubscribe(Guid observerId)
        => observerRegistry.Remove(observerId);

    public async Task WriteAsync(TTuple tuple)
    {
        ThrowHelpers.EmptyTuple(tuple);
        await store.Insert(new(agentId, tuple, TupleActionType.Insert));
        tuples.Add(tuple);
    }

    public ValueTask EvaluateAsync(Func<Task<TTuple>> evaluation)
    {
        if (evaluation == null) throw new ArgumentNullException(nameof(evaluation));
        return evaluationChannel.Writer.WriteAsync(evaluation);
    }

    public ValueTask<TTuple> PeekAsync(TTemplate template)
    {
        TTuple tuple = tuples.FindTuple<T, TTuple, TTemplate>(template);
        return new(tuple);
    }

    public async ValueTask PeekAsync(TTemplate template, Func<TTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        TTuple tuple = tuples.FindTuple<T, TTuple, TTemplate>(template);

        if (tuple.Length > 0)
        {
            await callback(tuple);
        }
        else
        {
            callbackRegistry.Add(template, new(callback, false));
        }
    }

    public async ValueTask<TTuple> PopAsync(TTemplate template)
    {
        TTuple tuple = tuples.FindTuple<T, TTuple, TTemplate>(template);

        if (tuple.Length > 0)
        {
            await store.Remove(new(agentId, tuple, TupleActionType.Remove));
            tuples.Remove(tuple);
        }

        return tuple;
    }

    public async ValueTask PopAsync(TTemplate template, Func<TTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        TTuple tuple = tuples.FindTuple<T, TTuple, TTemplate>(template);

        if (tuple.Length > 0)
        {
            await callback(tuple);
            await store.Remove(new(agentId, tuple, TupleActionType.Remove));

            tuples.Remove(tuple);
        }
        else
        {
            callbackRegistry.Add(template, new(callback, true));
        }
    }

    public ValueTask<IEnumerable<TTuple>> ScanAsync(TTemplate template)
        => new(tuples.FindAllTuples<T, TTuple, TTemplate>(template));

    public ValueTask<int> CountAsync() => new(tuples.Count);

    #endregion
}
