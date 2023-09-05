using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples;
using System.Collections.ObjectModel;

namespace OrleanSpaces.Tests.Agents;

public class SpaceAgentTests : IClassFixture<ClusterFixture>
{
    const string routingKey = "routing";
    const string peek = "peek";
    const string pop = "pop";
    const string scan = "scan";
    const string consume = "consume";
    const string peekNotAvailable = "peek-not-available";
    const string popNotAvailable = "pop-not-available";

    private readonly ISpaceAgent agent;
    private readonly ISpaceRouter<SpaceTuple, SpaceTemplate> router;
    private readonly ObserverRegistry<SpaceTuple> observerRegistry;
    private readonly CallbackRegistry callbackRegistry;
    private readonly EvaluationChannel<SpaceTuple> evaluationChannel;

    public SpaceAgentTests(ClusterFixture fixture)
    {
        agent = fixture.Client.ServiceProvider.GetRequiredService<ISpaceAgent>();
        router = fixture.Client.ServiceProvider.GetRequiredService<ISpaceRouter<SpaceTuple, SpaceTemplate>>();
        observerRegistry = fixture.Client.ServiceProvider.GetRequiredService<ObserverRegistry<SpaceTuple>>();
        callbackRegistry = fixture.Client.ServiceProvider.GetRequiredService<CallbackRegistry>();
        evaluationChannel = fixture.Client.ServiceProvider.GetRequiredService<EvaluationChannel<SpaceTuple>>();
    }

    #region Subscriptions

    [Fact]
    public void Should_Handle_Observer_Subscriptions()
    {
        TestSpaceObserver<SpaceTuple> observer = new();

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

    static readonly SpaceTuple routingTuple = new(routingKey);

    [Fact]
    public async Task Should_WriteAsync_When_Routing_Tuple()
    {
        await router.RouteTuple(routingTuple);

        SpaceTuple peekedTuple = await agent.PeekAsync(routingTuple.ToTemplate());

        Assert.Equal(routingTuple, peekedTuple);
    }

    [Fact]
    public async Task Should_PopAsync_When_Routing_Template()
    {
        SpaceTemplate template = routingTuple.ToTemplate();

        await router.RouteTemplate(template);

        SpaceTuple peekedTuple = await agent.PeekAsync(template);

        Assert.NotEqual(routingTuple, peekedTuple);
    }

    #endregion

    #region WriteAsync

    [Fact]
    public async Task Should_WriteAsync()
    {
        SpaceTuple tuple = new(1);
        await agent.WriteAsync(tuple);

        SpaceTuple peekedTuple = await agent.PeekAsync(tuple.ToTemplate());

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
        SpaceTuple tuple = new(1);
        await agent.EvaluateAsync(() => Task.FromResult(tuple));

        Func<Task<SpaceTuple>> evaluation = await evaluationChannel.Reader.ReadAsync();

        Assert.Equal(tuple, await evaluation());
    }

    [Fact]
    public Task Should_Throw_On_EvaluateAsync_If_Null_Eval_Function()
        => Assert.ThrowsAsync<ArgumentNullException>(async () => await agent.EvaluateAsync(null!));

    #endregion

    #region PeekAsync

    [Theory]
    [ClassData(typeof(SpaceTupleGenerator))]
    public async Task Should_PeekAsync(SpaceTuple tuple)
    {
        await agent.WriteAsync(tuple);
        SpaceTuple peekedTuple = await agent.PeekAsync(tuple.ToTemplate());

        Assert.Equal(tuple, peekedTuple);
    }

    [Theory]
    [ClassData(typeof(SpaceTupleGenerator))]
    public async Task Should_Return_Empty_Tuple_On_PeekAsync(SpaceTuple tuple)
    {
        await agent.WriteAsync(tuple);
        SpaceTuple peekedTuple = await agent.PeekAsync(new(0));

        peekedTuple.AssertEmpty();
    }

    [Theory]
    [ClassData(typeof(SpaceTupleGenerator))]
    public async Task Should_Invoke_Callback_If_Tuple_Is_Available_On_PeekAsync(SpaceTuple tuple)
    {
        await agent.WriteAsync(tuple);

        SpaceTuple? peekedTuple = null;

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
        SpaceTuple peekedTuple = new();
        SpaceTemplate template = new(peekNotAvailable);

        await agent.PeekAsync(template, callback);

        ReadOnlyCollection<CallbackEntry<SpaceTuple>>? entries;
        while (!callbackRegistry.Entries.TryGetValue(template, out entries))
        {

        }

        peekedTuple.AssertEmpty();
        Assert.Equal(callback, entries.Single().Callback);

        Task callback(SpaceTuple tuple) => Task.FromResult(peekedTuple = tuple);
    }

    [Fact]
    public async Task Should_Keep_Tuple_In_Space_On_PeekAsync()
    {
        SpaceTuple tuple = new(peek);

        await agent.WriteAsync(tuple);

        for (int i = 0; i < 3; i++)
        {
            SpaceTuple peekedTuple = await agent.PeekAsync(tuple.ToTemplate());
            Assert.Equal(tuple, peekedTuple);
        }
    }

    [Fact]
    public async Task Should_EnumerateAsync()
    {
        SpaceTuple[] tuples = new SpaceTuple[5]
        {
            new(consume, 0),
            new(consume, 1),
            new(consume, 2),
            new(consume, 3),
            new(consume, 4)
        };

        _ = Task.Run(async () =>
        {
            int i = 1;
            await foreach (SpaceTuple tuple in agent.PeekAsync())
            {
                Assert.Equal(tuples[i], tuple);
                i++;
            }
        });

        int i = 0;
        while (i < 5)
        {
            await agent.WriteAsync(tuples[i]);
            i++;
        }
    }

    [Fact]
    public Task Should_Throw_On_PeekAsync_If_Null_Callback()
        => Assert.ThrowsAsync<ArgumentNullException>(async () => await agent.PeekAsync(new(0), null!));

    #endregion

    #region PopAsync

    [Theory]
    [ClassData(typeof(SpaceTupleGenerator))]
    public async Task Should_PopAsync(SpaceTuple tuple)
    {
        await agent.WriteAsync(tuple);
        SpaceTuple popedTuple = await agent.PopAsync(tuple.ToTemplate());

        Assert.Equal(tuple, popedTuple);
    }

    [Theory]
    [ClassData(typeof(SpaceTupleGenerator))]
    public async Task Should_Return_Empty_Tuple_On_PopAsync(SpaceTuple tuple)
    {
        await agent.WriteAsync(tuple);
        SpaceTuple popedTuple = await agent.PopAsync(new(0));

        popedTuple.AssertEmpty();
    }

    [Theory]
    [ClassData(typeof(SpaceTupleGenerator))]
    public async Task Should_Invoke_Callback_If_Tuple_Is_Available_On_PopAsync(SpaceTuple tuple)
    {
        await agent.WriteAsync(tuple);

        SpaceTuple popedTuple = new();

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
        SpaceTuple peekedTuple = new();
        SpaceTemplate template = new(popNotAvailable);

        await agent.PopAsync(template, callback);

        ReadOnlyCollection<CallbackEntry<SpaceTuple>>? entries;
        while (!callbackRegistry.Entries.TryGetValue(template, out entries))
        {

        }

        peekedTuple.AssertEmpty();
        Assert.Equal(callback, entries.Single().Callback);

        Task callback(SpaceTuple tuple) => Task.FromResult(peekedTuple = tuple);
    }

    [Fact]
    public async Task Should_Remove_Tuple_From_Space_On_PopAsync()
    {
        SpaceTuple tuple = new(pop);

        await agent.WriteAsync(tuple);

        bool firstIteration = true;
        for (int i = 0; i < 3; i++)
        {
            SpaceTuple popedTuple = await agent.PopAsync(tuple.ToTemplate());

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

        IEnumerable<SpaceTuple> tuples = await agent.ScanAsync(new(scan, 1, typeof(string), typeof(float), null));
        Assert.Equal(3, tuples.Count());
    }

    private static IEnumerable<SpaceTuple> ScanData()
    {
        yield return new(scan, 1, "a", 1.0f, 1.0m);
        yield return new(scan, 1, "b", 1.2f, "d");
        yield return new(scan, 1, "c", 1.5f, 'e');
        yield return new(scan, 1, 1.5f, "c", 'e');
        yield return new(scan, 1, "f", 1.7f, 'g', "f");
        yield return new(scan, 2, "f", 1.7f, 'g');
        yield return new(scan, 2, "f", 1.7f, 'g', "f");
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

    #region ReloadAsync

    [Fact]
    public async Task Should_ReloadAsync()
    {
        await agent.WriteAsync(new(1));

        int count1 = await agent.CountAsync();
        await agent.ReloadAsync();
        int count2 = await agent.CountAsync();

        Assert.Equal(count1, count2);
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
