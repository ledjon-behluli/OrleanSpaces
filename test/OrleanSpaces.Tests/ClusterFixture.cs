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
    public TestCluster Cluster { get; }

    public ClusterFixture()
    {
        Cluster = new TestClusterBuilder()
            .AddSiloBuilderConfigurator<TestSiloConfigurator>()
            .AddClientBuilderConfigurator<TestClientConfigurator>()
            .Build();

        Cluster.Deploy();
    }

    public void Dispose() => Cluster.StopAllSilos();

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
