using Microsoft.Extensions.Hosting;
using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Continuations;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Helpers;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;
using System.Threading.Channels;

namespace OrleanSpaces.Agents;

internal class Agent<T, TTuple, TTemplate> :
    ISpaceAgent<T, TTuple, TTemplate>,
    ITupleActionReceiver<TTuple>, 
    ITupleRouter<TTuple, TTemplate>
    where T : unmanaged
    where TTuple : struct, ISpaceTuple<T>
    where TTemplate : struct, ISpaceTemplate<T>, ISpaceMatchable<T, TTuple>
{
    private readonly static object lockObj = new();
    private readonly Guid agentId = Guid.NewGuid();

    private readonly ITupleStore<TTuple> tupleStore;
    private readonly EvaluationChannel<TTuple> evaluationChannel;
    private readonly ObserverRegistry<TTuple> observerRegistry;
    private readonly CallbackRegistry<T, TTuple, TTemplate> callbackRegistry;

    private Channel<TTuple>? streamChannel;
    private ImmutableArray<TTuple> tuples = ImmutableArray<TTuple>.Empty;

    public Agent(
        ITupleStore<TTuple> tupleStore,
        EvaluationChannel<TTuple> evaluationChannel,
        ObserverRegistry<TTuple> observerRegistry,
        CallbackRegistry<T, TTuple, TTemplate> callbackRegistry)
    {
        this.tupleStore = tupleStore ?? throw new ArgumentNullException(nameof(tupleStore));
        this.evaluationChannel = evaluationChannel ?? throw new ArgumentNullException(nameof(evaluationChannel));
        this.observerRegistry = observerRegistry ?? throw new ArgumentNullException(nameof(observerRegistry));
        this.callbackRegistry = callbackRegistry ?? throw new ArgumentNullException(nameof(callbackRegistry));
    }

    #region ITupleActionReceiver

    void ITupleActionReceiver<TTuple>.Add(TupleAction<TTuple> action)
    {
        if (action.AgentId != agentId)
        {
            ImmutableHelpers<TTuple>.Add(ref tuples, action.Tuple);
        }
    }

    void ITupleActionReceiver<TTuple>.Remove(TupleAction<TTuple> action)
    {
        if (action.AgentId != agentId)
        {
            ImmutableHelpers<TTuple>.Remove(ref tuples, action.Tuple);
        }
    }

    void ITupleActionReceiver<TTuple>.Clear(TupleAction<TTuple> action)
    {
        if (action.AgentId != agentId)
        {
            ImmutableHelpers<TTuple>.Clear(ref tuples);
        }
    }

    #endregion

    #region ITupleRouter

    Task ITupleRouter<TTuple, TTemplate>.RouteAsync(TTuple tuple) => WriteAsync(tuple);
    async ValueTask ITupleRouter<TTuple, TTemplate>.RouteAsync(TTemplate template) => await PopAsync(template);

    #endregion

    #region ISpaceAgent

    public Guid Subscribe(ISpaceObserver<TTuple> observer)
        => observerRegistry.Add(observer);

    public void Unsubscribe(Guid observerId)
        => observerRegistry.Remove(observerId);

    public async Task WriteAsync(TTuple tuple)
    {
        ThrowHelpers.EmptyTuple(tuple);

        await tupleStore.Insert(new(agentId, tuple, TupleActionType.Insert));

        ImmutableHelpers<TTuple>.Add(ref tuples, tuple);
    }

    public ValueTask EvaluateAsync(Func<Task<TTuple>> evaluation)
    {
        if (evaluation == null) throw new ArgumentNullException(nameof(evaluation));
        return evaluationChannel.Writer.WriteAsync(evaluation);
    }

    public ValueTask<TTuple> PeekAsync(TTemplate template)
    {
        TTuple tuple = tuples.FirstOrDefault(template.Matches);
        return new(tuple);
    }

    public async ValueTask PeekAsync(TTemplate template, Func<TTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        TTuple tuple = tuples.FirstOrDefault(template.Matches);

        if (tuple.Length > 0)
        {
            await callback(tuple);
        }
        else
        {
            callbackRegistry.Add(template, new(callback, false));
        }
    }

    public async ValueTask<TTuple> PopAsync(TTemplate template)
    {
        TTuple tuple = tuples.FirstOrDefault(template.Matches);

        if (tuple.Length > 0)
        {
            await tupleStore.Remove(new(agentId, tuple, TupleActionType.Remove));
            ImmutableHelpers<TTuple>.Remove(ref tuples, tuple);
        }

        return tuple;
    }

    public async ValueTask PopAsync(TTemplate template, Func<TTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        TTuple tuple = tuples.FirstOrDefault(template.Matches);

        if (tuple.Length > 0)
        {
            await callback(tuple);
            await tupleStore.Remove(new(agentId, tuple, TupleActionType.Remove));

            ImmutableHelpers<TTuple>.Remove(ref tuples, tuple);
        }
        else
        {
            callbackRegistry.Add(template, new(callback, true));
        }
    }

    public ValueTask<IEnumerable<TTuple>> ScanAsync(TTemplate template)
    {
        List<TTuple> result = new();

        foreach (var tuple in tuples)
        {
            if (template.Matches(tuple))
            {
                result.Add(tuple);
            }
        }

        return new(result);
    }

    public async IAsyncEnumerable<TTuple> ConsumeAsync()
    {
        lock (lockObj)
        {
            if (streamChannel is null)
            {
                streamChannel = Channel.CreateUnbounded<TTuple>(new()
                {
                    SingleReader = true,
                    SingleWriter = true
                });


                foreach (var tuple in tuples)
                {
                    _ = streamChannel.Writer.TryWrite(tuple);  // will always be able to write to the channel
                }
            }
        }

        await foreach (TTuple tuple in streamChannel.Reader.ReadAllAsync())
        {
            yield return tuple;
        }
    }

    public ValueTask<int> CountAsync() => new(tuples.Length);

    public async Task ClearAsync()
    {
        await tupleStore.RemoveAll(agentId);
        ImmutableHelpers<TTuple>.Clear(ref tuples);
    }

    #endregion
}

internal class StreamProcessor<TTuple> : BackgroundService, IAsyncObserver<TupleAction<TTuple>>
    where TTuple : ISpaceTuple
{
    private readonly string key;
    private readonly IClusterClient client;
    private readonly ITupleActionReceiver<TTuple> receiver;
    private readonly ObserverChannel<TTuple> observerChannel;
    private readonly CallbackChannel<TTuple> callbackChannel;

    public StreamProcessor(
        string key,
        IClusterClient client,
        ITupleActionReceiver<TTuple> receiver,
        ObserverChannel<TTuple> observerChannel,
        CallbackChannel<TTuple> callbackChannel)
    {
        this.key = key ?? throw new ArgumentNullException(nameof(key));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        this.observerChannel = observerChannel ?? throw new ArgumentNullException(nameof(observerChannel));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await client.SubscribeAsync(this, StreamId.Create(Constants.StreamName, key));
        while (!cancellationToken.IsCancellationRequested)
        {

        }
    }

    public async Task OnNextAsync(TupleAction<TTuple> action, StreamSequenceToken? token = null)
    {
        await observerChannel.Writer.WriteAsync(action);

        switch (action.Type)
        {
            case TupleActionType.Insert:
                {
                    receiver.Add(action);
                    await callbackChannel.Writer.WriteAsync(action.Tuple);
                }
                break;
            case TupleActionType.Remove:
                receiver.Remove(action);
                break;
            case TupleActionType.Clear:
                receiver.Clear(action);
                break;
            default:
                throw new NotSupportedException();
        }
    }

    Task IAsyncObserver<TupleAction<TTuple>>.OnCompletedAsync() => Task.CompletedTask;
    Task IAsyncObserver<TupleAction<TTuple>>.OnErrorAsync(Exception e) => Task.CompletedTask;
}