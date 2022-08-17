using Orleans;
using Orleans.Hosting;
using Serialize.Linq.Serializers;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Internals;
using System.Reflection;

namespace OrleanSpaces;

internal static class This
{
    public static readonly Assembly Assembly = typeof(This).Assembly;
}

public static class ClientExtensions
{
    public static IClientBuilder ConfigureTupleSpace(this IClientBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(This.Assembly).WithReferences());
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<ExpressionSerializer>();
            services.AddSingleton<TupleFunctionSerializer>();

            services.AddSingleton<IOutgoingGrainCallFilter, TupleFunctionEvaluator>();

            services.AddSingleton(sp => sp.GetRequiredService<IGrainFactory>().GetGrain(typeof(SpaceGrain), Guid.Empty));

            services.AddSingleton(sp => (ISubscriberRegistry)sp.GetRequiredService<SpaceGrain>());
            services.AddSingleton(sp => (ITupleFunctionExecutor)sp.GetRequiredService<SpaceGrain>());
            services.AddSingleton(sp => (ISpaceProvider)sp.GetRequiredService<SpaceGrain>());
            services.AddSingleton(sp => (ISyncSpaceProvider)sp.GetRequiredService<SpaceGrain>());
        });

        return builder;
    }

    public static async Task SubscribeAsync<TObserver>(this IClusterClient client, TObserver observer) 
        where TObserver : ISpaceObserver
    {
        var registry = client.ServiceProvider.GetRequiredService<ISubscriberRegistry>();
        var observerRef = await client.CreateObjectReference<TObserver>(observer);

        await registry.AddAsync(observerRef);
    }

    public static async Task UnsubscribeAsync<TObserver>(this IClusterClient client, TObserver observer)
        where TObserver : ISpaceObserver
    {
        var registry = client.ServiceProvider.GetRequiredService<ISubscriberRegistry>();
        var observerRef = await client.CreateObjectReference<TObserver>(observer);

        await registry.RemoveAsync(observerRef);
    }
}

public static class HostingExtensions
{ 
    public static ISiloBuilder ConfigureTupleSpace(this ISiloBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(This.Assembly).WithReferences());
        builder.ConfigureServices(services => services.ConfigureSiloComponents());

        return builder;
    }

    public static ISiloHostBuilder ConfigureTupleSpace(this ISiloHostBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(This.Assembly).WithReferences());
        builder.ConfigureServices(services => services.ConfigureSiloComponents());

        return builder;
    }

    private static void ConfigureSiloComponents(this IServiceCollection services)
    {
        services.AddSingleton<ObserverManager>();

        services.AddSingleton<IIncomingGrainCallFilter, VolumeOscillationNotifier>();

        services.AddSingleton<ExpressionSerializer>();
        services.AddSingleton<TupleFunctionSerializer>();
    }
}

