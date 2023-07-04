using Orleans.Streams;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Observers;
using OrleanSpaces.Continuations;
using System.Diagnostics.CodeAnalysis;
using OrleanSpaces.Tuples;
using OrleanSpaces.Grains;
using OrleanSpaces.Helpers;
using Orleans.Runtime;

namespace OrleanSpaces.Agents;

internal sealed class SpaceAgentProvider : ISpaceAgentProvider
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly IClusterClient client;
    private readonly SpaceAgent agent;

    private bool initialized;

    public SpaceAgentProvider(
        IClusterClient client,
        SpaceAgent agent)
    {
        this.client = client;
        this.agent = agent;
    }

    public async ValueTask<ISpaceAgent> GetAsync()
    {
        if (initialized)
        {
            return agent;
        }

        await semaphore.WaitAsync();

        try
        {
            var grain = client.GetGrain<ISpaceGrain>(ISpaceGrain.Key);
            await agent.InitializeAsync(grain);
            initialized = true;
        }
        finally
        {
            semaphore.Release();
        }

        return agent;
    }
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class SpaceAgent : 
    ISpaceAgent,
    ITupleRouter<SpaceTuple, SpaceTemplate>,
    IAsyncObserver<TupleAction<SpaceTuple>>
{
    private readonly Guid agentId = Guid.NewGuid();
    private readonly IClusterClient client;
    private readonly CallbackRegistry callbackRegistry;
    private readonly EvaluationChannel<SpaceTuple> evaluationChannel;
    private readonly ObserverChannel<SpaceTuple> observerChannel;
    private readonly ObserverRegistry<SpaceTuple> observerRegistry;
    private readonly CallbackChannel<SpaceTuple> callbackChannel;
  
    [AllowNull] private ISpaceGrain grain;
    private List<SpaceTuple> tuples = new();

    public SpaceAgent(
        IClusterClient client,
        CallbackRegistry callbackRegistry,
        EvaluationChannel<SpaceTuple> evaluationChannel,
        ObserverChannel<SpaceTuple> observerChannel,
        ObserverRegistry<SpaceTuple> observerRegistry,
        CallbackChannel<SpaceTuple> callbackChannel)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.callbackRegistry = callbackRegistry ?? throw new ArgumentNullException(nameof(callbackRegistry));
        this.evaluationChannel = evaluationChannel ?? throw new ArgumentNullException(nameof(evaluationChannel));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
        this.observerChannel = observerChannel ?? throw new ArgumentNullException(nameof(observerChannel));
        this.observerRegistry = observerRegistry ?? throw new ArgumentNullException(nameof(observerRegistry));
    }

    public async Task InitializeAsync(ISpaceGrain grain)
    {
        tuples = (await grain.GetAll()).ToList();
        StreamId streamId = await grain.GetStreamId();
        await client.SubscribeAsync(this, streamId);

        this.grain = grain;
    }

    #region IAsyncObserver

    async Task IAsyncObserver<TupleAction<SpaceTuple>>.OnNextAsync(TupleAction<SpaceTuple> action, StreamSequenceToken? token)
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

    Task IAsyncObserver<TupleAction<SpaceTuple>>.OnCompletedAsync() => Task.CompletedTask;
    Task IAsyncObserver<TupleAction<SpaceTuple>>.OnErrorAsync(Exception e) => Task.CompletedTask;

    #endregion

    #region ITupleRouter

    Task ITupleRouter<SpaceTuple, SpaceTemplate>.RouteAsync(SpaceTuple tuple) => WriteAsync(tuple);
    async ValueTask ITupleRouter<SpaceTuple, SpaceTemplate>.RouteAsync(SpaceTemplate template) => await PopAsync(template);

    #endregion

    #region ISpaceAgent

    public Guid Subscribe(ISpaceObserver<SpaceTuple> observer)
        => observerRegistry.Add(observer);

    public void Unsubscribe(Guid observerId)
        => observerRegistry.Remove(observerId);

    public async Task WriteAsync(SpaceTuple tuple)
    {
        ThrowHelpers.EmptyTuple(tuple);
        await grain.Insert(new(agentId, tuple, TupleActionType.Insert));
        tuples.Add(tuple);
    }

    public ValueTask EvaluateAsync(Func<Task<SpaceTuple>> evaluation)
    {
        if (evaluation == null) throw new ArgumentNullException(nameof(evaluation));
        return evaluationChannel.Writer.WriteAsync(evaluation);
    }

    public ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template)
    {
        SpaceTuple tuple = FindTuple(template);
        return new(tuple);
    }

    public async ValueTask PeekAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        SpaceTuple tuple = FindTuple(template);

        if (tuple.Length > 0)
        {
            await callback(tuple);
        }
        else
        {
            callbackRegistry.Add(template, new(callback, false));
        }
    }

    public async ValueTask<SpaceTuple> PopAsync(SpaceTemplate template)
    {
        SpaceTuple tuple = FindTuple(template);

        if (tuple.Length > 0)
        {
            await grain.Remove(new(agentId, tuple, TupleActionType.Remove));
            tuples.Remove(tuple);
        }

        return tuple;
    }

    public async ValueTask PopAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));
     
        SpaceTuple tuple = FindTuple(template);

        if (tuple.Length > 0)
        {
            await callback(tuple);
            await grain.Remove(new(agentId, tuple, TupleActionType.Remove));

            tuples.Remove(tuple);
        }
        else
        {
            callbackRegistry.Add(template, new(callback, true));
        }
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

    public ValueTask<int> CountAsync() => new(tuples.Count);

    private SpaceTuple FindTuple(SpaceTemplate template)
    {
        foreach (var tuple in tuples)
        {
            if (template.Matches(tuple))
            {
                return tuple;
            }
        }

        return new();
    }

    #endregion
}