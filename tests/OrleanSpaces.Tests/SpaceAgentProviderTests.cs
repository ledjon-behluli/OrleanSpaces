using Microsoft.Extensions.DependencyInjection;
using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Tests;

public class SpaceAgentProviderTests : IClassFixture<ClusterFixture>
{
    private readonly IClusterClient client;
    private readonly ISpaceAgentProvider provider;

    public SpaceAgentProviderTests(ClusterFixture fixture)
    {
        client = fixture.Client;
        provider = fixture.Client.ServiceProvider.GetRequiredService<ISpaceAgentProvider>();
    }

    [Fact]
    public async Task Should_Get_Agent()
    {
        Assert.NotNull(await provider.GetAsync());
    }

    [Fact]
    public async Task Should_Get_Same_Agent_When_Called_Multiple_Times()
    {
        ISpaceAgent agent1 = await provider.GetAsync();
        ISpaceAgent agent2 = await provider.GetAsync();

        Assert.Equal(agent1, agent2);
        Assert.True(agent1 == agent2);
        Assert.True(agent1.Equals(agent2));
    }

    [Fact]
    public void Should_Not_Get_Agent_From_Interface_Via_ServiceProvider()
    {
        Assert.Null(client.ServiceProvider.GetService<ISpaceAgent>());
    }

    [Fact]
    public async Task Should_Not_Throw_When_Invoked_Concurrently()
    {
        var expection = await Record.ExceptionAsync(async () => _ = await GetAgentConcurrently());
        Assert.Null(expection);
    }

    [Fact]
    public async Task Should_Subscribe_Once_When_Invoked_Concurrently()
    {
        _ = await GetAgentConcurrently();

        var grain = client.GetGrain<ISpaceGrain>(ISpaceGrain.Key);
        StreamId streamId = await grain.GetStreamId();
        var stream = client.GetStreamProvider(Constants.PubSubProvider).GetStream<TupleAction<SpaceTuple>>(streamId);
        var subscriptions = await stream.GetAllSubscriptionHandles();

        Assert.Equal(1, subscriptions.Count);
    }

    private async Task<ISpaceAgent> GetAgentConcurrently()
    {
        ISpaceAgent agent = null!;
        var tasks = new Task[100];

        for (int i = 0; i < 100; i++)
        {
            tasks[i] = Task.Run(async () =>
            {
                agent = await provider.GetAsync();
            });
        }

        await Task.WhenAll(tasks);

        return agent;
    }
}

public class IntAgentProviderTests : IClassFixture<ClusterFixture>
{
    private readonly IClusterClient client;
    private readonly ISpaceAgentProvider<int, IntTuple, IntTemplate> provider;

    public IntAgentProviderTests(ClusterFixture fixture)
    {
        client = fixture.Client;
        provider = fixture.Client.ServiceProvider.GetRequiredService<ISpaceAgentProvider<int, IntTuple, IntTemplate>>();
    }

    [Fact]
    public async Task Should_Get_Agent()
    {
        Assert.NotNull(await provider.GetAsync());
    }

    [Fact]
    public async Task Should_Get_Same_Agent_When_Called_Multiple_Times()
    {
        var agent1 = await provider.GetAsync();
        var agent2 = await provider.GetAsync();

        Assert.Equal(agent1, agent2);
        Assert.True(agent1 == agent2);
        Assert.True(agent1.Equals(agent2));
    }

    [Fact]
    public void Should_Not_Get_Agent_From_Interface_Via_ServiceProvider()
    {
        Assert.Null(client.ServiceProvider.GetService<ISpaceAgent<int, IntTuple, IntTemplate>>());
    }

    [Fact]
    public async Task Should_Not_Throw_When_Invoked_Concurrently()
    {
        var expection = await Record.ExceptionAsync(async () => _ = await GetAgentConcurrently());
        Assert.Null(expection);
    }

    [Fact]
    public async Task Should_Subscribe_Once_When_Invoked_Concurrently()
    {
        _ = await GetAgentConcurrently();

        var grain = client.GetGrain<IIntGrain>(IIntGrain.Key);
        StreamId streamId = await grain.GetStreamId();
        var stream = client.GetStreamProvider(Constants.PubSubProvider).GetStream<TupleAction<IntTuple>>(streamId);
        var subscriptions = await stream.GetAllSubscriptionHandles();

        Assert.Equal(1, subscriptions.Count);
    }

    private async Task<ISpaceAgent<int, IntTuple, IntTemplate>> GetAgentConcurrently()
    {
        ISpaceAgent<int, IntTuple, IntTemplate> agent = null!;
        var tasks = new Task[100];

        for (int i = 0; i < 100; i++)
        {
            tasks[i] = Task.Run(async () =>
            {
                agent = await provider.GetAsync();
            });
        }

        await Task.WhenAll(tasks);

        return agent;
    }
}