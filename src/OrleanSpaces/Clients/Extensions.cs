using Orleans;
using Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Core.Observers;
using OrleanSpaces.Clients.Callbacks;
using OrleanSpaces.Clients.Bridges;
using OrleanSpaces.Core;
using OrleanSpaces.Clients.Observers;

namespace OrleanSpaces.Clients;

public static class Extensions
{
    public static IClientBuilder UseTupleSpace(this IClientBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(Extensions).Assembly).WithReferences());
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<SpaceAgent>();
            services.AddSingleton<ICallbackRegistry>(sp => sp.GetRequiredService<SpaceAgent>());
            services.AddSingleton<ISpaceClient, SpaceClient>();
            services.AddHostedService<CallbackDispatcher>();
        });

        return builder;
    }

    public static async Task<ISpaceObserverRef> SubscribeAsync(this IClusterClient client, ISpaceObserver observer)
        => await client.SubscribeAsync(_ => observer);

    public static async Task<ISpaceObserverRef> SubscribeAsync(this IClusterClient client, Func<IServiceProvider, ISpaceObserver> observerFactory)
    {
        if (observerFactory == null)
            throw new ArgumentNullException(nameof(observerFactory));

        var @ref = await client.CreateObjectReference<ISpaceObserver>(observerFactory.Invoke(client.ServiceProvider));
        client.GetObserverRegistry().Register(@ref);

        return new SpaceObserverRef(@ref);
    }

    public static async Task UnsubscribeAsync(this IClusterClient client, ISpaceObserverRef @ref)
    { 
        if (@ref == null)
            throw new ArgumentNullException(nameof(@ref));

        client.GetObserverRegistry().Deregister(@ref.Observer);
        await client.DeleteObjectReference<ISpaceObserver>(@ref.Observer);
    }
}