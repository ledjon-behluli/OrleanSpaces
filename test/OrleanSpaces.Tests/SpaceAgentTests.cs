using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Continuations;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;
using System.Collections.ObjectModel;

namespace OrleanSpaces.Tests;

[Collection(ClusterCollection.Name)]
public class SpaceAgentTests : IAsyncLifetime
{
    private readonly ISpaceChannel spaceChannel;
    private readonly ISpaceElementRouter router;
    private readonly EvaluationChannel evaluationChannel;
    private readonly ObserverRegistry observerRegistry;
    private readonly CallbackRegistry callbackRegistry;
    
    private ISpaceAgent agent;
    
    public SpaceAgentTests(ClusterFixture fixture)
    {
        spaceChannel = fixture.Client.ServiceProvider.GetRequiredService<ISpaceChannel>();
        router = fixture.Client.ServiceProvider.GetRequiredService<ISpaceElementRouter>();
        evaluationChannel = fixture.Client.ServiceProvider.GetRequiredService<EvaluationChannel>();
        observerRegistry = fixture.Client.ServiceProvider.GetRequiredService<ObserverRegistry>();
        callbackRegistry = fixture.Client.ServiceProvider.GetRequiredService<CallbackRegistry>();
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
        Assert.Equal(1, observerRegistry.Observers.Count(x => x.Equals(observer)));

        // Re-subscribe
        ObserverRef newRef = agent.Subscribe(observer);

        Assert.Equal(@ref, newRef);
        Assert.Equal(1, observerRegistry.Observers.Count(x => x.Equals(observer)));

        // Unsubscribe
        agent.Unsubscribe(@ref);
        Assert.Equal(0, observerRegistry.Observers.Count(x => x.Equals(observer)));
    }

    #endregion

    #region Router

    [Fact]
    public async Task Should_WriteAsync_When_SpaceElement_Is_A_Tuple()
    {
        SpaceTuple tuple = SpaceTuple.Create("routing");
        await router.RouteAsync(tuple);

        SpaceTuple peekedTuple = await agent.PeekAsync(SpaceTemplate.Create(tuple));

        Assert.False(peekedTuple.IsEmpty);
        Assert.Equal(tuple, peekedTuple);
    }

    [Fact]
    public async Task Should_PopAsync_When_SpaceElement_Is_A_Template()
    {
        SpaceTuple tuple = SpaceTuple.Create("routing");
        SpaceTemplate template = SpaceTemplate.Create(tuple);

        await agent.WriteAsync(tuple);
        await router.RouteAsync(template);

        SpaceTuple peekedTuple = await agent.PeekAsync(SpaceTemplate.Create(tuple));

        Assert.True(peekedTuple.IsEmpty);
        Assert.NotEqual(tuple, peekedTuple);
    }

    [Fact]
    public async Task Should_Throw_If_SpaceElement_Is_Null()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await router.RouteAsync(null));
    }

    [Fact]
    public async Task Should_Throw_If_SpaceElement_Is_Not_Supported()
    {
        await Assert.ThrowsAsync<NotImplementedException>(async () => await router.RouteAsync(new TestElement()));
    }

    #endregion

    #region WriteAsync

    [Fact]
    public async Task Should_WriteAsync()
    {
        SpaceTuple tuple = SpaceTuple.Create(1);
        await agent.WriteAsync(tuple);

        SpaceTuple peekedTuple = await agent.PeekAsync(SpaceTemplate.Create(tuple));

        Assert.False(peekedTuple.IsEmpty);
        Assert.Equal(tuple, peekedTuple);
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
    [MemberData(nameof(InlineTupleData))]
    public async Task Should_PeekAsync(SpaceTuple tuple)
    {
        await agent.WriteAsync(tuple);
        SpaceTuple peekedTuple = await agent.PeekAsync(SpaceTemplate.Create(tuple));

        Assert.Equal(tuple, peekedTuple);
    }

    [Theory]
    [MemberData(nameof(InlineTupleData))]
    public async Task Should_Return_Empty_Tuple_On_PeekAsync(SpaceTuple tuple)
    {
        await agent.WriteAsync(tuple);
        SpaceTuple peekedTuple = await agent.PeekAsync(SpaceTemplate.Create(0));

        Assert.True(peekedTuple.IsEmpty);
    }

    [Theory]
    [MemberData(nameof(InlineTupleData))]
    public async Task Should_Invoke_Callback_If_Tuple_Is_Available_On_PeekAsync(SpaceTuple tuple)
    {
        await agent.WriteAsync(tuple);

        SpaceTuple peekedTuple = new();

        await agent.PeekAsync(SpaceTemplate.Create(tuple), tuple =>
        {
            peekedTuple = tuple;
            return Task.CompletedTask;
        });

        Assert.False(peekedTuple.IsEmpty);
        Assert.Equal(tuple, peekedTuple);

    }

    [Fact]
    public async Task Should_Store_Callback_In_Registry_If_Tuple_Is_Not_Available_On_PeekAsync()
    {
        SpaceTuple peekedTuple = new();
        SpaceTemplate template = SpaceTemplate.Create("peek-not-available");
        Func<SpaceTuple, Task> callback = tuple => Task.FromResult(peekedTuple = tuple);

        await agent.PeekAsync(template, callback);

        ReadOnlyCollection<CallbackEntry> entries;
        while (!callbackRegistry.Entries.TryGetValue(template, out entries))
        {

        }

        Assert.True(peekedTuple.IsEmpty);
        Assert.Equal(callback, entries.Single().Callback);
    }

    [Fact]
    public async Task Should_Keep_Tuple_In_Space_On_PeekAsync()
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
    [MemberData(nameof(InlineTupleData))]
    public async Task Should_PopAsync(SpaceTuple tuple)
    {
        await agent.WriteAsync(tuple);
        SpaceTuple popedTuple = await agent.PopAsync(SpaceTemplate.Create(tuple));

        Assert.Equal(tuple, popedTuple);
    }

    [Theory]
    [MemberData(nameof(InlineTupleData))]
    public async Task Should_Return_Empty_Tuple_On_PopAsync(SpaceTuple tuple)
    {
        await agent.WriteAsync(tuple);
        SpaceTuple popedTuple = await agent.PopAsync(SpaceTemplate.Create(0));

        Assert.True(popedTuple.IsEmpty);
    }

    [Theory]
    [MemberData(nameof(InlineTupleData))]
    public async Task Should_Invoke_Callback_If_Tuple_Is_Available_On_PopAsync(SpaceTuple tuple)
    {
        await agent.WriteAsync(tuple);

        SpaceTuple popedTuple = new();

        await agent.PopAsync(SpaceTemplate.Create(tuple), tuple =>
        {
            popedTuple = tuple;
            return Task.CompletedTask;
        });

        Assert.False(popedTuple.IsEmpty);
        Assert.Equal(tuple, popedTuple);

    }

    [Fact]
    public async Task Should_Store_Callback_In_Registry_If_Tuple_Is_Not_Available_On_PopAsync()
    {
        SpaceTuple peekedTuple = new();
        SpaceTemplate template = SpaceTemplate.Create("pop-not-available");
        Func<SpaceTuple, Task> callback = tuple => Task.FromResult(peekedTuple = tuple);

        await agent.PopAsync(template, callback);

        ReadOnlyCollection<CallbackEntry> entries;
        while (!callbackRegistry.Entries.TryGetValue(template, out entries))
        {

        }

        Assert.True(peekedTuple.IsEmpty);
        Assert.Equal(callback, entries.Single().Callback);
    }

    [Fact]
    public async Task Should_Remove_Tuple_From_Space_On_PopAsync()
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

    #region ScanAsync

    [Fact]
    public async Task Should_ScanAsync()
    {
        const string key = "scan";

        foreach (var tuple in TupleData(key))
        {
            await agent.WriteAsync(tuple);
        }

        IEnumerable<SpaceTuple> tuples = await agent.ScanAsync(
            SpaceTemplate.Create((key, 1, typeof(string), typeof(float), UnitField.Null)));

        Assert.Equal(3, tuples.Count());
    }

    #endregion

    #region CountAsync

    [Fact]
    public async Task Should_CountAsync_Case_1()
    {
        const string key = "count-case-1";

        foreach (var tuple in TupleData(key))
        {
            await agent.WriteAsync(tuple);
        }

        int matchingCount = await agent.CountAsync(
            SpaceTemplate.Create((key, 1, typeof(string), typeof(float), UnitField.Null)));

        Assert.Equal(3, matchingCount);
    }

    [Fact]
    public async Task Should_CountAsync_Case_2()
    {
        const string key = "count-case-2";

        foreach (var tuple in TupleData(key))
        {
            await agent.WriteAsync(tuple);
        }

        int totalCount = await agent.CountAsync();
        int matchingCount = await agent.CountAsync(
            SpaceTemplate.Create((key, 1, typeof(string), typeof(float), UnitField.Null)));

        Assert.True(totalCount > matchingCount);
    }

    #endregion

    private static IEnumerable<SpaceTuple> TupleData(string key)
    {
        yield return SpaceTuple.Create((key, 1, "a", 1.0f, 1.0m));
        yield return SpaceTuple.Create((key, 1, "b", 1.2f, "d"));
        yield return SpaceTuple.Create((key, 1, "c", 1.5f, 'e'));
        yield return SpaceTuple.Create((key, 1, 1.5f, "c", 'e'));
        yield return SpaceTuple.Create((key, 1, "f", 1.7f, 'g', "f"));
        yield return SpaceTuple.Create((key, 2, "f", 1.7f, 'g'));
        yield return SpaceTuple.Create((key, 2, "f", 1.7f, 'g', "f"));
    }

    public static IEnumerable<object[]> InlineTupleData() =>
        new List<object[]>()
        {
            new object[] { SpaceTuple.Create(1) },
            new object[] { SpaceTuple.Create((1, "a")) },
            new object[] { SpaceTuple.Create((1, "a", 1.5f)) },
            new object[] { SpaceTuple.Create((1, "a", 1.5f, true)) }
        };

    private class TestElement : ISpaceElement
    {
        
    }
}