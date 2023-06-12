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
    ITupleRouter<int, IntTuple, IntTemplate>,
    IAsyncObserver<StreamAction<IntTuple>>
{
    private readonly List<IntTuple> tuples = new();

    private readonly IClusterClient client;
    private readonly EvaluationChannel<int, IntTuple, IntTemplate> evaluationChannel;
    private readonly CallbackChannel<int, IntTuple, IntTemplate> callbackChannel;
    private readonly ObserverChannel<int, IntTuple, IntTemplate> observerChannel;
    private readonly CallbackRegistry<int, IntTuple, IntTemplate> callbackRegistry;
    private readonly ObserverRegistry<int, IntTuple, IntTemplate> observerRegistry;

    [AllowNull] private IIntGrain grain;

    public IntAgent(
        IClusterClient client,
        EvaluationChannel<int, IntTuple, IntTemplate> evaluationChannel,
        CallbackChannel<int, IntTuple, IntTemplate> callbackChannel,
        ObserverChannel<int, IntTuple, IntTemplate> observerChannel,
        CallbackRegistry<int, IntTuple, IntTemplate> callbackRegistry,
        ObserverRegistry<int, IntTuple, IntTemplate> observerRegistry)
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
            await callbackChannel.TupleWriter.WriteAsync(action.Tuple);
        }
    }

    Task IAsyncObserver<StreamAction<IntTuple>>.OnCompletedAsync() => Task.CompletedTask;
    Task IAsyncObserver<StreamAction<IntTuple>>.OnErrorAsync(Exception e) => Task.CompletedTask;

    #endregion

    #region ITupleRouter

    Task ITupleRouter<int, IntTuple, IntTemplate>.RouteAsync(IntTuple tuple)
        => WriteAsync(tuple);

    async ValueTask ITupleRouter<int, IntTuple, IntTemplate>.RouteAsync(IntTemplate template)
        => await PopAsync(template);

    #endregion

    #region ISpaceAgent

    public Task WriteAsync(IntTuple tuple) => grain.AddAsync(tuple);

    public ValueTask EvaluateAsync(Func<Task<IntTuple>> evaluation)
    {
        evaluationChannel.ThrowIfNotBeingConsumed();

        if (evaluation == null)
        {
            throw new ArgumentNullException(nameof(evaluation));
        }

        return evaluationChannel.TupleWriter.WriteAsync(evaluation);
    }

    public ValueTask<IntTuple> PeekAsync(IntTemplate template)
    {
        IntTuple tuple = tuples.FindTuple<int, IntTuple, IntTemplate>(template);
        return new(tuple);
    }

    public async ValueTask PeekAsync(IntTemplate template, Func<IntTuple, Task> callback)
    {
        callbackChannel.ThrowIfNotBeingConsumed();

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
        callbackChannel.ThrowIfNotBeingConsumed();

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
