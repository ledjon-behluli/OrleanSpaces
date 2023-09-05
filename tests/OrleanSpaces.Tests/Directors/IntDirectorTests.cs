using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tests.Directors;

public class IntDirectorTests : IAsyncLifetime, IClassFixture<ClusterFixture>
{
    private readonly IIntDirector director;
    private readonly IClusterClient client;
    private readonly Guid agentId = Guid.NewGuid();
    private readonly TestStreamObserver<IntTuple> observer;

    [AllowNull] private IAsyncStream<TupleAction<IntTuple>> stream;

    public IntDirectorTests(ClusterFixture fixture)
    {
        observer = new();
        client = fixture.Client;
        director = fixture.Client.GetGrain<IIntDirector>(Constants.RealmKey_Int);
    }

    public async Task InitializeAsync()
    {
        stream = client
           .GetStreamProvider(Constants.PubSubProvider)
           .GetStream<TupleAction<IntTuple>>(
               StreamId.Create(Constants.StreamName, Constants.RealmKey_Int));

        await stream.SubscribeAsync(observer);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Theory]
    [ClassData(typeof(IntTupleGenerator))]
    public async Task Should_Notify_Observer(IntTuple tuple)
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

    static TupleAction<IntTuple> InsertAction(IntTuple tuple)
          => new(Guid.NewGuid(), tuple.WithDefaultStore(), TupleActionType.Insert);

    static TupleAction<IntTuple> RemoveAction(IntTuple tuple)
       => new(Guid.NewGuid(), tuple.WithDefaultStore(), TupleActionType.Remove);

    async Task Reset()
    {
        await director.RemoveAll(agentId);
        observer.Reset();
    }
}
