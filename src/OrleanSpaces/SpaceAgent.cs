using Orleans.Streams;
using Orleans;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;
using System.Diagnostics.CodeAnalysis;
using OrleanSpaces.Continuations;
using System.Runtime.CompilerServices;

namespace OrleanSpaces;

public interface ISpaceAgent
{
    ObserverRef Subscribe(ISpaceObserver observer);
    void Unsubscribe(ObserverRef @ref);

    /// <summary>
    /// <para>Used to write a tuple _in the tuple space.</para>
    /// <para><i>Analogous to the "OUT" primitive _in the tuple space model.</i></para>
    /// </summary>
    Task WriteAsync(SpaceTuple tuple);

    /// <summary>
    /// Evaluate
    /// </summary>
    /// <param name="evaluation"></param>
    /// <returns></returns>
    ValueTask EvaluateAsync(Func<Task<SpaceTuple>> evaluation);

    /// <summary>
    /// <para>Used to read a tuple from the tuple space.</para>
    /// <para><i>Analogous to the "RD" primitive _in the tuple space model.</i></para>
    /// </summary>
    ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template);

    /// <summary>
    /// <para>Used to read a tuple from the tuple space.</para>
    /// <para><i>Analogous to the "RDP" primitive _in the tuple space model.</i></para>
    /// </summary>
    ValueTask PeekAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback);

    /// <summary>
    /// <para>Used to read and remove a tuple from the tuple space.</para>
    /// <para><i>Analogous to the "IN" primitive _in the tuple space model.</i></para>
    /// </summary>
    Task<SpaceTuple> PopAsync(SpaceTemplate template);

    /// <summary>
    /// <para>Used to read and remove a tuple from the tuple space.</para>
    /// <para><i>Analogous to the "INP" primitive _in the tuple space model.</i></para>
    /// </summary>
    Task PopAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback);

    ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template);

    ValueTask<int> CountAsync();
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

        grain = client.GetGrain<ISpaceGrain>(Guid.Empty);

        var streamId = await grain.ListenAsync();
        var provider = client.GetStreamProvider(StreamNames.PubSubProvider);
        var stream = provider.GetStream<ITuple>(streamId, StreamNamespaces.Tuple);

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

    public ObserverRef Subscribe(ISpaceObserver observer)
        => new(observerRegistry.Add(observer), observer);

    public void Unsubscribe(ObserverRef @ref)
        => observerRegistry.Remove(@ref.Observer);

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