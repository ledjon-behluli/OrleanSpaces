using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tests;

// The majority of grain methods are tested via the agent tests. Here we test only some edge cases.

public class SpaceGrainTests : IAsyncLifetime, IClassFixture<ClusterFixture>
{
    private readonly ISpaceGrain grain;
    private readonly IClusterClient client;
    private readonly Guid agentId = Guid.NewGuid();
    private readonly TestStreamObserver<SpaceTuple> observer;

    private StreamId streamId;
    [AllowNull] private IAsyncStream<TupleAction<SpaceTuple>> stream;

    public SpaceGrainTests(ClusterFixture fixture)
    {
        observer = new();
        client = fixture.Client;
        grain = fixture.Client.GetGrain<ISpaceGrain>(ISpaceGrain.Key);
    }

    public async Task InitializeAsync()
    {
        streamId = await grain.GetStreamId();
        stream = client.GetStreamProvider(Constants.PubSubProvider).GetStream<TupleAction<SpaceTuple>>(streamId);
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

public class IntGrainTests : IAsyncLifetime, IClassFixture<ClusterFixture>
{
    private readonly IIntGrain grain;
    private readonly IClusterClient client;
    private readonly Guid agentId = Guid.NewGuid();
    private readonly TestStreamObserver<IntTuple> observer;

    private StreamId streamId;
    [AllowNull] private IAsyncStream<TupleAction<IntTuple>> stream;

    public IntGrainTests(ClusterFixture fixture)
    {
        observer = new();
        client = fixture.Client;
        grain = fixture.Client.GetGrain<IIntGrain>(IIntGrain.Key);
    }

    public async Task InitializeAsync()
    {
        streamId = await grain.GetStreamId();
        stream = client.GetStreamProvider(Constants.PubSubProvider).GetStream<TupleAction<IntTuple>>(streamId);
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

public class GrainIdentityTests
{
    [Theory]
    [MemberData(nameof(KeyData))]
    public void Should_Equal_Key(string expected, string actual)
        => Assert.Equal(expected, actual);

    private static object[][] KeyData() =>
        new[]
        {
            new object[] { "BoolStore", IBoolGrain.Key },
            new object[] { "ByteStore", IByteGrain.Key },
            new object[] { "CharStore", ICharGrain.Key },
            new object[] { "DateTimeOffsetStore", IDateTimeOffsetGrain.Key },
            new object[] { "DateTimeStore", IDateTimeGrain.Key },
            new object[] { "DecimalStore", IDecimalGrain.Key },
            new object[] { "DoubleStore", IDoubleGrain.Key },
            new object[] { "FloatStore", IFloatGrain.Key },
            new object[] { "GuidStore", IGuidGrain.Key },
            new object[] { "HugeStore", IHugeGrain.Key },
            new object[] { "IntStore", IIntGrain.Key },
            new object[] { "LongStore", ILongGrain.Key },
            new object[] { "SByteStore", ISByteGrain.Key },
            new object[] { "ShortStore", IShortGrain.Key },
            new object[] { "SpaceStore", ISpaceGrain.Key },
            new object[] { "TimeSpanStore", ITimeSpanGrain.Key },
            new object[] { "UHugeStore", IUHugeGrain.Key },
            new object[] { "UIntStore", IUIntGrain.Key },
            new object[] { "ULongStore", IULongGrain.Key },
            new object[] { "UShortStore", IUShortGrain.Key }
        };
}