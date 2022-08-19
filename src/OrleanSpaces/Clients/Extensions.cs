using Orleans;
using Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Core.Utils;
using OrleanSpaces.Core.Observers;
using OrleanSpaces.Clients.Filters;

namespace OrleanSpaces.Clients;

public static class Extensions
{
    public static IClientBuilder ConfigureTupleSpace(this IClientBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(Extensions).Assembly).WithReferences());
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<FuncSerializer>();
            services.AddSingleton<IOutgoingGrainCallFilter, SpaceWriterFilter>();
            services.AddSingleton<IOutgoingGrainCallFilter, SpaceBlockingReaderFilter>();
        });

        return builder;
    }

    public static async Task AddAgentAsync<TObserver>(this IClusterClient client)
    {
        SpaceBlockingReaderFilter agent = new(client.ServiceProvider.GetRequiredService<IGrainFactory>());
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