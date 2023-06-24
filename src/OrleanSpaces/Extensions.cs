﻿using OrleanSpaces.Observers;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Continuations;
using OrleanSpaces.Agents;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using Microsoft.Extensions.DependencyInjection;

namespace OrleanSpaces;

public static class Extensions
{
    /// <summary>
    /// Configures the tuple space on the client.
    /// </summary>
    public static IClientBuilder AddTupleSpace(this IClientBuilder builder, Action<SpaceOptions>? optionsAction = null)
    {
        SpaceOptions options = new();
        optionsAction?.Invoke(options);

        return builder.ConfigureServices(services => services.AddClientServices(options));
    }

    /// <summary>
    /// Configures the tuple space on the service collection.
    /// </summary>
    /// <param name="services"/>
    /// <param name="clusterClientFactory">An optional delegate that returns an <see cref="IClusterClient"/> to be used.<br/>
    /// <i>If omitted, then localhost clustering and simple message stream provider are used instead.</i></param>
    public static IServiceCollection AddTupleSpace(this IServiceCollection services, Action<SpaceOptions>? optionsAction = null)
    {
        SpaceOptions options = new();
        optionsAction?.Invoke(options);

        return services.AddClientServices(options);
    }

    private static IServiceCollection AddClientServices(this IServiceCollection services, SpaceOptions options)
    {
        if (options.SpaceTuplesEnabled)
        {
            services.AddSingleton<ObserverRegistry<SpaceTuple>>();
            services.AddSingleton<CallbackRegistry>();

            services.AddSingleton<EvaluationChannel<SpaceTuple>>();
            services.AddSingleton<ObserverChannel<SpaceTuple>>();
            services.AddSingleton<CallbackChannel<SpaceTuple, SpaceTemplate>>();
            services.AddSingleton<ContinuationChannel<SpaceTuple, SpaceTemplate>>();

            services.AddSingleton<SpaceAgent>();
            services.AddSingleton<ITupleRouter<SpaceTuple, SpaceTemplate>>(sp => sp.GetRequiredService<SpaceAgent>());
            services.AddSingleton<ISpaceAgentProvider, SpaceAgentProvider>();

            services.AddHostedService<CallbackProcessor>();
            services.AddHostedService<ObserverProcessor<SpaceTuple>>();
            services.AddHostedService<EvaluationProcessor<SpaceTuple, SpaceTemplate>>();
            services.AddHostedService<ContinuationProcessor<SpaceTuple, SpaceTemplate>>();
        }

        if (options.IntTuplesEnabled)
        {
            services.AddSingleton<ObserverRegistry<IntTuple>>();
            services.AddSingleton<CallbackRegistry<int, IntTuple, IntTemplate>>();

            services.AddSingleton<EvaluationChannel<IntTuple>>();
            services.AddSingleton<ObserverChannel<IntTuple>>();
            services.AddSingleton<CallbackChannel<IntTuple, IntTemplate>>();
            services.AddSingleton<ContinuationChannel<IntTuple, IntTemplate>>();

            services.AddSingleton<IntAgent>();
            services.AddSingleton<ITupleRouter<IntTuple, IntTemplate>>(sp => sp.GetRequiredService<IntAgent>());
            services.AddSingleton<ISpaceAgentProvider<int, IntTuple, IntTemplate>, IntAgentProvider>();

            services.AddHostedService<ObserverProcessor<IntTuple>>();
            services.AddHostedService<CallbackProcessor<int, IntTuple, IntTemplate>>();
            services.AddHostedService<EvaluationProcessor<IntTuple, IntTemplate>>();
            services.AddHostedService<ContinuationProcessor<IntTuple, IntTemplate>>();
        }

        return services;
    }
}

public sealed class SpaceOptions
{
    public bool SpaceTuplesEnabled { get; set; } = true;
    public bool IntTuplesEnabled { get; set; } = false;
}