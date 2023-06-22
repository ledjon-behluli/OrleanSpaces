using Orleans;
using Orleans.Streams;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Observers;
using OrleanSpaces.Continuations;
using System.Diagnostics.CodeAnalysis;
using OrleanSpaces.Tuples;
using OrleanSpaces.Grains;

namespace OrleanSpaces.Agents;

internal sealed class SpaceAgentProvider : ISpaceAgentProvider
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly SpaceAgent agent;
    private bool initialized;

    public SpaceAgentProvider(SpaceAgent agent)
    {
        this.agent = agent;
    }

    public async ValueTask<ISpaceAgent> GetAsync()
    {
        await semaphore.WaitAsync();

        try
        {
            if (!initialized)
            {
                await agent.InitializeAsync();
                initialized = true;
            }
        }
        finally
        {
            semaphore.Release();
        }

        return agent;
    }
}

[ImplicitStreamSubscription(Constants.SpaceStream)]
internal sealed class SpaceAgent : 
    ISpaceAgent,
    ITupleRouter<SpaceTuple, SpaceTemplate>,
    IAsyncObserver<TupleAction<SpaceTuple>>
{
    private Guid agentId = Guid.NewGuid();
    private List<SpaceTuple> tuples = new();

    private readonly IClusterClient client;
    private readonly CallbackRegistry callbackRegistry;
    private readonly EvaluationChannel<SpaceTuple> evaluationChannel;
    private readonly ObserverChannel<SpaceTuple> observerChannel;
    private readonly ObserverRegistry<SpaceTuple> observerRegistry;
    private readonly CallbackChannel<SpaceTuple, SpaceTemplate> callbackChannel;
  
    [AllowNull] private ISpaceGrain grain;

    public SpaceAgent(
        IClusterClient client,
        CallbackRegistry callbackRegistry,
        EvaluationChannel<SpaceTuple> evaluationChannel,
        ObserverChannel<SpaceTuple> observerChannel,
        ObserverRegistry<SpaceTuple> observerRegistry,
        CallbackChannel<SpaceTuple, SpaceTemplate> callbackChannel)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.callbackRegistry = callbackRegistry ?? throw new ArgumentNullException(nameof(callbackRegistry));
        this.evaluationChannel = evaluationChannel ?? throw new ArgumentNullException(nameof(evaluationChannel));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
        this.observerChannel = observerChannel ?? throw new ArgumentNullException(nameof(observerChannel));
        this.observerRegistry = observerRegistry ?? throw new ArgumentNullException(nameof(observerRegistry));
    }

    public async Task InitializeAsync()
    {
        if (!client.IsInitialized)
        {
            await client.Connect();
        }

        grain = client.GetGrain<ISpaceGrain>(Guid.Empty);

        var provider = client.GetStreamProvider(Constants.PubSubProvider);
        var stream = provider.GetStream<TupleAction<SpaceTuple>>(Guid.Empty, Constants.SpaceStream);

        await stream.SubscribeAsync(this);
        tuples = (await grain.GetAsync()).ToList();
    }

    #region IAsyncObserver

    async Task IAsyncObserver<TupleAction<SpaceTuple>>.OnNextAsync(TupleAction<SpaceTuple> action, StreamSequenceToken token)
    {
        await observerChannel.Writer.WriteAsync(action);

        if (action.Type == TupleActionType.Insert)
        {
            if (action.AgentId != agentId)
            {
                tuples.Add(action.Tuple);
            }

            await callbackChannel.Writer.WriteAsync(new(action.Tuple, action.Tuple));
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
        await grain.AddAsync(new(agentId, tuple, TupleActionType.Insert));
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

        if (tuple != SpaceTuple.Empty)
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

        if (tuple != SpaceTuple.Empty)
        {
            await grain.RemoveAsync(new(agentId, tuple, TupleActionType.Delete));
            tuples.Remove(tuple);
        }

        return tuple;
    }

    public async ValueTask PopAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));
     
        SpaceTuple tuple = FindTuple(template);

        if (tuple != SpaceTuple.Empty)
        {
            await callback(tuple);
            await grain.RemoveAsync(new(agentId, tuple, TupleActionType.Delete));

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

        return SpaceTuple.Empty;
    }

    #endregion
}