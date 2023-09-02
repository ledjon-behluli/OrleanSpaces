using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tests.Grains;

public class IntGrainTests : IAsyncLifetime, IClassFixture<ClusterFixture>
{
    private readonly IIntGrain grain;
    private readonly IClusterClient client;
    private readonly Guid agentId = Guid.NewGuid();
    private readonly TestStreamObserver<IntTuple> observer;

    [AllowNull] private IAsyncStream<TupleAction<IntTuple>> stream;

    public IntGrainTests(ClusterFixture fixture)
    {
        observer = new();
        client = fixture.Client;
        grain = fixture.Client.GetGrain<IIntGrain>(IIntGrain.Key);
    }

    public async Task InitializeAsync()
    {
        stream = client
           .GetStreamProvider(Constants.PubSubProvider)
           .GetStream<TupleAction<IntTuple>>(
               StreamId.Create(Constants.StreamName, IIntGrain.Key));

        await stream.SubscribeAsync(observer);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Theory]
    [ClassData(typeof(IntTupleGenerator))]
    public async Task Should_Notify_Observer(IntTuple tuple)
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

    static TupleAction<IntTuple> InsertAction(IntTuple tuple)
          => new(Guid.NewGuid(), tuple, TupleActionType.Insert);

    static TupleAction<IntTuple> RemoveAction(IntTuple tuple)
       => new(Guid.NewGuid(), tuple, TupleActionType.Remove);

    async Task Reset()
    {
        await grain.RemoveAll(agentId);
        observer.Reset();
    }
}
