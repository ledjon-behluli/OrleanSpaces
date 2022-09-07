using Orleans;
using Orleans.Hosting;
using OrleanSpaces.Observers;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Continuations;
using Microsoft.Extensions.DependencyInjection;

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

    public static IServiceCollection AddTupleSpace(this IServiceCollection services, Func<IClusterClient>? clusterClientFactory = null)
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

        services.AddSingleton<CallbackChannel>();
        services.AddSingleton<EvaluationChannel>();
        services.AddSingleton<ContinuationChannel>();
        services.AddSingleton<ObserverChannel>();

        services.AddSingleton<SpaceAgent>();
        services.AddSingleton<ISpaceTupleRouter>(sp => sp.GetRequiredService<SpaceAgent>());
        services.AddSingleton<ISpaceChannel, SpaceChannel>();

        services.AddHostedService<CallbackProcessor>();
        services.AddHostedService<EvaluationProcessor>();
        services.AddHostedService<ContinuationProcessor>();
        services.AddHostedService<ObserverProcessor>();

        return services;
    }
}