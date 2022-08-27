using Orleans;
using Orleans.Hosting;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Observers;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Clients;

namespace OrleanSpaces;

public static class Extensions
{
    public static ISiloBuilder AddTupleSpace(this ISiloBuilder builder) =>
        builder.ConfigureApplicationParts(parts =>
            parts.AddApplicationPart(typeof(Extensions).Assembly).WithReferences());
 
    public static ISiloHostBuilder AddTupleSpace(this ISiloHostBuilder builder) =>
        builder.ConfigureApplicationParts(parts =>
            parts.AddApplicationPart(typeof(Extensions).Assembly).WithReferences());

    public static IServiceCollection AddTupleSpace(
        this IServiceCollection services, 
        Func<IClusterClient>? clusterClientFactory = null)
    {
        services.AddSingleton(clusterClientFactory?.Invoke() ?? BuildInMemoryClient());
        
        services.AddSingleton<ICallbackRegistry, CallbackManager>();
        services.AddHostedService(sp => (CallbackManager)sp.GetRequiredService<ICallbackRegistry>());

        services.AddSingleton<IObserverRegistry, ObserverManager>();
        services.AddHostedService(sp => (ObserverManager)sp.GetRequiredService<IObserverRegistry>());

        services.AddSingleton<SpaceObserverBridge>();

        services.AddSingleton<IGrainFactoryProvider, Bootstrapper>();
        services.AddHostedService(sp => (Bootstrapper)sp.GetRequiredService<IGrainFactoryProvider>());

        services.AddSingleton<ISpaceClient, SpaceClient>();

        return services;

        static IClusterClient BuildInMemoryClient() =>
            new ClientBuilder()
                .UseLocalhostClustering()
                .AddSimpleMessageStreamProvider(StreamNames.PubSubProvider)
                .Build();
    }
}