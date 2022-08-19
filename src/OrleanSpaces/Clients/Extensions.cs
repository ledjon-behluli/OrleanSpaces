using Orleans;
using Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Internals.Agents;
using OrleanSpaces.Core;
using OrleanSpaces.Core.Utils;

namespace OrleanSpaces.Clients;

public static class Extensions
{
    public static ISpaceGrain GetSpaceProvider(this IGrainFactory factory)
        => factory.GetGrain<ISpaceGrain>(Guid.Empty);

    public static IClientBuilder ConfigureTupleSpace(this IClientBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(Extensions).Assembly).WithReferences());
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<FunctionSerializer>();
            services.AddSingleton<IOutgoingGrainCallFilter, FunctionEvaluator>();
        });

        return builder;
    }

    //public static async Task SubscribeAsync<TObserver>(this IClusterClient client, TObserver observer)
    //    where TObserver : ISpaceObserver
    //{
    //    var registry = client.ServiceProvider.GetRequiredService<ISpaceAgentRegistry>();
    //    var observerRef = await client.CreateObjectReference<TObserver>(observer);

    //    registry.Register(observerRef);
    //}

    //public static async Task UnsubscribeAsync<TObserver>(this IClusterClient client, TObserver observer)
    //    where TObserver : ISpaceObserver
    //{
    //    var registry = client.ServiceProvider.GetRequiredService<ISpaceAgentRegistry>();
    //    var observerRef = await client.CreateObjectReference<TObserver>(observer);

    //    registry.Deregister(observerRef);
    //}
}