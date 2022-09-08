using Orleans;
using Orleans.Streams;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests;

public class SpaceGrainTests : IClassFixture<ClusterFixture>
{
    private readonly IClusterClient client;
    private readonly ISpaceGrain grain;

    public SpaceGrainTests(ClusterFixture fixture)
    {
        client = fixture.Client;
        grain = fixture.Client.GetGrain<ISpaceGrain>(Guid.Empty);
    }

    [Fact]
    public async Task Should_Notify_On_Space_Emptying()
    {
        SpaceTuple tuple1 = SpaceTuple.Create(1);
        SpaceTuple tuple2 = SpaceTuple.Create((1, "a"));

        await grain.WriteAsync(tuple1);
        await grain.WriteAsync(tuple2);

        var streamId = await grain.ListenAsync();
        var provider = client.GetStreamProvider(StreamNames.PubSubProvider);
        var stream = provider.GetStream<SpaceTuple>(streamId, StreamNamespaces.TupleWrite);

        LocalObserver observer = new();
        await stream.SubscribeAsync(observer);

        await grain.PopAsync(SpaceTemplate.Create(tuple1));
        Assert.False(observer.SpaceEmptiedReceived);

        await grain.PopAsync(SpaceTemplate.Create(tuple2));
        Assert.True(observer.SpaceEmptiedReceived);
    }

    private class LocalObserver : IAsyncObserver<SpaceTuple>
    {
        public bool SpaceEmptiedReceived { get; private set; }

        public Task OnNextAsync(SpaceTuple tuple, StreamSequenceToken token)
        {
            if (tuple.IsEmpty)
            {
                SpaceEmptiedReceived = true;
            }

            return Task.CompletedTask;
        }

        public Task OnCompletedAsync() => Task.CompletedTask;
        public Task OnErrorAsync(Exception ex) => Task.CompletedTask;
    }
}
