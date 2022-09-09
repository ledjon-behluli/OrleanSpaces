using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Continuations;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Observers;

namespace OrleanSpaces.Tests;

public class ExtensionsTests : IClassFixture<ClusterFixture>
{
    private readonly IClusterClient client;

    public ExtensionsTests(ClusterFixture fixture)
    {
        client = fixture.Client;
    }

    [Fact]
    public void Should_Add_ApplicationParts_On_SiloHostBuilder()
    {
        var manager = new SiloHostBuilder()
            .AddTupleSpace()
            .GetApplicationPartManager();

        Assert.True(manager.ApplicationParts.Any());
    }

    [Fact]
    public void Should_Register_Services_On_ServiceCollection()
    {
        ServiceCollection services = new();

        services.AddSingleton<IHostApplicationLifetime, TestHostAppLifetime>();
        services.AddTupleSpace();

        var provider = services.BuildServiceProvider();

        EnsureAdded(provider);
    }

    [Fact]
    public void Should_Register_Services_On_ClientBuilder()
    {
        EnsureAdded(
            new ClientBuilder()
                .UseLocalhostClustering()
                .ConfigureServices(sp => sp.AddSingleton<IHostApplicationLifetime, TestHostAppLifetime>())
                .AddTupleSpace()
                .Build()
                .ServiceProvider);
    }

    [Fact]
    public void Should_Register_Services_With_CustomClusterClient_On_ServiceCollection()
    {
        Func<IClusterClient> clientFactory = () => this.client;
        ServiceCollection services = new();

        services.AddSingleton<IHostApplicationLifetime, TestHostAppLifetime>();
        services.AddTupleSpace(clientFactory);

        var provider = services.BuildServiceProvider();
        var client = provider.GetService<IClusterClient>();

        Assert.Equal(clientFactory(), client);
        EnsureAdded(provider);
    }

    private static void EnsureAdded(IServiceProvider provider)
    {
        Assert.NotNull(provider.GetService<CallbackRegistry>());
        Assert.NotNull(provider.GetService<ObserverRegistry>());

        Assert.NotNull(provider.GetService<CallbackChannel>());
        Assert.NotNull(provider.GetService<EvaluationChannel>());
        Assert.NotNull(provider.GetService<ContinuationChannel>());
        Assert.NotNull(provider.GetService<ObserverChannel>());

        Assert.NotNull(provider.GetService<SpaceAgent>());
        Assert.NotNull(provider.GetService<ISpaceTupleRouter>());
        Assert.NotNull(provider.GetService<ISpaceChannel>());
        Assert.NotNull(provider.GetService<IClusterClient>());

        Assert.All(provider.GetService<IEnumerable<IHostedService>>(), service =>
        {
            Assert.NotNull(service);
            Assert.True(
                service.GetType() == typeof(CallbackProcessor) ||
                service.GetType() == typeof(EvaluationProcessor) ||
                service.GetType() == typeof(ContinuationProcessor) ||
                service.GetType() == typeof(ObserverProcessor));
        });
    }
}