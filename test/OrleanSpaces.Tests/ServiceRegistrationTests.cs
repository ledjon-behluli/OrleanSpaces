using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using Orleans.TestingHost;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Continuations;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;

namespace OrleanSpaces.Tests;

[Collection(ClusterCollection.Name)]
public class ServiceRegistrationTests
{
    private readonly TestCluster cluster;

    public ServiceRegistrationTests(ClusterFixture fixture)
    {
        cluster = fixture.Cluster;
    }

    [Fact]
    public void Should_Register_Services_On_ServiceCollection()
    {
        ServiceCollection services = new();

        services.AddLogging();
        services.AddTupleSpace();

        var provider = services.BuildServiceProvider();

        EnsureServices(provider);
    }

    [Fact]
    public void Should_Register_Services_On_ClientBuilder()
    {
        EnsureServices(
            new ClientBuilder()
                .UseLocalhostClustering()
                .AddTupleSpace()
                .Build()
                .ServiceProvider);
    }

    [Fact]
    public void Should_Register_Services_With_CustomClusterClient_On_ServiceCollection()
    {
        Func<IClusterClient> clientFactory = () => cluster.Client;
        ServiceCollection services = new();

        services.AddLogging();
        services.AddTupleSpace(clientFactory);

        var provider = services.BuildServiceProvider();
        var client = provider.GetService<IClusterClient>();

        Assert.Equal(clientFactory(), client);
        EnsureServices(provider);
    }

    private static void EnsureServices(IServiceProvider provider)
    {
        Assert.NotNull(provider.GetService<CallbackRegistry>());
        Assert.NotNull(provider.GetService<ObserverRegistry>());

        Assert.All(provider.GetService<IEnumerable<IHostedService>>(), service =>
        {
            Assert.NotNull(service);
            Assert.True(
                service.GetType() == typeof(CallbackProcessor) ||
                service.GetType() == typeof(EvaluationProcessor) ||
                service.GetType() == typeof(ContinuationProcessor) ||
                service.GetType() == typeof(ObserverProcessor));
        });

        Assert.NotNull(provider.GetService<ISpaceChannel>());
        Assert.NotNull(provider.GetService<IClusterClient>());
    }
}