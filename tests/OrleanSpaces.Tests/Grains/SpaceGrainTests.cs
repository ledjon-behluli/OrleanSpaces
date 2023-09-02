using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tests.Grains;

public class SpaceGrainTests : IAsyncLifetime, IClassFixture<ClusterFixture>
{
    private readonly ISpaceGrain grain;
    private readonly IClusterClient client;
    private readonly Guid agentId = Guid.NewGuid();
    private readonly TestStreamObserver<SpaceTuple> observer;

    [AllowNull] private IAsyncStream<TupleAction<SpaceTuple>> stream;

    public SpaceGrainTests(ClusterFixture fixture)
    {
        observer = new();
        client = fixture.Client;
        grain = fixture.Client.GetGrain<ISpaceGrain>(ISpaceGrain.Key);
    }

    public async Task InitializeAsync()
    {
        stream = client
            .GetStreamProvider(Constants.PubSubProvider)
            .GetStream<TupleAction<SpaceTuple>>(
                StreamId.Create(Constants.Store_StreamNamespace, ISpaceGrain.Key));

        await stream.SubscribeAsync(observer);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Theory]
    [ClassData(typeof(SpaceTupleGenerator))]
    public async Task Should_Notify_Observer(SpaceTuple tuple)
    {
        await Reset();

        await grain.Insert(InsertAction(tuple));
        await grain.Remove(RemoveAction(tuple));

        while (observer.InvokedCount < 3)
        {
            // wait until the messages reach the observer...
        }

        Assert.Equal(tuple, observer.LastExpansionTuple);
        Assert.Equal(tuple, observer.LastContractionTuple);
        Assert.True(observer.HasFlattened);
    }

    static TupleAction<SpaceTuple> InsertAction(SpaceTuple tuple)
          => new(Guid.NewGuid(), tuple, TupleActionType.Insert);

    static TupleAction<SpaceTuple> RemoveAction(SpaceTuple tuple)
       => new(Guid.NewGuid(), tuple, TupleActionType.Remove);

    async Task Reset()
    {
        await grain.RemoveAll(agentId);
        observer.Reset();
    }
}
