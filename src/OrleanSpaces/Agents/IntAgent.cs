using Orleans;
using Orleans.Streams;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Observers;
using System.Diagnostics.CodeAnalysis;
using OrleanSpaces.Tuples.Typed;
using OrleanSpaces.Continuations;
using OrleanSpaces.Grains;

namespace OrleanSpaces.Agents;

internal sealed class IntAgentProvider : ISpaceAgentProvider<int, IntTuple, IntTemplate>
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly IntAgent agent;
    private bool initialized;

    public IntAgentProvider(IntAgent agent)
    {
        this.agent = agent;
    }

    public async ValueTask<ISpaceAgent<int, IntTuple, IntTemplate>> GetAsync()
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

[ImplicitStreamSubscription(Constants.IntStream)]
internal sealed class IntAgent : 
    ISpaceAgent<int, IntTuple, IntTemplate>, 
    ITupleRouter<IntTuple, IntTemplate>,
    IAsyncObserver<TupleAction<IntTuple>>
{
    private readonly List<IntTuple> tuples = new();

    private readonly IClusterClient client;
    private readonly EvaluationChannel<IntTuple> evaluationChannel;
    private readonly ObserverChannel<IntTuple> observerChannel;
    private readonly ObserverRegistry<IntTuple> observerRegistry;
    private readonly CallbackChannel<IntTuple, IntTemplate> callbackChannel;
    private readonly CallbackRegistry<int, IntTuple, IntTemplate> callbackRegistry;

    [AllowNull] private IIntGrain grain;

    public IntAgent(
        IClusterClient client,
        EvaluationChannel<IntTuple> evaluationChannel,
        ObserverChannel<IntTuple> observerChannel,   
        ObserverRegistry<IntTuple> observerRegistry,
        CallbackChannel<IntTuple, IntTemplate> callbackChannel,
        CallbackRegistry<int, IntTuple, IntTemplate> callbackRegistry)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.evaluationChannel = evaluationChannel ?? throw new ArgumentNullException(nameof(evaluationChannel));
        this.observerChannel = observerChannel ?? throw new ArgumentNullException(nameof(observerChannel));
        this.observerRegistry = observerRegistry ?? throw new ArgumentNullException(nameof(observerRegistry));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
        this.callbackRegistry = callbackRegistry ?? throw new ArgumentNullException(nameof(callbackRegistry));
    }

    public async Task InitializeAsync()
    {
        if (!client.IsInitialized)
        {
            await client.Connect();
        }

        grain = client.GetGrain<IIntGrain>(Guid.Empty);

        var provider = client.GetStreamProvider(Constants.PubSubProvider);
        var stream = provider.GetStream<TupleAction<IntTuple>>(Guid.Empty, Constants.IntStream);

        await stream.SubscribeAsync(this);
    }

    #region IAsyncObserver

    async Task IAsyncObserver<TupleAction<IntTuple>>.OnNextAsync(TupleAction<IntTuple> action, StreamSequenceToken token)
    {
        await observerChannel.Writer.WriteAsync(action);

        if (action.Type == TupleActionType.Added)
        {
            tuples.Add(action.Tuple);
            await callbackChannel.Writer.WriteAsync(new(action.Tuple, action.Tuple));
        }
        else
        {
            tuples.Remove(action.Tuple);
        }
    }

    Task IAsyncObserver<TupleAction<IntTuple>>.OnCompletedAsync() => Task.CompletedTask;
    Task IAsyncObserver<TupleAction<IntTuple>>.OnErrorAsync(Exception e) => Task.CompletedTask;

    #endregion

    #region ITupleRouter

    Task ITupleRouter<IntTuple, IntTemplate>.RouteAsync(IntTuple tuple) => WriteAsync(tuple);
    async ValueTask ITupleRouter<IntTuple, IntTemplate>.RouteAsync(IntTemplate template) => await PopAsync(template);

    #endregion

    #region ISpaceAgent

    public Guid Subscribe(ISpaceObserver<IntTuple> observer)
        => observerRegistry.Add(observer);

    public void Unsubscribe(Guid observerId)
        => observerRegistry.Remove(observerId);

    public Task WriteAsync(IntTuple tuple) => grain.AddAsync(tuple);

    public ValueTask EvaluateAsync(Func<Task<IntTuple>> evaluation)
    {
        if (evaluation == null) throw new ArgumentNullException(nameof(evaluation));
        return evaluationChannel.Writer.WriteAsync(evaluation);
    }

    public ValueTask<IntTuple> PeekAsync(IntTemplate template)
    {
        IntTuple tuple = tuples.FindTuple<int, IntTuple, IntTemplate>(template);
        return new(tuple);
    }

    public async ValueTask PeekAsync(IntTemplate template, Func<IntTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        IntTuple tuple = tuples.FindTuple<int, IntTuple, IntTemplate>(template);

        if (tuple != IntTuple.Empty)
        {
            await callback(tuple);
        }
        else
        {
            callbackRegistry.Add(template, new(callback, false));
        }
    }

    public async ValueTask<IntTuple> PopAsync(IntTemplate template)
    {
        IntTuple tuple = tuples.FindTuple<int, IntTuple, IntTemplate>(template);

        if (tuple != IntTuple.Empty)
        {
            await grain.RemoveAsync(tuple);
            tuples.Remove(tuple);
        }

        return tuple;
    }

    public async ValueTask PopAsync(IntTemplate template, Func<IntTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        IntTuple tuple = tuples.FindTuple<int, IntTuple, IntTemplate>(template);

        if (tuple != IntTuple.Empty)
        {
            await callback(tuple);
            await grain.RemoveAsync(tuple);

            tuples.Remove(tuple);
        }
        else
        {
            callbackRegistry.Add(template, new(callback, false));
        }
    }

    public ValueTask<IEnumerable<IntTuple>> ScanAsync(IntTemplate template)
        => new(tuples.FindAllTuples<int, IntTuple, IntTemplate>(template));

    public ValueTask<int> CountAsync() => new(tuples.Count);

    #endregion
}
