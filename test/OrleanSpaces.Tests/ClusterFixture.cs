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
            .AddSiloBuilderConfigurator<TestSiloConfigurations>()
            .Build();

        Cluster.Deploy();
    }

    public void Dispose() => Cluster.StopAllSilos();

    private class TestSiloConfigurations : ISiloConfigurator
    {
        public void Configure(ISiloBuilder siloBuilder)
        {
            siloBuilder.AddTupleSpace();
            siloBuilder.AddSimpleMessageStreamProvider(StreamNames.PubSubProvider);
            siloBuilder.AddMemoryGrainStorage(StreamNames.PubSubStore);
            siloBuilder.AddMemoryGrainStorage(StorageNames.TupleSpaceStore);
        }
    }
}
