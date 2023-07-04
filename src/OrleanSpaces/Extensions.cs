using OrleanSpaces.Observers;
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
    /// Configures the tuple space on the <see cref="IClientBuilder"/>.
    /// </summary>
    /// <param name="builder">The orleans client builder.</param>
    /// <param name="action">An optional delegate to configure the <see cref="SpaceOptions"/></param>
    public static IClientBuilder AddOrleanSpaces(this IClientBuilder builder, Action<SpaceOptions>? action = null)
    {
        SpaceOptions options = new();
        action?.Invoke(options);

        if (options.SpaceTuplesEnabled)
        {
            builder.Services.AddSingleton<ObserverRegistry<SpaceTuple>>();
            builder.Services.AddSingleton<CallbackRegistry>();

            builder.Services.AddSingleton<EvaluationChannel<SpaceTuple>>();
            builder.Services.AddSingleton<ObserverChannel<SpaceTuple>>();
            builder.Services.AddSingleton<CallbackChannel<SpaceTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<SpaceTuple, SpaceTemplate>>();

            builder.Services.AddSingleton<SpaceAgent>();
            builder.Services.AddSingleton<ITupleRouter<SpaceTuple, SpaceTemplate>>(sp => sp.GetRequiredService<SpaceAgent>());
            builder.Services.AddSingleton<ISpaceAgentProvider, SpaceAgentProvider>();

            builder.Services.AddHostedService<CallbackProcessor>();
            builder.Services.AddHostedService<ObserverProcessor<SpaceTuple>>();
            builder.Services.AddHostedService<EvaluationProcessor<SpaceTuple, SpaceTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<SpaceTuple, SpaceTemplate>>();
        }

        if (options.IntTuplesEnabled)
        {
            builder.Services.AddSingleton<ObserverRegistry<IntTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<int, IntTuple, IntTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<IntTuple>>();
            builder.Services.AddSingleton<ObserverChannel<IntTuple>>();
            builder.Services.AddSingleton<CallbackChannel<IntTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<IntTuple, IntTemplate>>();

            builder.Services.AddSingleton<IntAgent>();
            builder.Services.AddSingleton<ITupleRouter<IntTuple, IntTemplate>>(sp => sp.GetRequiredService<IntAgent>());
            builder.Services.AddSingleton<ISpaceAgentProvider<int, IntTuple, IntTemplate>, IntAgentProvider>();

            builder.Services.AddHostedService<ObserverProcessor<IntTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<int, IntTuple, IntTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<IntTuple, IntTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<IntTuple, IntTemplate>>();
        }

        return builder;
    }
}

public sealed class SpaceOptions
{
    public bool SpaceTuplesEnabled { get; set; } = true;
    public bool IntTuplesEnabled { get; set; } = false;
}