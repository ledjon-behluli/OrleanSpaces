using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Channels;
using OrleanSpaces.Continuations;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;
using System.Collections.ObjectModel;

namespace OrleanSpaces.Tests.Agents;

public class IntAgentTests : IClassFixture<ClusterFixture>
{
    private readonly ITupleRouter<IntTuple, IntTemplate> router;
    private readonly ObserverRegistry<IntTuple> observerRegistry;
    private readonly CallbackRegistry<int, IntTuple, IntTemplate> callbackRegistry;
    private readonly EvaluationChannel<IntTuple> evaluationChannel;
    private readonly ISpaceAgent<int, IntTuple, IntTemplate> agent;

    const int routingKey = 100;
    const int peek = 101;
    const int pop = 102;
    const int scan = 103;
    const int peekNotAvailable = 104;
    const int popNotAvailable = 105;

    public IntAgentTests(ClusterFixture fixture)
    {
        agent = fixture.Client.ServiceProvider.GetRequiredService<ISpaceAgent<int, IntTuple, IntTemplate>>();
        router = fixture.Client.ServiceProvider.GetRequiredService<ITupleRouter<IntTuple, IntTemplate>>();
        observerRegistry = fixture.Client.ServiceProvider.GetRequiredService<ObserverRegistry<IntTuple>>();
        callbackRegistry = fixture.Client.ServiceProvider.GetRequiredService<CallbackRegistry<int, IntTuple, IntTemplate>>();
        evaluationChannel = fixture.Client.ServiceProvider.GetRequiredService<EvaluationChannel<IntTuple>>();
    }

    #region Subscriptions

    [Fact]
    public void Should_Handle_Observer_Subscriptions()
    {
        TestSpaceObserver<IntTuple> observer = new();

        // Subscribe
        Guid id = agent.Subscribe(observer);

        Assert.NotEqual(Guid.Empty, id);
        Assert.Equal(1, observerRegistry.Observers.Count(x => x.Equals(observer)));

        // Re-subscribe
        Guid newId = agent.Subscribe(observer);

        Assert.Equal(id, newId);
        Assert.Equal(1, observerRegistry.Observers.Count(x => x.Equals(observer)));

        // Unsubscribe
        agent.Unsubscribe(id);
        Assert.Equal(0, observerRegistry.Observers.Count(x => x.Equals(observer)));
    }

    #endregion

    #region Router

    static readonly IntTuple routingTuple = new(101010);

    [Fact]
    public async Task Should_WriteAsync_When_Routing_Tuple()
    {
        await router.RouteAsync(routingTuple);

        IntTuple peekedTuple = await agent.PeekAsync(routingTuple.ToTemplate());

        Assert.Equal(routingTuple, peekedTuple);
    }

    [Fact]
    public async Task Should_PopAsync_When_Routing_Template()
    {
        IntTemplate template = routingTuple.ToTemplate();

        await router.RouteAsync(template);

        IntTuple peekedTuple = await agent.PeekAsync(template);

        Assert.NotEqual(routingTuple, peekedTuple);
    }

    #endregion

    #region WriteAsync

    [Fact]
    public async Task Should_WriteAsync()
    {
        IntTuple tuple = new(1);
        await agent.WriteAsync(tuple);

        IntTuple peekedTuple = await agent.PeekAsync(tuple.ToTemplate());

        Assert.Equal(tuple, peekedTuple);
    }

    [Fact]
    public Task Should_Throw_On_WriteAsync_If_Empty_Tuple()
        => Assert.ThrowsAsync<ArgumentException>(async () => await agent.WriteAsync(new()));

    #endregion

    #region EvaluateAsync

    [Fact]
    public async Task Should_EvaluateAsync()
    {
        IntTuple tuple = new(1);
        await agent.EvaluateAsync(() => Task.FromResult(tuple));

        Func<Task<IntTuple>> evaluation = await evaluationChannel.Reader.ReadAsync();

        Assert.Equal(tuple, await evaluation());
    }

    [Fact]
    public Task Should_Throw_On_EvaluateAsync_If_Empty()
        => Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await agent.EvaluateAsync(() => Task.FromResult(new IntTuple())));

    #endregion

    #region PeekAsync

    [Theory]
    [ClassData(typeof(IntTupleGenerator))]
    public async Task Should_PeekAsync(IntTuple tuple)
    {
        await agent.WriteAsync(tuple);
        IntTuple peekedTuple = await agent.PeekAsync(tuple.ToTemplate());

        Assert.Equal(tuple, peekedTuple);
    }

    [Theory]
    [ClassData(typeof(IntTupleGenerator))]
    public async Task Should_Return_Empty_Tuple_On_PeekAsync(IntTuple tuple)
    {
        await agent.WriteAsync(tuple);
        IntTuple peekedTuple = await agent.PeekAsync(new(0));

        peekedTuple.AssertEmpty();
    }

    [Theory]
    [ClassData(typeof(IntTupleGenerator))]
    public async Task Should_Invoke_Callback_If_Tuple_Is_Available_On_PeekAsync(IntTuple tuple)
    {
        await agent.WriteAsync(tuple);

        IntTuple? peekedTuple = null;

        await agent.PeekAsync(tuple.ToTemplate(), tuple =>
        {
            peekedTuple = tuple;
            return Task.CompletedTask;
        });

        Assert.NotNull(peekedTuple);
        Assert.Equal(tuple, peekedTuple);
    }

    [Fact]
    public async Task Should_Store_Callback_In_Registry_If_Tuple_Is_Not_Available_On_PeekAsync()
    {
        IntTuple peekedTuple = new();
        IntTemplate template = new(peekNotAvailable);

        await agent.PeekAsync(template, callback);

        ReadOnlyCollection<CallbackEntry<IntTuple>>? entries;
        while (!callbackRegistry.Entries.TryGetValue(template, out entries))
        {

        }

        peekedTuple.AssertEmpty();
        Assert.Equal(callback, entries.Single().Callback);

        Task callback(IntTuple tuple) => Task.FromResult(peekedTuple = tuple);
    }

    [Fact]
    public async Task Should_Keep_Tuple_In_Space_On_PeekAsync()
    {
        IntTuple tuple = new(peek);

        await agent.WriteAsync(tuple);

        for (int i = 0; i < 3; i++)
        {
            IntTuple peekedTuple = await agent.PeekAsync(tuple.ToTemplate());
            Assert.Equal(tuple, peekedTuple);
        }
    }

    [Fact]
    public Task Should_Throw_On_PeekAsync_If_Null_Callback()
        => Assert.ThrowsAsync<ArgumentNullException>(async () => await agent.PeekAsync(new(0), null!));

    #endregion

    #region PopAsync

    [Theory]
    [ClassData(typeof(IntTupleGenerator))]
    public async Task Should_PopAsync(IntTuple tuple)
    {
        await agent.WriteAsync(tuple);
        IntTuple popedTuple = await agent.PopAsync(tuple.ToTemplate());

        Assert.Equal(tuple, popedTuple);
    }

    [Theory]
    [ClassData(typeof(IntTupleGenerator))]
    public async Task Should_Return_Empty_Tuple_On_PopAsync(IntTuple tuple)
    {
        await agent.WriteAsync(tuple);
        IntTuple popedTuple = await agent.PopAsync(new(0));

        popedTuple.AssertEmpty();
    }

    [Theory]
    [ClassData(typeof(IntTupleGenerator))]
    public async Task Should_Invoke_Callback_If_Tuple_Is_Available_On_PopAsync(IntTuple tuple)
    {
        await agent.WriteAsync(tuple);

        IntTuple popedTuple = new();

        await agent.PopAsync(tuple.ToTemplate(), tuple =>
        {
            popedTuple = tuple;
            return Task.CompletedTask;
        });

        Assert.Equal(tuple, popedTuple);

    }

    [Fact]
    public async Task Should_Store_Callback_In_Registry_If_Tuple_Is_Not_Available_On_PopAsync()
    {
        IntTuple peekedTuple = new();
        IntTemplate template = new(popNotAvailable);

        await agent.PopAsync(template, callback);

        ReadOnlyCollection<CallbackEntry<IntTuple>>? entries;
        while (!callbackRegistry.Entries.TryGetValue(template, out entries))
        {

        }

        peekedTuple.AssertEmpty();
        Assert.Equal(callback, entries.Single().Callback);

        Task callback(IntTuple tuple) => Task.FromResult(peekedTuple = tuple);
    }

    [Fact]
    public async Task Should_Remove_Tuple_From_Space_On_PopAsync()
    {
        IntTuple tuple = new(pop);

        await agent.WriteAsync(tuple);

        bool firstIteration = true;
        for (int i = 0; i < 3; i++)
        {
            IntTuple popedTuple = await agent.PopAsync(tuple.ToTemplate());

            if (firstIteration)
            {
                Assert.Equal(tuple, popedTuple);
            }
            else
            {
                Assert.NotEqual(tuple, popedTuple);
            }

            firstIteration = false;
        }
    }

    [Fact]
    public Task Should_Throw_On_PopAsync_If_Null_Callback()
        => Assert.ThrowsAsync<ArgumentNullException>(async () => await agent.PopAsync(new(0), null!));

    #endregion

    #region ScanAsync

    [Fact]
    public async Task Should_ScanAsync()
    {
        foreach (var tuple in ScanData())
        {
            await agent.WriteAsync(tuple);
        }

        IEnumerable<IntTuple> tuples = await agent.ScanAsync(new(scan, 1, 2, 3, null));
        Assert.Equal(3, tuples.Count());
    }

    private static IEnumerable<IntTuple> ScanData()
    {
        yield return new(scan, 1, 2, 3, 4);
        yield return new(scan, 1, 2, 4, 5);
        yield return new(scan, 1, 2, 3, 6);
        yield return new(scan, 1, 2, 4, 5);
        yield return new(scan, 1, 3, 4, 7);
        yield return new(scan, 1, 1, 6, 5);
        yield return new(scan, 1, 2, 3, 9);
    }

    #endregion

    #region CountAsync

    [Fact]
    public async Task Should_CountAsync()
    {
        await agent.ClearAsync();

        int count = await agent.CountAsync();
        Assert.Equal(0, count);

        await agent.WriteAsync(new(1));
        await agent.WriteAsync(new(1));
        await agent.WriteAsync(new(2));

        count = await agent.CountAsync();
        Assert.Equal(3, count);
    }

    #endregion

    #region ClearAsync

    [Fact]
    public async Task Should_ClearAsync()
    {
        await agent.WriteAsync(new(1));

        int count = await agent.CountAsync();
        Assert.NotEqual(0, count);

        await agent.ClearAsync();

        count = await agent.CountAsync();
        Assert.Equal(0, count);
    }

    #endregion
}