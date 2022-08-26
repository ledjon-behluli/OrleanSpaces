using Orleans;
using Orleans.Streams;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Grains;

namespace OrleanSpaces.Spaces;

internal class SpaceAgent : IAsyncObserver<SpaceTuple>
{
    private readonly ILogger<SpaceAgent> logger;

    public SpaceAgent(ILogger<SpaceAgent> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InitializeAsync(IClusterClient client)
    {
        logger.LogDebug("Space agent initialization started.");

        var streamId = await client.GetGrain<ISpaceGrain>(Guid.Empty).ConnectAsync();
        var provider = client.GetStreamProvider(StreamNames.PubSubProvider);
        var stream = provider.GetStream<SpaceTuple>(streamId, StreamNamespaces.TupleWrite);

        await stream.SubscribeAsync(this);

        logger.LogDebug("Space agent initialized successfully.");
    }

    public async Task OnNextAsync(SpaceTuple tuple, StreamSequenceToken token)
    {
        logger.LogDebug("Space agent received new tuple {SpaceTuple}. Forwarding tuple to the internal channels...", tuple);

        await CallbackChannel.Writer.WriteAsync(tuple);
        await ObserverChannel.Writer.WriteAsync(tuple);
    }

    public Task OnCompletedAsync()
    {
        logger.LogDebug("Stream completed - Closing internal channels.");

        CallbackChannel.Writer.Complete();
        ObserverChannel.Writer.Complete();

        return Task.CompletedTask;
    }

    public Task OnErrorAsync(Exception e)
    {
        logger.LogError(e, e.Message);
        return Task.CompletedTask;
    }
}