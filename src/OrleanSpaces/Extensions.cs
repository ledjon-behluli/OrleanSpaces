using Orleans;
using Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
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
        });

        return builder;
    }

    internal static ISpaceProvider GetSpaceProvider(this IGrainFactory factory)
        => factory.GetGrain<ISpaceProvider>(Guid.Empty);

    public static async Task SubscribeAsync<TObserver>(this IClusterClient client, TObserver observer) 
        where TObserver : ISpaceObserver
    {
        var registry = client.ServiceProvider.GetRequiredService<ISpaceObserversRegistry>();
        var observerRef = await client.CreateObjectReference<TObserver>(observer);

        registry.Register(observerRef);
    }

    public static async Task UnsubscribeAsync<TObserver>(this IClusterClient client, TObserver observer)
        where TObserver : ISpaceObserver
    {
        var registry = client.ServiceProvider.GetRequiredService<ISpaceObserversRegistry>();
        var observerRef = await client.CreateObjectReference<TObserver>(observer);

        registry.Deregister(observerRef);
    }
}

public static class HostingExtensions
{ 
    public static ISiloBuilder ConfigureTupleSpace(this ISiloBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(This.Assembly).WithReferences());
        builder.ConfigureServices(services => services.AddSiloComponents());

        return builder;
    }

    public static ISiloHostBuilder ConfigureTupleSpace(this ISiloHostBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(This.Assembly).WithReferences());
        builder.ConfigureServices(services => services.AddSiloComponents());

        return builder;
    }

    private static void AddSiloComponents(this IServiceCollection services)
    {
        services.AddSingleton<TupleFunctionSerializer>();

        services.AddSingleton<ISpaceFluctuationsNotifier>(sp => sp.GetRequiredService<SpaceObservationsManager>());
        services.AddSingleton<ISpaceObserversRegistry>(sp => sp.GetRequiredService<SpaceObservationsManager>());
        services.AddSingleton<IIncomingGrainCallFilter, MyFilter>();
    }
}

