using Orleans;
using Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Core.Observers;
using OrleanSpaces.Clients.Callbacks;
using OrleanSpaces.Clients.Bridges;
using OrleanSpaces.Core;

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

    public static async Task SubscribeAsync(this IClusterClient client, ISpaceObserver observer)
        => await client.SubscribeAsync(_ => observer);

    public static async Task SubscribeAsync(this IClusterClient client, Func<IServiceProvider, ISpaceObserver> observerFactory)
    {
        if (observerFactory == null)
            throw new ArgumentNullException(nameof(observerFactory));

        var observerRef = await client.CreateObjectReference<ISpaceObserver>(observerFactory.Invoke(client.ServiceProvider));
        client.GetObserverRegistry().Register(observerRef);
    }

    public static async Task UnsubscribeAsync(this IClusterClient client, ISpaceObserver observer)
        => await client.UnsubscribeAsync(_ => observer);

    public static async Task UnsubscribeAsync(this IClusterClient client, Func<IServiceProvider, ISpaceObserver> observerFactory)
    {
        if (observerFactory == null)
            throw new ArgumentNullException(nameof(observerFactory));

        var observerRef = await client.CreateObjectReference<ISpaceObserver>(observerFactory.Invoke(client.ServiceProvider));
        client.GetObserverRegistry().Deregister(observerRef);
    }
}