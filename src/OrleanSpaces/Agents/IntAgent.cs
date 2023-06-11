using Orleans;
using Orleans.Streams;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Observers;
using System.Diagnostics.CodeAnalysis;
using OrleanSpaces.Tuples.Typed;
using OrleanSpaces.Continuations;
using OrleanSpaces.Tuples;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Agents;

internal sealed partial class IntAgent :
    ISpaceAgent<int, IntTuple, IntTemplate>,
    ITupleRouter<int, IntTuple, IntTemplate>,
    IAsyncObserver<IntTuple>,
    IAsyncObserver<IntTemplate>
{
    private readonly List<IntTuple> tuples = new();

    private readonly IClusterClient client;
    private readonly EvaluationChannel<int, IntTuple, IntTemplate> evaluationChannel;
    private readonly CallbackChannel<int, IntTuple, IntTemplate> callbackChannel;
    private readonly ObserverChannel<int, IntTuple, IntTemplate> observerChannel;
    private readonly CallbackRegistry callbackRegistry;
    private readonly ObserverRegistry observerRegistry;

    [AllowNull] private IIntGrain grain;

    public IntAgent(
        IClusterClient client,
        EvaluationChannel<int, IntTuple, IntTemplate> evaluationChannel,
        CallbackChannel<int, IntTuple, IntTemplate> callbackChannel,
        ObserverChannel<int, IntTuple, IntTemplate> observerChannel,
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

    #region IAsyncObserver

    async Task IAsyncObserver<IntTuple>.OnNextAsync(IntTuple tuple, StreamSequenceToken token)
    {
        await observerChannel.TupleWriter.WriteAsync(tuple);
        await callbackChannel.TupleWriter.WriteAsync(tuple);
    }

    async Task IAsyncObserver<IntTemplate>.OnNextAsync(IntTemplate tuple, StreamSequenceToken token)
    {
        await observerChannel.TemplateWriter.WriteAsync(tuple);
    }

    Task IAsyncObserver<IntTuple>.OnCompletedAsync() => Task.CompletedTask;
    Task IAsyncObserver<IntTemplate>.OnCompletedAsync() => Task.CompletedTask;

    Task IAsyncObserver<IntTuple>.OnErrorAsync(Exception e) => Task.CompletedTask;
    Task IAsyncObserver<IntTemplate>.OnErrorAsync(Exception e) => Task.CompletedTask;

    #endregion

    #region ITupleRouter

    Task ITupleRouter<int, IntTuple, IntTemplate>.RouteAsync(IntTuple tuple)
        => WriteAsync(tuple);

    async ValueTask ITupleRouter<int, IntTuple, IntTemplate>.RouteAsync(IntTemplate template)
        => await PopAsync(template);

    #endregion

    #region ISpaceAgent

    public Task WriteAsync(IntTuple tuple) => grain.WriteAsync(tuple);

    public ValueTask EvaluateAsync(Func<Task<IntTuple>> evaluation)
    {
        evaluationChannel.ThrowIfNotBeingConsumed();

        if (evaluation == null)
        {
            throw new ArgumentNullException(nameof(evaluation));
        }

        return evaluationChannel.TupleWriter.WriteAsync(evaluation);
    }

    public ValueTask<IntTuple> PeekAsync(IntTemplate template)
    {
        IntTuple tuple = tuples.FindTuple<int, IntTuple, IntTemplate>(template);
        return ValueTask.FromResult(tuple);
    }

    public async ValueTask PeekAsync(IntTemplate template, Func<IntTuple, Task> callback)
    {
        callbackChannel.ThrowIfNotBeingConsumed();

        if (callback == null)
        {
            throw new ArgumentNullException(nameof(callback));
        }

        IntTuple tuple = tuples.FindTuple<int, IntTuple, IntTemplate>(template);

        if (tuple != IntTuple.Empty)
        {
            await callback(tuple);
        }
        else
        {
            callbackRegistry.Add(template, new(callback, false));
        }
    }

    public async ValueTask<IntTuple> PopAsync(IntTemplate template)
    {
        IntTuple tuple = tuples.FindTuple<int, IntTuple, IntTemplate>(template);

        if (tuple != IntTuple.Empty)
        {
            await grain.RemoveAsync(tuple);
            tuples.Remove(tuple);
        }

        return tuple;
    }

    public async ValueTask PopAsync(IntTemplate template, Func<IntTuple, Task> callback)
    {
        callbackChannel.ThrowIfNotBeingConsumed();

        if (callback == null)
        {
            throw new ArgumentNullException(nameof(callback));
        }

        IntTuple tuple = tuples.FindTuple<int, IntTuple, IntTemplate>(template);

        if (tuple != IntTuple.Empty)
        {
            await callback(tuple);
            await grain.RemoveAsync(tuple);
            tuples.Remove(tuple);
        }
        else
        {
            callbackRegistry.Add(template, new(callback, false));
        }
    }

    public ValueTask<IEnumerable<IntTuple>> ScanAsync(IntTemplate template)
    {
        throw new NotImplementedException();
    }

    public ValueTask<int> CountAsync(IntTemplate template) => new(tuples.Count);

    #endregion
}
