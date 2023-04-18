using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Hosting;
using Orleans.TestingHost;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Continuations;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Observers;
using Microsoft.Extensions.DependencyInjection;

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

        Client.ServiceProvider.GetRequiredService<EvaluationChannel>().IsBeingConsumed = true;
        Client.ServiceProvider.GetRequiredService<CallbackChannel>().IsBeingConsumed = true;
        Client.ServiceProvider.GetRequiredService<ContinuationChannel>().IsBeingConsumed = true;
        Client.ServiceProvider.GetRequiredService<ObserverChannel>().IsBeingConsumed = true;
    }

    public void Dispose()
    {
        cluster.StopAllSilos();
        GC.SuppressFinalize(this);
    }

    private class TestSiloConfigurator : ISiloConfigurator
    {
        public void Configure(ISiloBuilder siloBuilder)
        {
            siloBuilder.AddSimpleMessageStreamProvider(Constants.PubSubProvider);
            siloBuilder.AddMemoryGrainStorage(Constants.PubSubStore);
            siloBuilder.AddMemoryGrainStorage(Constants.TupleSpaceStore);
            siloBuilder.AddTupleSpace();
        }
    }

    private class TestClientConfigurator : IClientBuilderConfigurator
    {
        public void Configure(IConfiguration configuration, IClientBuilder clientBuilder)
        {
            clientBuilder.AddSimpleMessageStreamProvider(Constants.PubSubProvider);
            clientBuilder.AddTupleSpace();
        }
    }
}
