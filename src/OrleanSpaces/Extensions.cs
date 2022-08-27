using Orleans;
using Orleans.Hosting;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Observers;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Proxies;

namespace OrleanSpaces;

public static class Extensions
{
    public static ISiloBuilder AddTupleSpace(this ISiloBuilder builder) =>
        builder.ConfigureApplicationParts(parts =>
            parts.AddApplicationPart(typeof(Extensions).Assembly).WithReferences());
 
    public static ISiloHostBuilder AddTupleSpace(this ISiloHostBuilder builder) =>
        builder.ConfigureApplicationParts(parts =>
            parts.AddApplicationPart(typeof(Extensions).Assembly).WithReferences());

    public static IClientBuilder AddTupleSpace(this IClientBuilder builder) =>
        builder.ConfigureServices(services => services.AddClientServices());

    public static IServiceCollection AddTupleSpace(
        this IServiceCollection services,
        Func<IClusterClient>? clusterClientFactory = null)
    {
        services.AddSingleton(clusterClientFactory?.Invoke() ?? BuildDefaultClient());
        services.AddClientServices();

        return services;

        static IClusterClient BuildDefaultClient() =>
            new ClientBuilder()
                .UseLocalhostClustering()
                .AddSimpleMessageStreamProvider(StreamNames.PubSubProvider)
                .Build();
    }

    private static IServiceCollection AddClientServices(this IServiceCollection services)
    {
        services.AddSingleton<ICallbackRegistry, CallbackManager>();
        services.AddHostedService(sp => (CallbackManager)sp.GetRequiredService<ICallbackRegistry>());

        services.AddSingleton<IObserverRegistry, ObserverManager>();
        services.AddHostedService(sp => (ObserverManager)sp.GetRequiredService<IObserverRegistry>());

        services.AddSingleton<SpaceAgent>();
        services.AddSingleton<ISpaceChannelProxy, ChannelProxy>();

        return services;
    }
}