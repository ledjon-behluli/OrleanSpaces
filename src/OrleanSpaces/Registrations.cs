using Orleans;
using Orleans.Hosting;
using OrleanSpaces.Observers;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Continuations;
using Microsoft.Extensions.DependencyInjection;

namespace OrleanSpaces;

public static class Registrations
{
    public static ISiloBuilder AddTupleSpace(this ISiloBuilder builder) =>
        builder.ConfigureApplicationParts(parts =>
            parts.AddApplicationPart(typeof(Registrations).Assembly).WithReferences());

    public static ISiloHostBuilder AddTupleSpace(this ISiloHostBuilder builder) =>
        builder.ConfigureApplicationParts(parts =>
            parts.AddApplicationPart(typeof(Registrations).Assembly).WithReferences());

    public static IClientBuilder AddTupleSpace(this IClientBuilder builder) =>
        builder.ConfigureServices(services => services.AddClientServices());

    public static IServiceCollection UseLocalhostTupleSpace(this IServiceCollection services, Func<IClusterClient>? clusterClientFactory = null)
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
        services.AddSingleton<CallbackRegistry>();
        services.AddSingleton<ObserverRegistry>();

        services.AddSingleton<ISpaceChannel, SpaceChannel>();

        services.AddHostedService<CallbackProcessor>();
        services.AddHostedService<EvaluationProcessor>();
        services.AddHostedService<ContinuationProcessor>();
        services.AddHostedService<ObserverProcessor>();

        return services;
    }
}