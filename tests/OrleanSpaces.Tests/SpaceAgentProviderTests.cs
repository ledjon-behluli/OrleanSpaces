using Microsoft.Extensions.DependencyInjection;
using Orleans;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tests;

public class SpaceAgentProviderTests : IClassFixture<ClusterFixture>
{
    private readonly IClusterClient client;
    private readonly ISpaceAgentProvider spaceChannel;

    public SpaceAgentProviderTests(ClusterFixture fixture)
    {
        client = fixture.Client;
        spaceChannel = fixture.Client.ServiceProvider.GetRequiredService<ISpaceAgentProvider>();
    }

    [Fact]
    public async Task Should_Get_Agent()
    {
        Assert.NotNull(await spaceChannel.GetAsync());
    }

    [Fact]
    public async Task Should_Get_Same_Agent_When_Called_Multiple_Times()
    {
        ISpaceAgent agent1 = await spaceChannel.GetAsync();
        ISpaceAgent agent2 = await spaceChannel.GetAsync();

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

        var grain = client.GetGrain<ISpaceGrain>(Constants.SpaceGrainId);
        var streamId = await grain.ListenAsync();
        var provider = client.GetStreamProvider(Constants.PubSubProvider);
        var stream = provider.GetStream<ITuple>(streamId, Constants.TupleStream);

        var subscriptions = await stream.GetAllSubscriptionHandles();
        Assert.Equal(1, subscriptions.Count);
    }

    private async Task<ISpaceAgent> GetAgentConcurrently()
    {
        ISpaceAgent agent = null;
        var tasks = new Task[100];

        for (int i = 0; i < 100; i++)
        {
            tasks[i] = Task.Run(async () =>
            {
                agent = await spaceChannel.GetAsync();
            });
        }

        await Task.WhenAll(tasks);

        return agent;
    }
}