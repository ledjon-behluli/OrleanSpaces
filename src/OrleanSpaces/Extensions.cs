using Orleans;
using Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Clients;
using OrleanSpaces.Grains;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Observers;

namespace OrleanSpaces;

internal static class GrainFactoryExtensions
{
    public static ISpaceObserverRegistry GetObserverRegistry(this IGrainFactory factory)
        => factory.GetSpaceGrain();

    public static ITupleSpace GetSpaceGrain(this IGrainFactory factory)
        => factory.GetGrain<ITupleSpace>(Guid.Empty);
}

public static class SiloBuilderExtensions
{
    public static ISiloBuilder ConfigureTupleSpace(this ISiloBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(SiloBuilderExtensions).Assembly).WithReferences());
        return builder;
    }

    public static ISiloHostBuilder ConfigureTupleSpace(this ISiloHostBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(SiloBuilderExtensions).Assembly).WithReferences());
        return builder;
    }
}

public static class ClientBuilderExtensions
{
    public static IClientBuilder UseTupleSpace(this IClientBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(ClientBuilderExtensions).Assembly).WithReferences());
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<SpaceAgent>();
            services.AddSingleton<ICallbackRegistry>(sp => sp.GetRequiredService<SpaceAgent>());
            services.AddSingleton<ISpaceClient, SpaceClient>();
            services.AddHostedService<CallbackDispatcher>();
        });

        return builder;
    }
}

public static class ClusterClientExtensions
{
    public static async Task<ISpaceObserverRef> SubscribeAsync(this IClusterClient client, ISpaceObserver observer)
        => await client.SubscribeAsync(_ => observer);

    public static async Task<ISpaceObserverRef> SubscribeAsync(this IClusterClient client, Func<IServiceProvider, ISpaceObserver> observerFactory)
    {
        ISpaceObserver? observer = observerFactory?.Invoke(client.ServiceProvider);

        if (observer == null)
            throw new ArgumentException("Implementation of ISpaceObserver can not be null.");

        var _observer = await client.CreateObjectReference<ISpaceObserver>(observer);
        await client.GetObserverRegistry().RegisterAsync(_observer);

        return new SpaceObserverRef(_observer);
    }

    public static async Task UnsubscribeAsync(this IClusterClient client, ISpaceObserverRef @ref)
    {
        if (@ref == null)
            throw new ArgumentNullException(nameof(@ref));

        ISpaceObserverRegistry registry = client.GetObserverRegistry();

        if (await registry.IsRegisteredAsync(@ref.Observer))
        {
            await registry.DeregisterAsync(@ref.Observer);
            await client.DeleteObjectReference<ISpaceObserver>(@ref.Observer);
        }
    }
}
