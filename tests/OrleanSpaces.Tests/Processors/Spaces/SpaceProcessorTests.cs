using OrleanSpaces.Channels;
using OrleanSpaces.Processors.Spaces;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Processors.Spaces;

public class SpaceProcessorTests : IClassFixture<ClusterFixture>
{
    private readonly Fixture fixture;

    public SpaceProcessorTests(ClusterFixture clusterFixture) => fixture = new(clusterFixture);

    [Fact]
    public async Task Should_Forward_On_Insert_Action()
    {
        TupleAction<SpaceTuple> action = new(Guid.NewGuid(), new SpaceTuple(1, "a").WithDefaultStore(), TupleActionType.Insert);

        await fixture.Processor.OnNextAsync(action);

        SpaceTuple callbackResult = await fixture.CallbackChannel.Reader.ReadAsync(default);
        TupleAction<SpaceTuple> observerResult = await fixture.ObserverChannel.Reader.ReadAsync(default);

        Assert.Equal(action.AgentId, fixture.Bridge.Action.AgentId);
        Assert.Equal(action.StoreTuple.Tuple, fixture.Bridge.Action.StoreTuple.Tuple);
        Assert.Equal(action.Type, fixture.Bridge.Action.Type);
        Assert.Equal(callbackResult, action.StoreTuple.Tuple);
        Assert.Equal(observerResult, action);
    }

    [Fact]
    public async Task Should_Forward_On_Remove_Action()
    {
        TupleAction<SpaceTuple> action = new(Guid.NewGuid(), new SpaceTuple(1).WithDefaultStore(), TupleActionType.Remove);

        await fixture.Processor.OnNextAsync(action);

        TupleAction<SpaceTuple> observerResult = await fixture.ObserverChannel.Reader.ReadAsync(default);

        Assert.Equal(action.AgentId, fixture.Bridge.Action.AgentId);
        Assert.Equal(action.StoreTuple.Tuple, fixture.Bridge.Action.StoreTuple.Tuple);
        Assert.Equal(action.Type, fixture.Bridge.Action.Type);
        Assert.Equal(observerResult, action);
    }

    [Fact]
    public async Task Should_Forward_On_Clear_Action()
    {
        TupleAction<SpaceTuple> action = new(Guid.NewGuid(), new SpaceTuple(1).WithDefaultStore(), TupleActionType.Clear);

        await fixture.Processor.OnNextAsync(action);

        TupleAction<SpaceTuple> observerResult = await fixture.ObserverChannel.Reader.ReadAsync(default);
                    
        Assert.Equal(action.AgentId, fixture.Bridge.Action.AgentId);
        Assert.Equal(action.StoreTuple.Tuple, fixture.Bridge.Action.StoreTuple.Tuple);
        Assert.Equal(action.Type, fixture.Bridge.Action.Type);
        Assert.Equal(observerResult, action);
    }

    private class Fixture
    {
        internal SpaceProcessor Processor { get; }

        internal TestSpaceRouter<SpaceTuple, SpaceTemplate> Bridge { get; }
        internal ObserverChannel<SpaceTuple> ObserverChannel { get; }
        internal CallbackChannel<SpaceTuple> CallbackChannel { get; }

        public Fixture(ClusterFixture fixture)
        {
            Bridge = new();
            ObserverChannel = new();
            CallbackChannel = new();
            Processor = new(new(), fixture.Client, Bridge, ObserverChannel, CallbackChannel);
        }
    }
}