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
    private readonly TestStreamObserver<IntTuple> observer = new();

    [AllowNull] private IAsyncStream<TupleAction<IntTuple>> stream;

    public IntDirectorTests(ClusterFixture fixture)
    {
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
        observer.Reset();

        Guid storeId = await director.Insert(new(agentId, tuple.WithDefaultStore(), TupleActionType.Insert));
        await director.Remove(new(agentId, new(storeId, tuple), TupleActionType.Remove));
        await director.RemoveAll(agentId);

        while (observer.InvokedCount < 3)
        {
            await Task.Delay(1);
        }

        Assert.Equal(tuple, observer.LastExpansionTuple);
        Assert.Equal(tuple, observer.LastContractionTuple);
        Assert.True(observer.HasFlattened);
    }
}