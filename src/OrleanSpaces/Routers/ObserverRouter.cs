using Orleans;
using Orleans.Streams;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Primitives;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;

namespace OrleanSpaces.Routers;

internal class ObserverRouter : IAsyncObserver<SpaceTuple>
{
    private readonly ILogger<ObserverRouter> logger;

    public ObserverRouter(ILogger<ObserverRouter> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InitializeAsync(IClusterClient client)
    {
        logger.LogDebug("{Service} initialization started.", nameof(ObserverRouter));

        var streamId = await client.GetGrain<ISpaceGrain>(Guid.Empty).ConnectAsync();
        var provider = client.GetStreamProvider(StreamNames.PubSubProvider);
        var stream = provider.GetStream<SpaceTuple>(streamId, StreamNamespaces.TupleWrite);

        await stream.SubscribeAsync(this);

        logger.LogDebug("{Service} initialized successfully.", nameof(ObserverRouter));
    }

    public async Task OnNextAsync(SpaceTuple tuple, StreamSequenceToken token)
    {
        await CallbackChannel.Writer.WriteAsync(tuple);
        await ObserverChannel.Writer.WriteAsync(tuple);
    }

    public Task OnCompletedAsync()
    {
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