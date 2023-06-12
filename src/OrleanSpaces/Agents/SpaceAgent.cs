using Orleans;
using Orleans.Streams;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Observers;
using OrleanSpaces.Continuations;
using System.Diagnostics.CodeAnalysis;
using OrleanSpaces.Tuples;
using System.Runtime.CompilerServices;
using OrleanSpaces.Grains;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription("SpaceStream")]
internal sealed class SpaceAgent : 
    ISpaceAgent, 
    ITupleRouter, 
    IAsyncObserver<StreamAction<SpaceTuple>>
{
    private readonly List<SpaceTuple> tuples = new();

    private readonly IClusterClient client;
    private readonly EvaluationChannel evaluationChannel;
    private readonly CallbackChannel callbackChannel;
    private readonly ObserverChannel observerChannel;
    private readonly CallbackRegistry callbackRegistry;
    private readonly ObserverRegistry observerRegistry;

    [AllowNull] private ISpaceGrain grain;

    public SpaceAgent(
        IClusterClient client,
        EvaluationChannel evaluationChannel,
        CallbackChannel callbackChannel,
        ObserverChannel observerChannel,
        CallbackRegistry callbackRegistry,
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

        grain = client.GetGrain<ISpaceGrain>(Guid.Empty);

        if (observerChannel.IsBeingConsumed)
        {
            var provider = client.GetStreamProvider(Constants.PubSubProvider);
            var stream = provider.GetStream<StreamAction<SpaceTuple>>(Guid.Empty, "SpaceStream");

            await stream.SubscribeAsync(this);
        }
    }

    #region IAsyncObserver

    async Task IAsyncObserver<StreamAction<SpaceTuple>>.OnNextAsync(StreamAction<SpaceTuple> action, StreamSequenceToken token)
    {
        await observerChannel.TupleWriter.WriteAsync(action.Tuple);
        if (action.Type == StreamActionType.Added)
        {
            await callbackChannel.Writer.WriteAsync(action.Tuple);
        }
    }

    Task IAsyncObserver<StreamAction<SpaceTuple>>.OnCompletedAsync() => Task.CompletedTask;
    Task IAsyncObserver<StreamAction<SpaceTuple>>.OnErrorAsync(Exception e) => Task.CompletedTask;

    #endregion

    #region ITupleRouter

    async Task ITupleRouter.RouteAsync(ISpaceElement element)
    {
        if (element is SpaceTuple tuple)
        {
            await WriteAsync(tuple);
        }

        if (element is SpaceTemplate template)
        {
            await PopAsync(template);
        }

        Helpers.ThrowNotSupported(element);
    }

    #endregion

    #region ISpaceAgent

    public Guid Subscribe(ISpaceObserver observer)
    {
        ThrowIfNotBeingConsumed(observerChannel);
        return observerRegistry.Add(observer);
    }

    public void Unsubscribe(Guid observerId)
    {
        ThrowIfNotBeingConsumed(observerChannel);
        observerRegistry.Remove(observerId);
    }

    public Task WriteAsync(SpaceTuple tuple)
        => grain.AddAsync(tuple);

    public ValueTask EvaluateAsync(Func<Task<SpaceTuple>> evaluation)
    {
        ThrowIfNotBeingConsumed(evaluationChannel);

        if (evaluation == null)
        {
            throw new ArgumentNullException(nameof(evaluation));
        }

        return evaluationChannel.Writer.WriteAsync(evaluation);
    }

    public ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template)
    {
        SpaceTuple tuple = FindTuple(template);
        return new(tuple);
    }

    public async ValueTask PeekAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        ThrowIfNotBeingConsumed(callbackChannel);

        if (callback == null)
        {
            throw new ArgumentNullException(nameof(callback));
        }

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
            await grain.RemoveAsync(tuple);
            tuples.Remove(tuple);
        }

        return tuple;
    }

    public async ValueTask PopAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        ThrowIfNotBeingConsumed(callbackChannel);

        if (callback == null)
        {
            throw new ArgumentNullException(nameof(callback));
        }

        SpaceTuple tuple = FindTuple(template);

        if (tuple != SpaceTuple.Empty)
        {
            await callback(tuple);
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

    private static void ThrowIfNotBeingConsumed(IConsumable consumable, [CallerMemberName] string? methodName = null)
    {
        if (!consumable.IsBeingConsumed)
        {
            throw new InvalidOperationException(
                $"The method '{methodName}' is not available due to '{consumable.GetType().Name}' not having an active consumer. " +
                "This due to the client application not having started the generic host.");
        }
    }

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