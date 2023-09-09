using Microsoft.Extensions.Configuration;
using Orleans.TestingHost;

namespace OrleanSpaces.Tests;

public class ClusterFixture : IDisposable
{
    private readonly TestCluster cluster;
    public IClusterClient Client { get; }

    public ClusterFixture()
    {
        cluster = new TestClusterBuilder()
            .AddSiloBuilderConfigurator<TestSiloConfigurator>()
            .AddClientBuilderConfigurator<TestClientConfigurator>()
        .Build();

        cluster.Deploy();

        Client = cluster.Client;
    }

    public void Dispose()
    {
        cluster.StopAllSilos();
        GC.SuppressFinalize(this);
    }

    private class TestSiloConfigurator : ISiloConfigurator
    {
        public void Configure(ISiloBuilder builder) =>
            builder.AddOrleanSpaces()
                .AddMemoryStreams(Constants.PubSubProvider)
                .AddMemoryGrainStorage(Constants.PubSubStore)
                .AddMemoryGrainStorage(Constants.StorageName);
    }

    private class TestClientConfigurator : IClientBuilderConfigurator
    {
        public void Configure(IConfiguration configuration, IClientBuilder builder) =>
            builder.AddOrleanSpaces(options => options.EnabledSpaces = SpaceKind.All)
                   .AddMemoryStreams(Constants.PubSubProvider);
    }
}
