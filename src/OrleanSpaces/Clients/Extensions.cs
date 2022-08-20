using Orleans;
using Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Core.Utils;
using OrleanSpaces.Core.Observers;
using OrleanSpaces.Clients.Callbacks;

namespace OrleanSpaces.Clients;

public static class Extensions
{
    public static IClientBuilder ConfigureTupleSpace(this IClientBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(Extensions).Assembly).WithReferences());
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<FuncSerializer>();
            services.AddSingleton<CallbackChannel>();
            services.AddSingleton<ICallbackBuffer, SpaceAgent>();
            services.AddSingleton<IOutgoingGrainCallFilter, Interceptor>();
            services.AddHostedService<CallbackDispatcher>();
        });

        return builder;
    }

    public static async Task AddAgentAsync<TObserver>(this IClusterClient client)
    {
        SpaceAgent agent = new(client.ServiceProvider.GetRequiredService<CallbackChannel>());
        await client.SubscribeAsync(agent);
    }

    public static async Task SubscribeAsync<TObserver>(this IClusterClient client, TObserver observer)
        where TObserver : ISpaceObserver
    {
        var registry = client.ServiceProvider.GetRequiredService<IObserverRegistry>();
        var observerRef = await client.CreateObjectReference<TObserver>(observer);

        registry.Register(observerRef);
    }

    public static async Task UnsubscribeAsync<TObserver>(this IClusterClient client, TObserver observer)
        where TObserver : ISpaceObserver
    {
        var registry = client.ServiceProvider.GetRequiredService<IObserverRegistry>();
        var observerRef = await client.CreateObjectReference<TObserver>(observer);

        registry.Deregister(observerRef);
    }
}