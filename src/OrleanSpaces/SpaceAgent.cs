using Orleans;
using Orleans.Streams;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;
using OrleanSpaces.Continuations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace OrleanSpaces;

public interface ISpaceAgent
{
    /// <summary>
    /// Enables the <paramref name="observer"/> to subscribe to events that happen in the tuple space.
    /// </summary>
    /// <param name="observer">Any space observer.</param>
    /// <remarks><i>Method is idempotant.</i></remarks>
    /// <returns>An ID that can be used to <see cref="Unsubscribe"/>.</returns>
    Guid Subscribe(ISpaceObserver observer);

    /// <summary>
    /// Removes the observer with the corresponding <paramref name="observerId"/>.
    /// </summary>
    /// <param name="observerId">The ID obtained from calling <see cref="Subscribe"/>.</param>
    /// <remarks><i>Method is idempotant.</i></remarks>
    void Unsubscribe(Guid observerId);

    /// <summary>
    /// Directly writes the <paramref name="tuple"/> in the space.
    /// </summary>
    /// <param name="tuple">Any non-<see cref="SpaceTuple.Passive"/>.</param>
    /// <remarks><i>Analogous to the "OUT" operation in the tuple space paradigm.</i></remarks>
    Task WriteAsync(SpaceTuple tuple);

    /// <summary>
    /// Indirectly writes a <see cref="SpaceTuple"/> in the space.
    /// <list type="number">
    /// <item><description>Executes the <paramref name="evaluation"/> function.</description></item>
    /// <item><description>Proceeds to write the resulting <see cref="SpaceTuple"/> in the space.</description></item>
    /// </list>
    /// </summary>
    /// <param name="evaluation">Any function that returns a non-<see cref="SpaceTuple.Passive"/>.</param>
    /// <remarks><i>Analogous to the "EVAL" operation in the tuple space paradigm.</i></remarks>
    ValueTask EvaluateAsync(Func<Task<SpaceTuple>> evaluation);

    /// <summary>
    /// Reads a <see cref="SpaceTuple"/> that is potentially matched by the given <paramref name="template"/>.
    /// <list type="bullet">
    /// <item><description>If one such tuple exists, then a <u>copy</u> is returned thereby keeping the original in the space.</description></item>
    /// <item><description>Otherwise a <see cref="SpaceTuple.Passive"/> is returned to indicate a "no-match".</description></item>
    /// </list>
    /// </summary>
    /// <param name="template">A template that potentially matches a <see cref="SpaceTuple"/>.</param>
    /// <remarks><i>Analogous to the "RDP" operation in the tuple space paradigm.</i></remarks>
    /// <returns><see cref="SpaceTuple"/> or <see cref="SpaceTuple.Passive"/>.</returns>
    ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template);

    /// <summary>
    /// Reads a <see cref="SpaceTuple"/> that is potentially matched by the given <paramref name="template"/>.
    /// <list type="bullet">
    /// <item><description>If one such tuple exists, the <paramref name="callback"/> is invoked immediately.</description></item>
    /// <item><description>Otherwise the operation is remembered and the <paramref name="callback"/> will eventually be invoked as soon as a matching <see cref="SpaceTuple"/> is written in the space.</description></item>
    /// </list>
    /// </summary>
    /// <param name="template">A template that potentially matches a <see cref="SpaceTuple"/>.</param>
    /// <param name="callback">A callback function that will be executed, with the <see cref="SpaceTuple"/> as the argument.</param>
    /// <remarks>
    /// <para><i>Same as with <see cref="PeekAsync"/>, the original tuple is <u>kept</u> in the space once <paramref name="callback"/> gets invoked.</i></para>
    /// <para><i>Analogous to the "RD" operation in the tuple space paradigm.</i></para>
    /// </remarks>
    ValueTask PeekAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback);

    /// <summary>
    /// Reads a <see cref="SpaceTuple"/> that is potentially matched by the given <paramref name="template"/>.
    /// <list type="bullet">
    /// <item><description>If one such tuple exists, then the <u>original</u> is returned thereby removing it from the space.</description></item>
    /// <item><description>Otherwise a <see cref="SpaceTuple.Passive"/> is returned to indicate a "no-match".</description></item>
    /// </list>
    /// </summary>
    /// <param name="template">A template that potentially matches a <see cref="SpaceTuple"/>.</param>
    /// <remarks><i>Analogous to the "INP" operation in the tuple space paradigm.</i></remarks>
    /// <returns><see cref="SpaceTuple"/> or <see cref="SpaceTuple.Passive"/>.</returns>
    Task<SpaceTuple> PopAsync(SpaceTemplate template);

    /// <summary>
    /// Reads a <see cref="SpaceTuple"/> that is potentially matched by the given <paramref name="template"/>.
    /// <list type="bullet">
    /// <item><description>If one such tuple exists, the <paramref name="callback"/> is invoked immediately.</description></item>
    /// <item><description>Otherwise the operation is remembered and the <paramref name="callback"/> will eventually be invoked as soon as a matching <see cref="SpaceTuple"/> is written in the space.</description></item>
    /// </list>
    /// </summary>
    /// <param name="template">A template that potentially matches a <see cref="SpaceTuple"/>.</param>
    /// <param name="callback">A callback function that will be executed, with the <see cref="SpaceTuple"/> as the argument.</param>
    /// <remarks>
    /// <para><i>Same as with <see cref="PopAsync"/>, the original tuple is <u>removed</u> from the space once <paramref name="callback"/> gets invoked.</i></para>
    /// <para><i>Analogous to the "IN" operation in the tuple space paradigm.</i></para>
    /// </remarks>
    Task PopAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback);

    /// <summary>
    /// Reads multiple <see cref="SpaceTuple"/>'s that are potentially matched by the given <paramref name="template"/>.
    /// </summary>
    /// <param name="template">A template that potentially matches multiple <see cref="SpaceTuple"/>'s.</param>
    /// <remarks><i>Same as with <see cref="PeekAsync"/>, the original tuple's are <u>kept</u> in the space.</i></remarks>
    ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template);

    /// <summary>
    /// Returns the total number of <see cref="SpaceTuple"/>'s in the space. 
    /// </summary>
    ValueTask<int> CountAsync();

    /// <summary>
    /// Returns the total number of <see cref="SpaceTuple"/>'s which are potentially matched by the given <paramref name="template"/>.
    /// </summary>
    /// <param name="template">A template that potentially matches multiple <see cref="SpaceTuple"/>'s.</param>
    ValueTask<int> CountAsync(SpaceTemplate template);
}

internal sealed class SpaceAgent : ISpaceAgent, ITupleRouter, IAsyncObserver<ITuple>
{
    private readonly IClusterClient client;

    private readonly EvaluationChannel evaluationChannel;
    private readonly CallbackChannel callbackChannel;
    private readonly ObserverChannel observerChannel;

    private readonly CallbackRegistry callbackRegistry;
    private readonly ObserverRegistry observerRegistry;

    [AllowNull] private ISpaceGrain grain;

    public SpaceAgent(
        IClusterClient client,
        EvaluationChannel evaluationChannel,
        CallbackChannel callbackChannel,
        ObserverChannel observerChannel,
        CallbackRegistry callbackRegistry,
        ObserverRegistry observerRegistry)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));

        this.evaluationChannel = evaluationChannel ?? throw new ArgumentNullException(nameof(evaluationChannel));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
        this.observerChannel = observerChannel ?? throw new ArgumentNullException(nameof(observerChannel));

        this.callbackRegistry = callbackRegistry ?? throw new ArgumentNullException(nameof(callbackRegistry));
        this.observerRegistry = observerRegistry ?? throw new ArgumentNullException(nameof(observerRegistry));
    }

    public async Task InitializeAsync()
    {
        if (!client.IsInitialized)
        {
            await client.Connect();
        }

        grain = client.GetGrain<ISpaceGrain>(Constants.SpaceGrainId);

        var streamId = await grain.ListenAsync();
        var provider = client.GetStreamProvider(Constants.PubSubProvider);
        var stream = provider.GetStream<ITuple>(streamId, Constants.TupleStream);

        await stream.SubscribeAsync(this);
    }

    #region IAsyncObserver

    public async Task OnNextAsync(ITuple tuple, StreamSequenceToken token)
    {   
        await observerChannel.Writer.WriteAsync(tuple);
        if (tuple is SpaceTuple spaceTuple)
        {
            await callbackChannel.Writer.WriteAsync(spaceTuple);
        }
    }

    public Task OnCompletedAsync() => Task.CompletedTask;
    public Task OnErrorAsync(Exception e) => Task.CompletedTask;

    #endregion

    #region ITupleRouter

    public async Task RouteAsync(ITuple tuple)
    {
        if (tuple == null)
        {
            throw new ArgumentNullException(nameof(tuple));
        }

        if (tuple is SpaceTuple spaceTuple)
        {
            await WriteAsync(spaceTuple);
            return;
        }
        
        if (tuple is SpaceTemplate spaceTemplate)
        {
            _ = await PopAsync(spaceTemplate);
            return;
        }
    }

    #endregion

    #region ISpaceAgent

    public Guid Subscribe(ISpaceObserver observer)
        => observerRegistry.Add(observer);

    public void Unsubscribe(Guid observerId)
        => observerRegistry.Remove(observerId);

    public async Task WriteAsync(SpaceTuple tuple)
        => await grain.WriteAsync(tuple);

    public async ValueTask EvaluateAsync(Func<Task<SpaceTuple>> evaluation)
    {
        if (evaluation == null)
        {
            throw new ArgumentNullException(nameof(evaluation));
        }

        await evaluationChannel.Writer.WriteAsync(evaluation);
    }

    public async ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template)
        => await grain.PeekAsync(template);

    public async ValueTask PeekAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (callback == null)
        {
            throw new ArgumentNullException(nameof(callback));
        }

        SpaceTuple tuple = await grain.PeekAsync(template);

        if (!tuple.IsPassive)
        {
            await callback(tuple);
        }
        else
        {
            callbackRegistry.Add(template, new(callback, false));
        }
    }

    public async Task<SpaceTuple> PopAsync(SpaceTemplate template)
            => await grain.PopAsync(template);

    public async Task PopAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (callback == null)
        {
            throw new ArgumentNullException(nameof(callback));
        }

        SpaceTuple tuple = await grain.PopAsync(template);

        if (!tuple.IsPassive)
        {
            await callback(tuple);
        }
        else
        {
            callbackRegistry.Add(template, new(callback, true));
        }
    }

    public async ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template)
            => await grain.ScanAsync(template);

    public async ValueTask<int> CountAsync()
            => await grain.CountAsync(null);

    public async ValueTask<int> CountAsync(SpaceTemplate template)
            => await grain.CountAsync(template);

    #endregion
}