using Orleans;
using Orleans.Hosting;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Observers;
using OrleanSpaces.Spaces;
using Microsoft.Extensions.DependencyInjection;

namespace OrleanSpaces;

public static class SiloExtensions
{
    public static ISiloBuilder AddTupleSpace(this ISiloBuilder builder) =>
        builder.ConfigureApplicationParts(parts =>
            parts.AddApplicationPart(typeof(SiloExtensions).Assembly).WithReferences());
 
    public static ISiloHostBuilder AddTupleSpace(this ISiloHostBuilder builder) =>
        builder.ConfigureApplicationParts(parts =>
            parts.AddApplicationPart(typeof(SiloExtensions).Assembly).WithReferences());
}

public static class ClientExtensions
{
    public static IClientBuilder UseTupleSpace(this IClientBuilder builder) =>
        builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(ClientExtensions).Assembly).WithReferences())
            .ConfigureServices(services =>
            {
                services.AddSingleton<ICallbackRegistry, CallbackManager>();
                services.AddHostedService(sp => (CallbackManager)sp.GetRequiredService<ICallbackRegistry>());

                services.AddSingleton<IObserverRegistry, ObserverManager>();
                services.AddHostedService(sp => (ObserverManager)sp.GetRequiredService<IObserverRegistry>());

                services.AddSingleton<SpaceAgent>();
                services.AddHostedService<AgentActivator>();

                services.AddSingleton<ISpaceClient, SpaceClient>();
            });
}