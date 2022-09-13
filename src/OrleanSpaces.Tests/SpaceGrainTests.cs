using Orleans;
using Orleans.Streams;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests;

// The majority of grain methods are tested via the agent tests. Here we test only some edge cases.
public class SpaceGrainTests : IAsyncLifetime, IClassFixture<ClusterFixture>
{
    private readonly IClusterClient client;
    private readonly ISpaceGrain grain;
    private readonly AsyncObserver observer;

    private IAsyncStream<SpaceTuple> stream;
    private Guid streamId;

    public SpaceGrainTests(ClusterFixture fixture)
    {
        observer = new();
        client = fixture.Client;
        grain = fixture.Client.GetGrain<ISpaceGrain>(Guid.Empty);
    }

    public async Task InitializeAsync()
    {
        streamId = await grain.ListenAsync();
        var provider = client.GetStreamProvider(StreamNames.PubSubProvider);
        stream = provider.GetStream<SpaceTuple>(streamId, StreamNamespaces.Tuple);
        await stream.SubscribeAsync(observer);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Should_Get_Stream_Id()
    {
        Guid id = await grain.ListenAsync();
        Assert.Equal(streamId, id);
    }

    [Fact]
    public async Task Should_Notify_Observer_On_Space_Emptying()
    {
        SpaceTuple tuple1 = new(1);
        SpaceTuple tuple2 = new((1, "a"));

        await grain.WriteAsync(tuple1);
        await grain.WriteAsync(tuple2);

        await grain.PopAsync(tuple1);
        Assert.False(observer.SpaceEmptiedReceived);

        await grain.PopAsync(tuple2);
        Assert.True(observer.SpaceEmptiedReceived);
    }

    [Theory]
    [ClassData(typeof(TupleGenerator))]
    public async Task Should_Notify_Observer_On_New_Tuple(SpaceTuple tuple)
    {
        await grain.WriteAsync(tuple);

        Assert.False(observer.LastReceived.IsNull);
        Assert.Equal(tuple, observer.LastReceived);
    }

    private class AsyncObserver : IAsyncObserver<SpaceTuple>
    {
        public SpaceTuple LastReceived { get; private set; } = new();
        public bool SpaceEmptiedReceived { get; private set; }

        public Task OnNextAsync(SpaceTuple tuple, StreamSequenceToken token)
        {
            if (!tuple.IsNull)
            {
                LastReceived = tuple;
            }
            else
            { 
                SpaceEmptiedReceived = true;
            }

            return Task.CompletedTask;
        }

        public Task OnCompletedAsync() => Task.CompletedTask;
        public Task OnErrorAsync(Exception ex) => Task.CompletedTask;
    }
}