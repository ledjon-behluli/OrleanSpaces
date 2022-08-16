using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Hosting;
using OrleanSpaces.Filters;
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
            services.AddSingleton<IOutgoingGrainCallFilter, TupleFunctionEvaluator>();
            services.AddTupleSpace();
        });

        return builder;
    }

    public static ISiloBuilder ConfigureTupleSpace(this ISiloBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(TupleSpaceGrain).Assembly).WithReferences());
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<ObserverManager>();
            services.AddSingleton<SubscriberRegistry>();
            services.AddSingleton<IIncomingGrainCallFilter, SpaceOscillationNotifier>();
            services.AddTupleSpace();
        });

        return builder;
    }

    private static void AddTupleSpace(this IServiceCollection services) =>
        services.AddSingleton(sp => sp.GetRequiredService<IGrainFactory>().GetGrain(typeof(TupleSpaceGrain), Guid.Empty));
}

