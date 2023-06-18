using Orleans;
using Orleans.Hosting;
using OrleanSpaces.Observers;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Continuations;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Agents;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;

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
        #region Generic

        services.AddSingleton<CallbackRegistry<SpaceTuple, SpaceTemplate>>();
        services.AddSingleton<ObserverRegistry<SpaceTuple>>();

        services.AddSingleton<CallbackChannel<SpaceTuple>>();
        services.AddSingleton<EvaluationChannel<SpaceTuple>>();
        services.AddSingleton<ContinuationChannel<SpaceTuple, SpaceTemplate>>();
        services.AddSingleton<ObserverChannel<SpaceTuple>>();

        services.AddSingleton<SpaceAgent>();
        services.AddSingleton<ITupleRouter<SpaceTuple, SpaceTemplate>>(sp => sp.GetRequiredService<SpaceAgent>());
        services.AddSingleton<ISpaceAgentProvider, SpaceAgentProvider>();

        services.AddHostedService<CallbackProcessor<SpaceTuple, SpaceTemplate>>();
        services.AddHostedService<EvaluationProcessor<SpaceTuple, SpaceTemplate>>();
        services.AddHostedService<ContinuationProcessor<SpaceTuple, SpaceTemplate>>();
        services.AddHostedService<ObserverProcessor<SpaceTuple>>();

        #endregion

        #region Int

        services.AddSingleton<CallbackRegistry<IntTuple, IntTemplate>>();
        services.AddSingleton<ObserverRegistry<IntTuple>>();

        services.AddSingleton<CallbackChannel<IntTuple>>();
        services.AddSingleton<EvaluationChannel<IntTuple>>();
        services.AddSingleton<ContinuationChannel<IntTuple, IntTemplate>>();
        services.AddSingleton<ObserverChannel<IntTuple>>();

        services.AddSingleton<IntAgent>();
        services.AddSingleton<ITupleRouter<IntTuple, IntTemplate>>(sp => sp.GetRequiredService<IntAgent>());
        services.AddSingleton<IIntAgentProvider, IntAgentProvider>();

        services.AddHostedService<CallbackProcessor<IntTuple, IntTemplate>>();
        services.AddHostedService<EvaluationProcessor<IntTuple, IntTemplate>>();
        services.AddHostedService<ContinuationProcessor<IntTuple, IntTemplate>>();
        services.AddHostedService<ObserverProcessor<IntTuple>>();

        #endregion

        return services;
    }
}