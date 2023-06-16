using Orleans;
using Orleans.Streams;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Observers;
using System.Diagnostics.CodeAnalysis;
using OrleanSpaces.Tuples.Typed;
using OrleanSpaces.Continuations;
using OrleanSpaces.Tuples;
using OrleanSpaces.Grains;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription("IntStream")]
internal sealed class IntAgent : 
    ISpaceAgent<int, IntTuple, IntTemplate>, 
    IAsyncObserver<StreamAction<IntTuple>>,
    ITupleRouter
{
    private readonly List<IntTuple> tuples = new();

    private readonly IClusterClient client;
    private readonly EvaluationChannel evaluationChannel;
    private readonly CallbackChannel callbackChannel;
    private readonly ObserverChannel observerChannel;
    private readonly CallbackRegistry<int, IntTuple, IntTemplate> callbackRegistry;
    private readonly ObserverRegistry observerRegistry;

    [AllowNull] private IIntGrain grain;

    public IntAgent(
        IClusterClient client,
        EvaluationChannel evaluationChannel,
        CallbackChannel callbackChannel,
        ObserverChannel observerChannel,
        CallbackRegistry<int, IntTuple, IntTemplate> callbackRegistry,
        ObserverRegistry observerRegistry)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.evaluationChannel = evaluationChannel ?? throw new ArgumentNullException(nameof(evaluationChannel));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
        this.observerChannel = observerChannel ?? throw new ArgumentNullException(nameof(observerChannel));
        this.callbackRegistry = callbackRegistry ?? throw new ArgumentNullException(nameof(callbackRegistry));
        this.observerRegistry = observerRegistry ?? throw new ArgumentNullException(nameof(observerRegistry));
    }

    public async Task InitializeAsync()
    {
        if (!client.IsInitialized)
        {
            await client.Connect();
        }

        grain = client.GetGrain<IIntGrain>(Guid.Empty);

        if (observerChannel.IsBeingConsumed)
        {
            var provider = client.GetStreamProvider(Constants.PubSubProvider);
            var stream = provider.GetStream<StreamAction<IntTuple>>(Guid.Empty, "IntStream");

            await stream.SubscribeAsync(this);
        }
    }

    #region IAsyncObserver

    async Task IAsyncObserver<StreamAction<IntTuple>>.OnNextAsync(StreamAction<IntTuple> action, StreamSequenceToken token)
    {
        await observerChannel.TupleWriter.WriteAsync(action.Tuple);
        if (action.Type == StreamActionType.Added)
        {
            await callbackChannel.Writer.WriteAsync(action.Tuple);
        }
    }

    Task IAsyncObserver<StreamAction<IntTuple>>.OnCompletedAsync() => Task.CompletedTask;
    Task IAsyncObserver<StreamAction<IntTuple>>.OnErrorAsync(Exception e) => Task.CompletedTask;

    #endregion

    #region ITupleRouter

    Task ITupleRouter.RouteAsync(ISpaceTuple tuple) => WriteAsync((IntTuple)tuple);
    async ValueTask ITupleRouter.RouteAsync(ISpaceTemplate template) => await PopAsync((IntTemplate)template);

    #endregion

    #region ISpaceAgent

    public Guid Subscribe(ISpaceObserver<int, IntTuple, IntTemplate> observer)
    {
        Helpers.ThrowIfNotBeingConsumed(observerChannel);
        return observerRegistry.Add(observer);
    }

    public void Unsubscribe(Guid observerId)
    {
        Helpers.ThrowIfNotBeingConsumed(observerChannel);
        observerRegistry.Remove(observerId);
    }

    public Task WriteAsync(IntTuple tuple) => grain.AddAsync(tuple);

    public ValueTask EvaluateAsync(Func<Task<IntTuple>> evaluation)
    {
        Helpers.ThrowIfNotBeingConsumed(evaluationChannel);

        if (evaluation == null)
        {
            throw new ArgumentNullException(nameof(evaluation));
        }

        return evaluationChannel.Writer.WriteAsync(evaluation);
    }

    public ValueTask<IntTuple> PeekAsync(IntTemplate template)
    {
        IntTuple tuple = tuples.FindTuple<int, IntTuple, IntTemplate>(template);
        return new(tuple);
    }

    public async ValueTask PeekAsync(IntTemplate template, Func<IntTuple, Task> callback)
    {
        Helpers.ThrowIfNotBeingConsumed(callbackChannel);

        if (callback == null)
        {
            throw new ArgumentNullException(nameof(callback));
        }

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
        Helpers.ThrowIfNotBeingConsumed(callbackChannel);

        if (callback == null)
        {
            throw new ArgumentNullException(nameof(callback));
        }

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
