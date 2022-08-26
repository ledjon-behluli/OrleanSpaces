using Orleans;
using Orleans.Hosting;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Observers;
using OrleanSpaces.Spaces;
using Microsoft.Extensions.DependencyInjection;

namespace OrleanSpaces;

public static class Extensions
{
    public static ISiloBuilder UseTupleSpace(this ISiloBuilder builder) =>
        builder.ConfigureApplicationParts(parts =>
            parts.AddApplicationPart(typeof(Extensions).Assembly).WithReferences());
 
    public static ISiloHostBuilder UseTupleSpace(this ISiloHostBuilder builder) =>
        builder.ConfigureApplicationParts(parts =>
            parts.AddApplicationPart(typeof(Extensions).Assembly).WithReferences());

    // TODO: Might not need it at all!!!
    public static IClientBuilder UseTupleSpace(this IClientBuilder builder) =>
        builder.ConfigureApplicationParts(parts => 
            parts.AddApplicationPart(typeof(Extensions).Assembly).WithReferences());
   
    public static IServiceCollection AddTupleSpace(this IServiceCollection services)
    {
        services.AddSingleton<ICallbackRegistry, CallbackManager>();
        services.AddHostedService(sp => (CallbackManager)sp.GetRequiredService<ICallbackRegistry>());

        services.AddSingleton<IObserverRegistry, ObserverManager>();
        services.AddHostedService(sp => (ObserverManager)sp.GetRequiredService<IObserverRegistry>());

        services.AddSingleton<SpaceAgent>();
        services.AddHostedService<AgentActivator>();

        services.AddSingleton<ISpaceClient, SpaceClient>();

        return services;
    }
}