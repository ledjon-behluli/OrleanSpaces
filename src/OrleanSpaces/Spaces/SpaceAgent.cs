using Orleans;
using Orleans.Streams;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Spaces;

internal class SpaceAgent : IAsyncObserver<SpaceTuple>
{
    private readonly IClusterClient client;

    public SpaceAgent(IClusterClient client)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task ActivateAsync()
    {
        var streamId = await client.GetSpaceGrain().ConnectAsync();
        var provider = client.GetStreamProvider(Constants.StreamProviderName);
        var stream = provider.GetStream<SpaceTuple>(streamId, Constants.StreamNamespace);

        await stream.SubscribeAsync(this);
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

    public Task OnErrorAsync(Exception e) => Task.CompletedTask;
}