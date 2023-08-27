using OrleanSpaces.Tuples;
using OrleanSpaces.Helpers;
using System.Threading.Channels;
using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using System.Diagnostics.CodeAnalysis;
using OrleanSpaces.Collections;

namespace OrleanSpaces.Agents;

internal sealed class SpaceAgent : ISpaceAgent, ISpaceRouter<SpaceTuple, SpaceTemplate>
{
    private readonly static object lockObj = new();
    private readonly Guid agentId = Guid.NewGuid();
    private readonly SpaceOptions options;
    private readonly EvaluationChannel<SpaceTuple> evaluationChannel;
    private readonly ObserverRegistry<SpaceTuple> observerRegistry;
    private readonly CallbackRegistry callbackRegistry;

    [AllowNull] private ITupleStore<SpaceTuple> tupleStore;
   
    private ITupleCollection<SpaceTuple, SpaceTemplate> collection;
    private CollectionStatistics collectionStats;
    private Channel<SpaceTuple>? streamChannel;
   
    public SpaceAgent(
        SpaceOptions options,
        EvaluationChannel<SpaceTuple> evaluationChannel,
        ObserverRegistry<SpaceTuple> observerRegistry,
        CallbackRegistry callbackRegistry)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
        this.evaluationChannel = evaluationChannel ?? throw new ArgumentNullException(nameof(evaluationChannel));
        this.observerRegistry = observerRegistry ?? throw new ArgumentNullException(nameof(observerRegistry));
        this.callbackRegistry = callbackRegistry ?? throw new ArgumentNullException(nameof(callbackRegistry));

        switch (options.AgentOptions.ExecutionMode)
        {
            case AgentExecutionMode.ReadOptimized:
                collection = new ReadOptimizedCollection();
                break;
            case AgentExecutionMode.WriteOptimized:
                collection = new WriteOptimizedCollection();
                break;
            case AgentExecutionMode.Adaptable:
                {
                    collection = new WriteOptimizedCollection();
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

        if (collection.Count < 1000 && collection is ReadOptimizedCollection)
        {
            ToWriteOptimizedCollection();
        }
        else
        {
            if (collection is ReadOptimizedCollection &&
                collectionStats.TupleLengthRelativeStdDev > Constants.RelStdDevAgentThreshold)
            {
                ToWriteOptimizedCollection();
                return;
            }

            if (collection is WriteOptimizedCollection &&
                collectionStats.TupleLengthRelativeStdDev <= Constants.RelStdDevAgentThreshold)
            {
                ToReadOptimizedCollection();
                return;
            }
        }

        void ToWriteOptimizedCollection()
        {
            WriteOptimizedCollection collection = new();

            foreach (var tuple in this.collection)
            {
                collection.Add(tuple);
            }
            
            this.collection = collection;
        }

        void ToReadOptimizedCollection()
        {
            ReadOptimizedCollection collection = new();

            foreach (var tuple in this.collection)
            {
                collection.Add(tuple);
            }

            this.collection = collection;
        }
    }

    #region ISpaceRouter

    void ISpaceRouter<SpaceTuple, SpaceTemplate>.RouteStore(ITupleStore<SpaceTuple> tupleStore) 
        => this.tupleStore = tupleStore;

    async ValueTask ISpaceRouter<SpaceTuple, SpaceTemplate>.RouteAction(TupleAction<SpaceTuple> action)
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

    Task ISpaceRouter<SpaceTuple, SpaceTemplate>.RouteTuple(SpaceTuple tuple) => WriteAsync(tuple);
    async ValueTask ISpaceRouter<SpaceTuple, SpaceTemplate>.RouteTemplate(SpaceTemplate template) => await PopAsync(template);

    #endregion

    #region ISpaceAgent

    public Guid Subscribe(ISpaceObserver<SpaceTuple> observer)
        => observerRegistry.Add(observer);

    public void Unsubscribe(Guid observerId)
        => observerRegistry.Remove(observerId);

    public async Task WriteAsync(SpaceTuple tuple)
    {
        ThrowHelpers.EmptyTuple(tuple);

        await tupleStore.Insert(new(agentId, tuple, TupleActionType.Insert));
        await streamChannel.WriteIfNotNull(tuple);

        collection.Add(tuple);
    }

    public ValueTask EvaluateAsync(Func<Task<SpaceTuple>> evaluation)
    {
        if (evaluation == null) throw new ArgumentNullException(nameof(evaluation));
        return evaluationChannel.Writer.WriteAsync(evaluation);
    }

    public ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template)
    {
        SpaceTuple tuple = collection.FirstOrDefault(template.Matches);
        return new(tuple);
    }

    public async ValueTask PeekAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        SpaceTuple tuple = collection.FirstOrDefault(template.Matches);

        if (tuple.IsEmpty)
        {
            callbackRegistry.Add(template, new(callback, false));
            return;
        }

        await callback(tuple);
    }

    public async ValueTask<SpaceTuple> PopAsync(SpaceTemplate template)
    {
        SpaceTuple tuple = collection.FirstOrDefault(template.Matches);

        if (!tuple.IsEmpty)
        {
            await tupleStore.Remove(new(agentId, tuple, TupleActionType.Remove));
            collection.Remove(tuple);
        }

        return tuple;
    }

    public async ValueTask PopAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        SpaceTuple tuple = collection.FirstOrDefault(template.Matches);

        if (tuple.IsEmpty)
        {
            callbackRegistry.Add(template, new(callback, true));
            return;
        }

        await callback(tuple);
        await tupleStore.Remove(new(agentId, tuple, TupleActionType.Remove));

        collection.Remove(tuple);
    }

    public ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template)
    {
        var result = collection.FindAll(template);
        return new(result);
    }

    public async IAsyncEnumerable<SpaceTuple> PeekAsync()
    {
        lock (lockObj)
        {
            if (streamChannel is null)
            {
                streamChannel = Channel.CreateUnbounded<SpaceTuple>(new()
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

        await foreach (SpaceTuple tuple in streamChannel.Reader.ReadAllAsync())
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