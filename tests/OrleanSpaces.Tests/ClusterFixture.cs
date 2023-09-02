﻿using Microsoft.Extensions.Configuration;
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
        public void Configure(ISiloBuilder siloBuilder)
        {
            // we dont register (though we could) the space services in the silo, since we are using the client to test.

            siloBuilder.AddMemoryStreams(Constants.PubSubProvider);
            siloBuilder.AddMemoryGrainStorage(Constants.PubSubStore);
            siloBuilder.AddMemoryGrainStorage(Constants.Store_StorageName);
        }
    }

    private class TestClientConfigurator : IClientBuilderConfigurator
    {
        public void Configure(IConfiguration configuration, IClientBuilder clientBuilder)
        {
            clientBuilder.AddOrleanSpaces(options => options.EnabledSpaces = SpaceKind.All);
            clientBuilder.AddMemoryStreams(Constants.PubSubProvider);
        }
    }
}
