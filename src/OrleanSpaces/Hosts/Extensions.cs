using Orleans;
using Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Hosts.Internals;
using OrleanSpaces.Core.Internals;

namespace OrleanSpaces.Hosts;

public static class Extensions
{
    public static ISiloBuilder ConfigureTupleSpace(this ISiloBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(Extensions).Assembly).WithReferences());
        builder.ConfigureServices(services => services.AddComponents());

        return builder;
    }

    public static ISiloHostBuilder ConfigureTupleSpace(this ISiloHostBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(Extensions).Assembly).WithReferences());
        builder.ConfigureServices(services => services.AddComponents());

        return builder;
    }

    private static void AddComponents(this IServiceCollection services)
    {
        services.AddSingleton<LambdaSerializer>();
        services.AddSingleton<IObserverNotifier, ObserverManager>();
        services.AddSingleton<IObserverRegistry, ObserverManager>();
        services.AddSingleton<IIncomingGrainCallFilter, Interceptor>();
    }
}

