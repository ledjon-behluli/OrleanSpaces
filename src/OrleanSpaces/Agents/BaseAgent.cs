using OrleanSpaces.Channels;
using OrleanSpaces.Collections;
using OrleanSpaces.Helpers;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Intrinsics.X86;
using System.Threading.Channels;

namespace OrleanSpaces.Agents;

internal class BaseAgent<T, TTuple, TTemplate> : ISpaceAgent<T, TTuple, TTemplate>, ISpaceRouter<TTuple, TTemplate>
    where T : unmanaged
    where TTuple : struct, ISpaceTuple<T>
    where TTemplate : struct, ISpaceTemplate<T>, ISpaceMatchable<T, TTuple>
{
    private readonly static object lockObj = new();
    private readonly Guid agentId = Guid.NewGuid();
    private readonly SpaceOptions options;
    private readonly EvaluationChannel<TTuple> evaluationChannel;
    private readonly ObserverRegistry<TTuple> observerRegistry;
    private readonly CallbackRegistry<T, TTuple, TTemplate> callbackRegistry;

    [AllowNull] private ITupleStore<TTuple> tupleStore;
    
    private ITupleCollection<TTuple, TTemplate> collection;
    private CollectionStatistics collectionStats;
    private Channel<TTuple>? streamChannel;

    public BaseAgent(
        SpaceOptions options,
        EvaluationChannel<TTuple> evaluationChannel,
        ObserverRegistry<TTuple> observerRegistry,
        CallbackRegistry<T, TTuple, TTemplate> callbackRegistry)
    {
        this.options = options;
        this.evaluationChannel = evaluationChannel ?? throw new ArgumentNullException(nameof(evaluationChannel));
        this.observerRegistry = observerRegistry ?? throw new ArgumentNullException(nameof(observerRegistry));
        this.callbackRegistry = callbackRegistry ?? throw new ArgumentNullException(nameof(callbackRegistry));

        switch (options.AgentOptions.ExecutionMode)
        {
            case AgentExecutionMode.ReadOptimized:
                collection = new ReadOptimizedCollection<T, TTuple, TTemplate>();
                break;
            case AgentExecutionMode.WriteOptimized:
                collection = new WriteOptimizedCollection<T, TTuple, TTemplate>();
                break;
            case AgentExecutionMode.Adaptable:
                {
                    collection = new WriteOptimizedCollection<T, TTuple, TTemplate>();
                    _ = new Timer(state => OptimizeCollectionType(), null, 0,
                        options.AgentOptions.OptimizationTriggerPeriod.Milliseconds);
                }
                break;
            default:
                throw new NotSupportedException();
        }
    }

    private void OptimizeCollectionType()
    {
        /*
         1) If the total number of tuples is small -
            Use WriteOptimizedCollection regardless of tuple lengths.

         2) If the total number of tuples is large -
            2.1) If the standard deviation of tuple lengths is high -
                 Use WriteOptimizedCollection since there's a wide variation in lengths, which makes it closer to having distinct dictionary keys.

            2.2) If the standard deviation of tuple lengths is low -
               Use ReadOptimizedCollection since lengths are relatively uniform, and we can benefit from the dictionary's key-based filtering for better find performance.
        */

        collectionStats = collection.Calculate(collectionStats);

        if (collection.Count < 1_000 && collection is ReadOptimizedCollection<T, TTuple, TTemplate>)
        {
            ToWriteOptimizedCollection();
        }
        else
        {
            //TODO: Implement hysteresis
            if (collectionStats.TupleLengthRelativeStdDev > Constants.RelStdDevAgentThreshold && 
                collection is ReadOptimizedCollection<T, TTuple, TTemplate>)
            {
                ToWriteOptimizedCollection();
            }

            if (collectionStats.TupleLengthRelativeStdDev <= Constants.RelStdDevAgentThreshold && 
                collection is WriteOptimizedCollection<T, TTuple, TTemplate>)
            {
                ToReadOptimizedCollection();
            }
        }

        void ToWriteOptimizedCollection()
        {
            WriteOptimizedCollection<T, TTuple, TTemplate> collection = new();

            foreach (var tuple in this.collection)
            {
                collection.Add(tuple);
            }

            this.collection = collection;
        }

        void ToReadOptimizedCollection()
        {
            ReadOptimizedCollection<T, TTuple, TTemplate> collection = new();

            foreach (var tuple in this.collection)
            {
                collection.Add(tuple);
            }

            this.collection = collection;
        }
    }

    #region ISpaceRouter

    void ISpaceRouter<TTuple, TTemplate>.RouteStore(ITupleStore<TTuple> tupleStore)
        => this.tupleStore = tupleStore;

    async ValueTask ISpaceRouter<TTuple, TTemplate>.RouteAction(TupleAction<TTuple> action)
    {
        if (action.AgentId != agentId)
        {
            switch (action.Type)
            {
                case TupleActionType.Insert:
                    {
                        collection.Add(action.Tuple);
                        await streamChannel.WriteIfNotNull(action.Tuple);
                    }
                    break;
                case TupleActionType.Remove:
                    collection.Remove(action.Tuple);
                    break;
                case TupleActionType.Clear:
                    collection.Clear();
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }

    Task ISpaceRouter<TTuple, TTemplate>.RouteTuple(TTuple tuple) => WriteAsync(tuple);
    async ValueTask ISpaceRouter<TTuple, TTemplate>.RouteTemplate(TTemplate template) => await PopAsync(template);

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
        await streamChannel.WriteIfNotNull(tuple);

        collection.Add(tuple);
    }

    public ValueTask EvaluateAsync(Func<Task<TTuple>> evaluation)
    {
        if (evaluation == null) throw new ArgumentNullException(nameof(evaluation));
        return evaluationChannel.Writer.WriteAsync(evaluation);
    }

    public ValueTask<TTuple> PeekAsync(TTemplate template)
    {
        TTuple tuple = collection.Find(template);
        return new(tuple);
    }

    public async ValueTask PeekAsync(TTemplate template, Func<TTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        TTuple tuple = collection.Find(template);

        if (tuple.IsEmpty)
        {
            callbackRegistry.Add(template, new(callback, false));
            return;
        }

        await callback(tuple);
    }

    public async ValueTask<TTuple> PopAsync(TTemplate template)
    {
        TTuple tuple = collection.Find(template);

        if (!tuple.IsEmpty)
        {
            await tupleStore.Remove(new(agentId, tuple, TupleActionType.Remove));
            collection.Remove(tuple);
        }

        return tuple;
    }

    public async ValueTask PopAsync(TTemplate template, Func<TTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        TTuple tuple = collection.Find(template);

        if (tuple.IsEmpty)
        {
            callbackRegistry.Add(template, new(callback, true));
            return;
        }

        await callback(tuple);
        await tupleStore.Remove(new(agentId, tuple, TupleActionType.Remove));

        collection.Remove(tuple);
    }

    public ValueTask<IEnumerable<TTuple>> ScanAsync(TTemplate template)
    {
        var result = collection.FindAll(template);
        return new(result);
    }

    public async IAsyncEnumerable<TTuple> PeekAsync()
    {
        lock (lockObj)
        {
            if (streamChannel is null)
            {
                streamChannel = Channel.CreateUnbounded<TTuple>(new()
                {
                    SingleReader = !options.AgentOptions.AllowMultipleAgentStreamConsumers,
                    SingleWriter = true
                });


                foreach (var tuple in collection)
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

    public ValueTask<int> CountAsync() => new(collection.Count);

    public async Task ClearAsync()
    {
        await tupleStore.RemoveAll(agentId);
        collection.Clear();
    }

    #endregion
}
