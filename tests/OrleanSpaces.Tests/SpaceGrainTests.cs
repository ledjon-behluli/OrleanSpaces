using Orleans;
using Orleans.Streams;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests;

// The majority of grain methods are tested via the agent tests. Here we test only some edge cases.
public class SpaceGrainTests : IAsyncLifetime, IClassFixture<ClusterFixture>
{
    private readonly IClusterClient client;
    private readonly ISpaceGrain grain;
    private readonly AsyncObserver observer;

    private IAsyncStream<ISpaceTuple> stream;
    private Guid streamId;

    public SpaceGrainTests(ClusterFixture fixture)
    {
        observer = new();
        client = fixture.Client;
        grain = fixture.Client.GetGrain<ISpaceGrain>(Constants.SpaceGrainId);
    }

    public async Task InitializeAsync()
    {
        streamId = await grain.ListenAsync();
        var provider = client.GetStreamProvider(Constants.PubSubProvider);
        stream = provider.GetStream<ISpaceTuple>(streamId, Constants.TupleStream);
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
    public async Task Should_Notify_Observer_On_Flattening()
    {
        SpaceTuple tuple1 = new(1);
        SpaceTuple tuple2 = new(1, "a");

        await grain.WriteAsync(tuple1);
        await grain.WriteAsync(tuple2);

        _ = await grain.PopAsync(tuple1);
        Assert.False(observer.LastFlattening);

        _ = await grain.PopAsync(tuple2);
        Assert.True(observer.LastFlattening);

        // Clear for next test
        await grain.PopAsync(tuple1);
        await grain.PopAsync(tuple2);

        observer.Reset();
    }

    [Theory]
    [ClassData(typeof(SpaceTupleGenerator))]
    public async Task Should_Notify_Observer_On_Expansion_And_Contraction(SpaceTuple tuple)
    {
        // Add
        await grain.WriteAsync(tuple);

        // Remove
        SpaceTemplate template = tuple;
        await grain.PopAsync(template);

        Assert.Equal(tuple, observer.LastTuple);
        Assert.Equal(template, observer.LastTemplate);

        // Clear for next test
        observer.Reset();
    }

    private class AsyncObserver : IAsyncObserver<ISpaceTuple>
    {
        public SpaceTuple LastTuple { get; private set; } = new();
        public SpaceTemplate LastTemplate { get; private set; } = new();
        public bool LastFlattening { get; private set; }

        public Task OnNextAsync(ISpaceTuple tuple, StreamSequenceToken token)
        {
            if (tuple is SpaceTuple spaceTuple)
            {
                LastTuple = spaceTuple;
            }
            else if (tuple is SpaceTemplate template)
            {
                LastTemplate = template;
            }
            else if (tuple is SpaceUnit)
            {
                LastFlattening = true;
            }

            return Task.CompletedTask;
        }

        public Task OnCompletedAsync() => Task.CompletedTask;
        public Task OnErrorAsync(Exception ex) => Task.CompletedTask;

        public void Reset()
        {
            LastTuple = SpaceTuple.Null;
            LastFlattening = false;
        }
    }
}

public class StateTests
{
    [Fact]
    public void Should_Implicitly_Convert_To_SpaceTuple()
    {
        SpaceTuple tuple = new(1, 2, 3);
        SpaceTuple @implicit = new TupleSpaceState.SpaceTupleState() { Fields = new List<object> { 1, 2, 3 } };

        Assert.Equal(tuple, @implicit);
    }

    [Fact]
    public void Should_Implicitly_Convert_From_SpaceTuple()
    {
        TupleSpaceState.SpaceTupleState state = new() { Fields = new List<object> { 1, 2, 3 } };
        TupleSpaceState.SpaceTupleState @implicit = new SpaceTuple(1, 2, 3);

        Assert.Equal(state.Length, @implicit.Length);

        for (int i = 0; i < state.Length; i++)
        {
            Assert.Equal(state.Fields.ElementAt(i), @implicit.Fields.ElementAt(i));
        }
    }
}