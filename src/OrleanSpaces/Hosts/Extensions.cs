using Orleans;
using Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Internals.Agents;
using OrleanSpaces.Core.Utils;

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
        services.AddSingleton<FunctionSerializer>();

        services.AddSingleton<ISpaceAgentNotifier, SpaceAgentManager>();
        services.AddSingleton<ISpaceAgentRegistry, SpaceAgentManager>();
        services.AddSingleton<IIncomingGrainCallFilter, MyFilter>();
    }
}

