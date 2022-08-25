using Orleans;
using Orleans.Hosting;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Observers;
using OrleanSpaces.Spaces;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace OrleanSpaces;

internal static class This
{
    public static Assembly Assembly => typeof(This).Assembly;
}

internal static class GrainFactoryExtensions
{
    public static ISpaceGrain GetSpaceGrain(this IGrainFactory factory)
        => factory.GetGrain<SpaceGrain>(Guid.Empty);
}

public static class SiloBuilderExtensions
{
    public static ISiloBuilder ConfigureTupleSpace(this ISiloBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(This.Assembly).WithReferences());
        return builder;
    }

    public static ISiloHostBuilder ConfigureTupleSpace(this ISiloHostBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(This.Assembly).WithReferences());
        return builder;
    }
}

public static class ClientBuilderExtensions
{
    public static IClientBuilder UseTupleSpace(this IClientBuilder builder)
    {
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(This.Assembly).WithReferences());
        builder.ConfigureServices(services =>
        {
            services.AddHostedService<CallbackManager>();
            services.AddSingleton<ICallbackRegistry>(sp => sp.GetRequiredService<CallbackManager>());

            services.AddHostedService<ObserverManager>();
            services.AddSingleton<IObserverRegistry>(sp => sp.GetRequiredService<ObserverManager>());

            services.AddSingleton<ISpaceClient, SpaceClient>();

            services.AddSingleton<SpaceAgent>();
            services.AddHostedService<AgentActivator>();
        });

        return builder;
    }
}