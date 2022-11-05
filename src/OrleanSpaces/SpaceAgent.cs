using Orleans;
using Orleans.Streams;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Observers;
using OrleanSpaces.Continuations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using OrleanSpaces.Tuples;

namespace OrleanSpaces;

internal sealed class SpaceAgent : ISpaceAgent, ITupleRouter, IAsyncObserver<ITuple>
{
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

        grain = client.GetGrain<ISpaceGrain>(Constants.SpaceGrainId);

        if (observerChannel.IsBeingConsumed)
        {
            var streamId = await grain.ListenAsync();
            var provider = client.GetStreamProvider(Constants.PubSubProvider);
            var stream = provider.GetStream<ITuple>(streamId, Constants.TupleStream);

            await stream.SubscribeAsync(this);
        }
    }

    #region IAsyncObserver

    public async Task OnNextAsync(ITuple tuple, StreamSequenceToken token)
    {
        await observerChannel.Writer.WriteAsync(tuple);
        if (tuple is SpaceTuple spaceTuple)
        {
            await callbackChannel.Writer.WriteAsync(spaceTuple);
        }
    }

    public Task OnCompletedAsync() => Task.CompletedTask;
    public Task OnErrorAsync(Exception e) => Task.CompletedTask;

    #endregion

    #region ITupleRouter

    public async Task RouteAsync(ITuple tuple)
    {
        if (tuple == null)
        {
            throw new ArgumentNullException(nameof(tuple));
        }

        if (tuple is SpaceTuple spaceTuple)
        {
            await WriteAsync(spaceTuple);
            return;
        }
        
        if (tuple is SpaceTemplate spaceTemplate)
        {
            _ = await PopAsync(spaceTemplate);
            return;
        }
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
        => grain.WriteAsync(tuple);

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
        => grain.PeekAsync(template);

    public async ValueTask PeekAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        ThrowIfNotBeingConsumed(callbackChannel);

        if (callback == null)
        {
            throw new ArgumentNullException(nameof(callback));
        }

        SpaceTuple tuple = await grain.PeekAsync(template);

        if (!tuple.IsNull)
        {
            await callback(tuple);
        }
        else
        {
            callbackRegistry.Add(template, new(callback, false));
        }
    }

    public ValueTask<SpaceTuple> PopAsync(SpaceTemplate template)
            => grain.PopAsync(template);

    public async ValueTask PopAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        ThrowIfNotBeingConsumed(callbackChannel);

        if (callback == null)
        {
            throw new ArgumentNullException(nameof(callback));
        }

        SpaceTuple tuple = await grain.PopAsync(template);

        if (!tuple.IsNull)
        {
            await callback(tuple);
        }
        else
        {
            callbackRegistry.Add(template, new(callback, true));
        }
    }

    public ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template)
        => grain.ScanAsync(template);

    public ValueTask<int> CountAsync()
        => grain.CountAsync(null);

    public ValueTask<int> CountAsync(SpaceTemplate template)
        => grain.CountAsync(template);

    private static void ThrowIfNotBeingConsumed(IConsumable consumable, [CallerMemberName] string? methodName = null)
    {
        if (!consumable.IsBeingConsumed)
        {
            throw new InvalidOperationException(
                $"The method '{methodName}' is not available due to '{consumable.GetType().Name}' not having an active consumer. " +
                "This due to the client application not having started the generic host.");
        }
    }

    #endregion
}