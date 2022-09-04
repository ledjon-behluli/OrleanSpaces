using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Hosting;
using Orleans.TestingHost;

namespace OrleanSpaces.Tests;

[CollectionDefinition(Name)]
public class ClusterCollection : ICollectionFixture<ClusterFixture>
{
    public const string Name = "ClusterCollection";
}

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

    public void Dispose() => cluster.StopAllSilos();

    private class TestSiloConfigurator : ISiloConfigurator
    {
        public void Configure(ISiloBuilder siloBuilder)
        {
            siloBuilder.AddSimpleMessageStreamProvider(StreamNames.PubSubProvider);
            siloBuilder.AddMemoryGrainStorage(StreamNames.PubSubStore);
            siloBuilder.AddMemoryGrainStorage(StorageNames.TupleSpaceStore);
            siloBuilder.AddTupleSpace();
        }
    }

    private class TestClientConfigurator : IClientBuilderConfigurator
    {
        public void Configure(IConfiguration configuration, IClientBuilder clientBuilder)
        {
            clientBuilder.AddSimpleMessageStreamProvider(StreamNames.PubSubProvider);
            clientBuilder.AddTupleSpace();
        }
    }
}
