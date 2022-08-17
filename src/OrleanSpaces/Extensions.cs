using Orleans;
using Orleans.Hosting;
using Serialize.Linq.Serializers;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Internals;
using System.Reflection;

namespace OrleanSpaces;

public static class Extensions
{
    private static readonly Assembly ThisAssembly = typeof(Extensions).Assembly;

    public static IClientBuilder ConfigureTupleSpace(this IClientBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(ThisAssembly).WithReferences());
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<ExpressionSerializer>();
            services.AddSingleton<TupleFunctionSerializer>();

            services.AddSingleton<IOutgoingGrainCallFilter, TupleFunctionExecuter>();

            services.AddSingleton(sp => sp.GetRequiredService<IGrainFactory>().GetGrain<ISpaceGrain>(Guid.Empty));

            services.AddSingleton(sp => (ITupleFunctionExecutor)sp.GetRequiredService<ISpaceGrain>());
            services.AddSingleton(sp => (ISpaceProvider)sp.GetRequiredService<ISpaceGrain>());
            services.AddSingleton(sp => (ISyncSpaceProvider)sp.GetRequiredService<ISpaceGrain>());
        });

        return builder;
    }

    public static ISiloBuilder ConfigureTupleSpace(this ISiloBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(ThisAssembly).WithReferences());
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<ObserverManager>();
            services.AddSingleton<SubscriberRegistry>();

            services.AddSingleton<IIncomingGrainCallFilter, VolumeOscillationNotifier>();

            services.AddSingleton<ExpressionSerializer>();
            services.AddSingleton<TupleFunctionSerializer>();

            services.AddSingleton(sp => (SpaceGrain)sp.GetRequiredService<IGrainFactory>().GetGrain(typeof(SpaceGrain), Guid.Empty));
        });

        return builder;
    }
}

