using Orleans;
using Orleans.Hosting;
using OrleanSpaces.Observers;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Continuations;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Agents;

namespace OrleanSpaces;

public static class Extensions
{
    /// <summary>
    /// Configures the tuple space on the silo.
    /// </summary>
    public static ISiloBuilder AddTupleSpace(this ISiloBuilder builder) =>
        builder.ConfigureApplicationParts(parts =>
            parts.AddApplicationPart(typeof(Extensions).Assembly).WithReferences());

    /// <summary>
    /// Configures the tuple space on the silo.
    /// </summary>
    public static ISiloHostBuilder AddTupleSpace(this ISiloHostBuilder builder) =>
        builder.ConfigureApplicationParts(parts =>
            parts.AddApplicationPart(typeof(Extensions).Assembly).WithReferences());

    /// <summary>
    /// Configures the tuple space on the client.
    /// </summary>
    public static IClientBuilder AddTupleSpace(this IClientBuilder builder) =>
        builder.ConfigureServices(services => services.AddClientServices());

    /// <summary>
    /// Configures the tuple space on the service collection.
    /// </summary>
    /// <param name="services"/>
    /// <param name="clusterClientFactory">An optional delegate that returns an <see cref="IClusterClient"/> to be used.<br/>
    /// <i>If omitted, then localhost clustering and simple message stream provider are used instead.</i></param>
    public static IServiceCollection AddTupleSpace(this IServiceCollection services, Func<IClusterClient>? clusterClientFactory = null)
    {
        services.AddSingleton(clusterClientFactory?.Invoke() ?? BuildDefaultClient());
        services.AddClientServices();

        return services;

        static IClusterClient BuildDefaultClient() =>
            new ClientBuilder()
                .UseLocalhostClustering()
                .AddSimpleMessageStreamProvider(Constants.PubSubProvider)
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
        services.AddSingleton<ITupleRouter>(sp => sp.GetRequiredService<SpaceAgent>());
        services.AddSingleton<ISpaceAgentProvider, SpaceAgentProvider>();

        services.AddHostedService<CallbackProcessor>();
        services.AddHostedService<EvaluationProcessor>();
        services.AddHostedService<TupleContinuationProcessor>();
        services.AddHostedService<TupleObserverProcessor>();

        return services;
    }
}