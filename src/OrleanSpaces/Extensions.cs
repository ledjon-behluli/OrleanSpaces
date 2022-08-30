using Orleans;
using Orleans.Hosting;
using OrleanSpaces.Bridges;
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

    public static IClientBuilder AddTupleSpace(this IClientBuilder builder, Action<SpaceOptions>? config = null) =>
        builder.ConfigureServices(services => services.AddClientServices(config));

    public static IServiceCollection AddTupleSpace(this IServiceCollection services,
        Action<SpaceOptions>? config = null, Func<IClusterClient>? factory = null)
    {
        services.AddSingleton(factory?.Invoke() ?? BuildDefaultClient());
        services.AddClientServices(config);

        return services;

        static IClusterClient BuildDefaultClient() =>
            new ClientBuilder()
                .UseLocalhostClustering()
                .AddSimpleMessageStreamProvider(StreamNames.PubSubProvider)
                .Build();
    }

    private static IServiceCollection AddClientServices(this IServiceCollection services, Action<SpaceOptions>? config = null)
    {
        SpaceOptions options = new();
        config?.Invoke(options);

        services.AddSingleton(options);

        services.AddSingleton<SpaceAgent>();
        services.AddSingleton<ISpaceChannelProvider, SpaceChannelBridge>();

        if (options.UseObservers)
        {
            services.AddSingleton<ObserverRegistry>();
            services.AddHostedService<ObserverProcessor>();
        }

        if (options.UseEvaluators)
        {
            services.AddHostedService<EvaluationProcessor>();
        }

        if (options.UseCallbackReaders)
        {
            services.AddSingleton<CallbackRegistry>();
            services.AddHostedService<CallbackProcessor>();
        }

        if (options.UseEvaluators || options.UseCallbackReaders)
        {
            services.AddHostedService<ContinuationProcessor>();
        }

        return services;
    }
}

public sealed class SpaceOptions
{
    public bool UseObservers { get; set; } = true;
    public bool UseEvaluators { get; set; } = true;
    public bool UseCallbackReaders { get; set; } = true;
}