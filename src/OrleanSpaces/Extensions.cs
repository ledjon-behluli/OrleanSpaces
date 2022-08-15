using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Hosting;
using OrleanSpaces.Observables;
using System;

namespace OrleanSpaces;

public static class ClientExtensions
{
    public static IClientBuilder ConfigureTupleSpace(this IClientBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(TupleSpaceGrain).Assembly).WithReferences());
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(sp => sp.GetRequiredService<IGrainFactory>().GetTupleSpace());
        });

        return builder;
    }

    public static ISiloBuilder ConfigureTupleSpace(this ISiloBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(TupleSpaceGrain).Assembly).WithReferences());
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<SpaceObserverManager>();
            services.AddSingleton<SpaceSubscribersRegistry>();
            services.AddSingleton<IIncomingGrainCallFilter, SpaceNotificationTrigger>();
            services.AddSingleton(sp => sp.GetRequiredService<IGrainFactory>().GetTupleSpace());
        });

        return builder;
    }

    private static ITupleSpace GetTupleSpace(this IGrainFactory factory) => (ITupleSpace)factory.GetGrain(typeof(TupleSpaceGrain), Guid.Empty);
}

