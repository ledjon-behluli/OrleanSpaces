using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests;

[Collection(ClusterCollection.Name)]
public class SpaceAgentTests : IAsyncLifetime
{
    private readonly ISpaceChannel spaceChannel;
    private readonly EvaluationChannel evaluationChannel;
    private readonly ObserverRegistry registry;

    private ISpaceAgent agent;

    public SpaceAgentTests(ClusterFixture fixture)
    {
        spaceChannel = fixture.Client.ServiceProvider.GetRequiredService<ISpaceChannel>();
        evaluationChannel = fixture.Client.ServiceProvider.GetRequiredService<EvaluationChannel>();
        registry = fixture.Client.ServiceProvider.GetRequiredService<ObserverRegistry>();
    }

    public async Task InitializeAsync() => agent = await spaceChannel.GetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    #region Subscriptions

    [Fact]
    public void Should_Handle_Observer_Subscriptions()
    {
        TestObserver observer = new();

        // Subscribe
        ObserverRef @ref = agent.Subscribe(observer);

        Assert.NotNull(@ref);
        Assert.NotNull(@ref.Observer);
        Assert.NotEqual(Guid.Empty, @ref.Id);
        Assert.Equal(observer, @ref.Observer);
        Assert.Equal(1, registry.Observers.Count(x => x.Equals(observer)));

        // Re-subscribe
        ObserverRef newRef = agent.Subscribe(observer);

        Assert.Equal(@ref, newRef);
        Assert.Equal(1, registry.Observers.Count(x => x.Equals(observer)));

        // Unsubscribe
        agent.Unsubscribe(@ref);
        Assert.Equal(0, registry.Observers.Count(x => x.Equals(observer)));
    }

    #endregion

    #region WriteAsync

    [Fact]
    public async Task Should_WriteAsync()
    {
        // TODO: Test me better
        await agent.WriteAsync(SpaceTuple.Create(1));
        Assert.True(true);
    }

    [Fact]
    public async Task Should_Throw_On_WriteAsync_If_Empty_Tuple()
    {
        await Assert.ThrowsAsync<ArgumentException>(async () => await agent.WriteAsync(new SpaceTuple()));
    }

    #endregion

    #region EvaluateAsync

    [Fact]
    public async Task Should_EvaluateAsync()
    {
        SpaceTuple tuple = SpaceTuple.Create(1);
        await agent.EvaluateAsync(() => Task.FromResult(tuple));

        Func<Task<SpaceTuple>> evaluation = await evaluationChannel.Reader.ReadAsync();

        Assert.Equal(tuple, await evaluation());
    }

    [Fact]
    public async Task Should_Throw_On_EvaluateAsync_If_Null()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await agent.EvaluateAsync(null));
    }

    #endregion

    #region PeekAsync

    [Theory]
    [MemberData(nameof(TupleData))]
    public async Task Should_PeekAsync(SpaceTuple tuple)
    {
        await agent.WriteAsync(tuple);
        SpaceTuple peekedTuple = await agent.PeekAsync(SpaceTemplate.Create(tuple));

        Assert.Equal(tuple, peekedTuple);
    }

    [Theory]
    [MemberData(nameof(TupleData))]
    public async Task Should_Return_Empty_Tuple_On_PeekAsync(SpaceTuple tuple)
    {
        await agent.WriteAsync(tuple);
        SpaceTuple peekedTuple = await agent.PeekAsync(SpaceTemplate.Create(0));

        Assert.True(peekedTuple.IsEmpty);
    }

    [Fact]
    public async Task Should_Keep_Tuple_In_Space_When_PeekAsync()
    {
        SpaceTuple tuple = SpaceTuple.Create("peek");
        SpaceTemplate template = SpaceTemplate.Create(tuple);

        await agent.WriteAsync(tuple);

        for (int i = 0; i < 3; i++)
        {
            SpaceTuple peekedTuple = await agent.PeekAsync(template);

            Assert.False(peekedTuple.IsEmpty);
            Assert.Equal(tuple, peekedTuple);
        }
    }

    [Fact]
    public async Task Should_Throw_On_PeekAsync_If_Null()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await agent.PeekAsync(SpaceTemplate.Create(0), null));
    }


    #endregion

    #region PopAsync

    [Theory]
    [MemberData(nameof(TupleData))]
    public async Task Should_PopAsync(SpaceTuple tuple)
    {
        await agent.WriteAsync(tuple);
        SpaceTuple popedTuple = await agent.PopAsync(SpaceTemplate.Create(tuple));

        Assert.Equal(tuple, popedTuple);
    }

    [Theory]
    [MemberData(nameof(TupleData))]
    public async Task Should_Return_Empty_Tuple_On_PopAsync(SpaceTuple tuple)
    {
        await agent.WriteAsync(tuple);
        SpaceTuple popedTuple = await agent.PopAsync(SpaceTemplate.Create(0));

        Assert.True(popedTuple.IsEmpty);
    }

    [Fact]
    public async Task Should_Remove_Tuple_From_Space_When_PopAsync()
    {
        SpaceTuple tuple = SpaceTuple.Create("pop");
        SpaceTemplate template = SpaceTemplate.Create(tuple);

        await agent.WriteAsync(tuple);

        bool firstIteration = true;
        for (int i = 0; i < 3; i++)
        {
            SpaceTuple popedTuple = await agent.PopAsync(template);

            if (firstIteration)
            {
                Assert.False(popedTuple.IsEmpty);
                Assert.Equal(tuple, popedTuple);
            }
            else
            {
                Assert.True(popedTuple.IsEmpty);
                Assert.NotEqual(tuple, popedTuple);
            }

            firstIteration = false;
        }
    }

    [Fact]
    public async Task Should_Throw_On_PopAsync_If_Null()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await agent.PopAsync(SpaceTemplate.Create(0), null));
    }

    #endregion

    public static IEnumerable<object[]> TupleData() =>
        new List<object[]>()
        {
            new object[] { SpaceTuple.Create(1) },
            new object[] { SpaceTuple.Create((1, "a")) },
            new object[] { SpaceTuple.Create((1, "a", 1.5f)) },
            new object[] { SpaceTuple.Create((1, "a", 1.5f, true)) }
        };
}