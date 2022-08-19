using Orleans;
using Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Core.Utils;
using OrleanSpaces.Hosts.Filters;
using OrleanSpaces.Hosts.Observers;
using OrleanSpaces.Core.Observers;

namespace OrleanSpaces.Hosts;

public static class Extensions
{
    public static ISiloBuilder ConfigureTupleSpace(this ISiloBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(Extensions).Assembly).WithReferences());
        builder.ConfigureServices(services => services.AddSiloComponents());

        return builder;
    }

    public static ISiloHostBuilder ConfigureTupleSpace(this ISiloHostBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(Extensions).Assembly).WithReferences());
        builder.ConfigureServices(services => services.AddSiloComponents());

        return builder;
    }

    private static void AddSiloComponents(this IServiceCollection services)
    {
        services.AddSingleton<FuncSerializer>();

        services.AddSingleton<IObserverNotifier, ObserverManager>();
        services.AddSingleton<IObserverRegistry, ObserverManager>();
        services.AddSingleton<IIncomingGrainCallFilter, SpaceWriterFilter>();
    }
}

