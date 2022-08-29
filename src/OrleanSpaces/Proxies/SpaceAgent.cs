﻿using Orleans;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Primitives;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Observers;
using Orleans.Streams;
using OrleanSpaces.Utils;
using OrleanSpaces.Grains;

namespace OrleanSpaces.Proxies;

internal class SpaceAgent : IAsyncObserver<SpaceTuple>, ISpaceChannel
{
    private readonly ILogger<SpaceAgent> logger;
    private readonly IClusterClient client;
    private readonly ICallbackRegistry callbackRegistry;
    private readonly IObserverRegistry observerRegistry;

#nullable disable
    private ISpaceGrain grain;
#nullable enable

    public SpaceAgent(
        ILogger<SpaceAgent> logger,
        IClusterClient client,
        ICallbackRegistry callbackRegistry,
        IObserverRegistry observerRegistry)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.callbackRegistry = callbackRegistry ?? throw new ArgumentNullException(nameof(callbackRegistry));
        this.observerRegistry = observerRegistry ?? throw new ArgumentNullException(nameof(observerRegistry));
    }

    #region Init

    public async Task InitAsync()
    {
        logger.LogDebug("Initializing space agent.");

        await ConnectToCluster();
        await SubscribeToStream();

        logger.LogDebug("Space agent initialized.");
    }

    private async Task ConnectToCluster()
    {
        if (!client.IsInitialized)
        {
            logger.LogDebug("Establishing cluster connection.");

            await client.Connect();

            logger.LogDebug("Cluster connection established.");
        }
    }

    private async Task SubscribeToStream()
    {
        logger.LogDebug("Establishing space stream connection.");

        grain = client.GetGrain<ISpaceGrain>(Guid.Empty);

        var streamId = await grain.ConnectAsync();
        var provider = client.GetStreamProvider(StreamNames.PubSubProvider);
        var stream = provider.GetStream<SpaceTuple>(streamId, StreamNamespaces.TupleWrite);

        await stream.SubscribeAsync(this);

        logger.LogDebug("Space stream connection established.");
    }

    #endregion

    #region IAsyncObserver

    public async Task OnNextAsync(SpaceTuple tuple, StreamSequenceToken token)
    {
        await CallbackChannel.Writer.WriteAsync(tuple);
        await ObserverChannel.Writer.WriteAsync(tuple);

        logger.LogDebug("Forwarded tuple {SpaceTuple} channels for processing.", tuple);
    }

    public Task OnCompletedAsync()
    {
        CallbackChannel.Writer.Complete();
        ObserverChannel.Writer.Complete();

        logger.LogDebug("Marked channels as 'completed' since {Namespace} stream closed.", StreamNamespaces.TupleWrite);

        return Task.CompletedTask;
    }

    public Task OnErrorAsync(Exception e)
    {
        logger.LogError(e, e.Message);
        return Task.CompletedTask;
    }

    #endregion

    #region ISpaceClient

    public ObserverRef Subscribe(ISpaceObserver observer)
        => new(observerRegistry.Register(observer), observer);

    public void Unsubscribe(ObserverRef @ref)
        => observerRegistry.Deregister(@ref.Observer);

    public async Task WriteAsync(SpaceTuple tuple)
        => await grain.WriteAsync(tuple);

    public async Task EvaluateAsync(Func<SpaceTuple> func)
        => await grain.EvaluateAsync(LambdaSerializer.Serialize(func));

    public async ValueTask<SpaceTuple?> PeekAsync(SpaceTemplate template)
        => await grain.PeekAsync(template);

    public async ValueTask PeekAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        SpaceTuple? tuple = await grain.PeekAsync(template);

        if (tuple != null)
        {
            await callback(tuple);
        }
        else
        {
            callbackRegistry.Register(template, callback);
        }
    }

    public async Task<SpaceTuple?> PopAsync(SpaceTemplate template)
         => await grain.PopAsync(template);

    public async Task PopAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        SpaceTuple? tuple = await grain.PopAsync(template);

        if (tuple != null)
        {
            await callback(tuple);
        }
        else
        {
            callbackRegistry.Register(template, callback);
        }
    }

    public async ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template)
         => await grain.ScanAsync(template);

    public async ValueTask<int> CountAsync()
         => await grain.CountAsync();

    public async ValueTask<int> CountAsync(SpaceTemplate template)
         => await grain.CountAsync(template);

    #endregion
}