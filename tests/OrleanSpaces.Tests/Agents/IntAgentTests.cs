﻿using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;
using System.Collections.ObjectModel;

namespace OrleanSpaces.Tests.Agents;

public class IntAgentTests : IClassFixture<ClusterFixture>
{
    const int routingKey = 100;
    const int peek = 101;
    const int pop = 102;
    const int enumerate = 103;
    const int consume = 104;
    const int peekNotAvailable = 105;
    const int popNotAvailable = 106;

    private readonly ISpaceAgent<int, IntTuple, IntTemplate> agent;
    private readonly ISpaceRouter<IntTuple, IntTemplate> router;
    private readonly ObserverRegistry<IntTuple> observerRegistry;
    private readonly CallbackRegistry<int, IntTuple, IntTemplate> callbackRegistry;
    private readonly EvaluationChannel<IntTuple> evaluationChannel;
   
    public IntAgentTests(ClusterFixture fixture)
    {
        agent = fixture.Client.ServiceProvider.GetRequiredService<ISpaceAgent<int, IntTuple, IntTemplate>>();
        router = fixture.Client.ServiceProvider.GetRequiredService<ISpaceRouter<IntTuple, IntTemplate>>();
        observerRegistry = fixture.Client.ServiceProvider.GetRequiredService<ObserverRegistry<IntTuple>>();
        callbackRegistry = fixture.Client.ServiceProvider.GetRequiredService<CallbackRegistry<int, IntTuple, IntTemplate>>();
        evaluationChannel = fixture.Client.ServiceProvider.GetRequiredService<EvaluationChannel<IntTuple>>();
    }

    #region Count

    [Fact]
    public async Task Should_Count()
    {
        await agent.ClearAsync();
        Assert.Equal(0, agent.Count);

        await agent.WriteAsync(new(1));
        await agent.WriteAsync(new(1));
        await agent.WriteAsync(new(2));

        Assert.Equal(3, agent.Count);
    }

    #endregion

    #region Subscribe

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

    static readonly IntTuple routingTuple = new(routingKey);

    [Fact]
    public async Task Should_WriteAsync_When_Routing_Tuple()
    {
        await router.RouteTuple(routingTuple);

        IntTuple peekedTuple = agent.Peek(routingTuple.ToTemplate());

        Assert.Equal(routingTuple, peekedTuple);
    }

    [Fact]
    public async Task Should_PopAsync_When_Routing_Template()
    {
        IntTemplate template = routingTuple.ToTemplate();

        await router.RouteTemplate(template);

        IntTuple peekedTuple = agent.Peek(template);

        Assert.NotEqual(routingTuple, peekedTuple);
    }

    #endregion

    #region Write

    [Fact]
    public async Task Should_WriteAsync()
    {
        IntTuple tuple = new(1);
        await agent.WriteAsync(tuple);

        IntTuple peekedTuple = agent.Peek(tuple.ToTemplate());

        Assert.Equal(tuple, peekedTuple);
    }

    [Fact]
    public Task Should_Throw_On_WriteAsync_If_Empty_Tuple()
        => Assert.ThrowsAsync<ArgumentException>(async () => await agent.WriteAsync(new()));

    #endregion

    #region Evaluate

    [Fact]
    public async Task Should_EvaluateAsync()
    {
        IntTuple tuple = new(1);
        await agent.EvaluateAsync(() => Task.FromResult(tuple));

        Func<Task<IntTuple>> evaluation = await evaluationChannel.Reader.ReadAsync();

        Assert.Equal(tuple, await evaluation());
    }

    [Fact]
    public Task Should_Throw_On_EvaluateAsync_If_Null_Eval_Function()
        => Assert.ThrowsAsync<ArgumentNullException>(async () => await agent.EvaluateAsync(null!));

    #endregion

    #region Peek

    [Theory]
    [ClassData(typeof(IntTupleGenerator))]
    public async Task Should_Peek(IntTuple tuple)
    {
        await agent.WriteAsync(tuple);
        IntTuple peekedTuple = agent.Peek(tuple.ToTemplate());

        Assert.Equal(tuple, peekedTuple);
    }

    [Theory]
    [ClassData(typeof(IntTupleGenerator))]
    public async Task Should_Return_Empty_Tuple_On_Peek(IntTuple tuple)
    {
        await agent.WriteAsync(tuple);
        IntTuple peekedTuple = agent.Peek(new(0));

        peekedTuple.AssertEmpty();
    }

    [Fact]
    public async Task Should_Keep_Tuple_In_Space_On_Peek()
    {
        IntTuple tuple = new(peek);

        await agent.WriteAsync(tuple);

        for (int i = 0; i < 3; i++)
        {
            IntTuple peekedTuple = agent.Peek(tuple.ToTemplate());
            Assert.Equal(tuple, peekedTuple);
        }
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
    public Task Should_Throw_On_PeekAsync_If_Null_Callback()
        => Assert.ThrowsAsync<ArgumentNullException>(async () => await agent.PeekAsync(new(0), null!));

    #endregion

    #region Pop

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

    #region Enumerate

    [Fact]
    public async Task Should_Enumerate()
    {
        List<IntTuple> tuples = new()
        {
            new(enumerate, 1, 2, 3, 4),
            new(enumerate, 1, 2, 4, 5),
            new(enumerate, 1, 2, 3, 6),
            new(enumerate, 1, 2, 4, 5),
            new(enumerate, 1, 3, 4, 7),
            new(enumerate, 1, 1, 6, 5),
            new(enumerate, 1, 2, 3, 9)
        };

        foreach (var tuple in tuples)
        {
            await agent.WriteAsync(tuple);
        }

        IEnumerable<IntTuple> resultAll = agent.Enumerate();
        IEnumerable<IntTuple> resultFiltered = agent.Enumerate(new(enumerate, 1, 2, 3, null));

        Assert.Equal(7, resultAll.Count());
        Assert.Equal(3, resultFiltered.Count());
    }

    [Fact]
    public async Task Should_EnumerateAsync()
    {
        IntTuple[] tuples = new IntTuple[5]
        {
            new(consume, 0),
            new(consume, 1),
            new(consume, 2),
            new(consume, 2),
            new(consume, 4)
        };

        _ = Task.Run(async () =>
        {
            int i = 1;

            await foreach (IntTuple tuple in agent.EnumerateAsync())
            {
                Assert.Equal(tuples[i], tuple);
                i++;
            }

            Assert.Equal(5, i);
        });

        _ = Task.Run(async () =>
        {
            int i = 1;

            await foreach (IntTuple tuple in agent.EnumerateAsync(new(consume, 2)))
            {
                Assert.Equal(tuples[i], tuple);
                i++;
            }

            Assert.Equal(2, i);
        });

        int i = 0;
        while (i < 5)
        {
            await agent.WriteAsync(tuples[i]);
            i++;
        }
    }

    #endregion

    #region Reload

    [Fact]
    public async Task Should_ReloadAsync()
    {
        await agent.WriteAsync(new(1));

        int count1 = agent.Count;
        await agent.ReloadAsync();
        int count2 = agent.Count;

        Assert.Equal(count1, count2);
    }

    #endregion

    #region Clear

    [Fact]
    public async Task Should_ClearAsync()
    {
        await agent.WriteAsync(new(1));
        Assert.NotEqual(0, agent.Count);

        await agent.ClearAsync();
        Assert.Equal(0, agent.Count);
    }

    #endregion
}