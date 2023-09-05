using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Tuples;
using OrleanSpaces.Grains.Directors;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tests.Directors;

public class SpaceDirectorTests : IAsyncLifetime, IClassFixture<ClusterFixture>
{
    private readonly ISpaceDirector director;
    private readonly IClusterClient client;
    private readonly Guid agentId = Guid.NewGuid();
    private readonly TestStreamObserver<SpaceTuple> observer;

    [AllowNull] private IAsyncStream<TupleAction<SpaceTuple>> stream;

    public SpaceDirectorTests(ClusterFixture fixture)
    {
        observer = new();
        client = fixture.Client;
        director = fixture.Client.GetGrain<ISpaceDirector>(Constants.RealmKey_Space);
    }

    public async Task InitializeAsync()
    {
        stream = client
            .GetStreamProvider(Constants.PubSubProvider)
            .GetStream<TupleAction<SpaceTuple>>(
                StreamId.Create(Constants.StreamName, Constants.RealmKey_Space));

        await stream.SubscribeAsync(observer);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Theory]
    [ClassData(typeof(SpaceTupleGenerator))]
    public async Task Should_Notify_Observer(SpaceTuple tuple)
    {
        await Reset();

        await director.Insert(InsertAction(tuple));
        await director.Remove(RemoveAction(tuple));

        while (observer.InvokedCount < 3)
        {
            // wait until the messages reach the observer...
        }

        Assert.Equal(tuple, observer.LastExpansionTuple);
        Assert.Equal(tuple, observer.LastContractionTuple);
        Assert.True(observer.HasFlattened);
    }

    static TupleAction<SpaceTuple> InsertAction(SpaceTuple tuple)
          => new(Guid.NewGuid(), tuple.WithDefaultStore(), TupleActionType.Insert);

    static TupleAction<SpaceTuple> RemoveAction(SpaceTuple tuple)
       => new(Guid.NewGuid(), tuple.WithDefaultStore(), TupleActionType.Remove);

    async Task Reset()
    {
        await director.RemoveAll(agentId);
        observer.Reset();
    }
}
