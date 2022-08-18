using Orleans;
using Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Internals;
using System.Reflection;
using OrleanSpaces.Internals.Observations;
using OrleanSpaces.Internals.Evaluations;

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
            services.AddSingleton<TupleFunctionSerializer>();
            services.AddSingleton<IOutgoingGrainCallFilter, TupleFunctionEvaluator>();

            services.AddSingleton(sp => sp.GetRequiredService<IGrainFactory>().GetGrain(typeof(TupleSpace), Guid.Empty));

            services.AddSingleton(sp => (ISpaceObserverRegistry)sp.GetRequiredService<TupleSpace>());
            services.AddSingleton(sp => (ITupleFunctionExecutor)sp.GetRequiredService<TupleSpace>());
            services.AddSingleton(sp => (ISpaceProvider)sp.GetRequiredService<TupleSpace>());
            services.AddSingleton(sp => (ISyncSpaceProvider)sp.GetRequiredService<TupleSpace>());
        });

        return builder;
    }

    public static async Task SubscribeAsync<TObserver>(this IClusterClient client, TObserver observer) 
        where TObserver : ISpaceObserver
    {
        var registry = client.ServiceProvider.GetRequiredService<ISpaceObserverRegistry>();
        var observerRef = await client.CreateObjectReference<TObserver>(observer);

        registry.Register(observerRef);
    }

    public static async Task UnsubscribeAsync<TObserver>(this IClusterClient client, TObserver observer)
        where TObserver : ISpaceObserver
    {
        var registry = client.ServiceProvider.GetRequiredService<ISpaceObserverRegistry>();
        var observerRef = await client.CreateObjectReference<TObserver>(observer);

        registry.Deregister(observerRef);
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
        services.AddSingleton<SpaceObservationManager>();
        services.AddSingleton<IIncomingGrainCallFilter, MyFilter>();;
        services.AddSingleton<TupleFunctionSerializer>();
    }
}

