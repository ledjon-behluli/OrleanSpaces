using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Orleans;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests;

[Collection(ClusterCollection.Name)]
public class SpaceChannelTests
{
    private readonly IClusterClient client;
    private readonly SpaceChannel channel;

    public SpaceChannelTests(ClusterFixture fixture)
    {
        client = fixture.Cluster.Client;
        channel = new SpaceChannel(new NullLogger<SpaceChannel>(), client, new(), new());
    }

    [Fact]
    public async Task Should_Get_Agent()
    {
        Assert.NotNull(await channel.GetAsync());
    }

    [Fact]
    public async Task Should_Get_Same_Agent_When_Called_Multiple_Times()
    {
        ISpaceAgent agent1 = await channel.GetAsync();
        ISpaceAgent agent2 = await channel.GetAsync();

        Assert.Equal(agent1, agent2);
        Assert.True(agent1 == agent2);
        Assert.True(agent1.Equals(agent2));
    }

    [Fact]
    public void Should_Not_Get_Agent_From_ServiceProvider()
    {
        Assert.Throws<InvalidOperationException>(() => client.ServiceProvider.GetRequiredService<ISpaceAgent>());
        Assert.Null(client.ServiceProvider.GetService<ISpaceAgent>());
    }

    [Fact]
    public async Task Should_Not_Throw_When_Multiple_Threads_Open_The_Channel()
    {
        var expection = await Record.ExceptionAsync(async () => _ = await GetAgentConcurrently());
        Assert.Null(expection);
    }

    [Fact]
    public async Task Should_Subscribe_Only_Once_Even_When_Multiple_Threads_Initialize_The_Agent()
    {
        ISpaceAgent agent = await GetAgentConcurrently();
        await agent.WriteAsync(SpaceTuple.Create(1));

        Assert.Equal(1, CallbackChannel.Reader.Count);
        Assert.Equal(1, ObserverChannel.Reader.Count);
    }

    private async Task<ISpaceAgent> GetAgentConcurrently()
    {
        ISpaceAgent agent = null;
        var tasks = new Task[100];

        for (int i = 0; i < 100; i++)
        {
            tasks[i] = Task.Run(async () =>
            {
                agent = await channel.GetAsync();
            });
        }

        await Task.WhenAll(tasks);

        return agent;
    }
}