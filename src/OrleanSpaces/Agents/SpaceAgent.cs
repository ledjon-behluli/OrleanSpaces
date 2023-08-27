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
   
    private ITupleCollection tupleCollection;
    private Channel<SpaceTuple>? streamChannel;
    private double averageTupleLength;
    private double tupleLengthStdDev;

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
                tupleCollection = new ReadOptimizedCollection();
                break;
            case AgentExecutionMode.WriteOptimized:
                tupleCollection = new WriteOptimizedCollection();
                break;
            case AgentExecutionMode.Adaptable:
                {
                    tupleCollection = new WriteOptimizedCollection();
                    _ = new Timer(state => Recalibrate(), null, 0, 
                        options.AgentOptions.RecalibrationTriggerPeriod.Milliseconds);
                }
                break;
            default: 
                throw new NotSupportedException();
        }
    }

    private void Recalibrate()
    {
        RecalculateStatistics();

        if (tupleCollection.Count < 1000 && tupleCollection is ReadOptimizedCollection)
        {
            ToWriteOptimizedCollection();
        }
        else
        {
            if (tupleLengthStdDev > 50 && tupleCollection is ReadOptimizedCollection)
            {
                ToWriteOptimizedCollection();
            }

            if (tupleLengthStdDev <= 50 && tupleCollection is WriteOptimizedCollection)
            {
                ToReadOptimizedCollection();
            }
        }

        void RecalculateStatistics()
        {
            int count = 0;
            int totalLength = 0;
            double sumSquaredDifferences = 0;

            foreach (var tuple in tupleCollection)
            {
                int length = tuple.Length;

                count++;
                totalLength += length;
                sumSquaredDifferences += (length - averageTupleLength) * (length - averageTupleLength);
            }

            if (count > 0)
            {
                averageTupleLength = (double)totalLength / count;
                tupleLengthStdDev = Math.Sqrt(sumSquaredDifferences / count);
            }
            else
            {
                averageTupleLength = 0;
                tupleLengthStdDev = 0;
            }
        }

        void ToWriteOptimizedCollection()
        {
            WriteOptimizedCollection collection = new();

            foreach (var tuple in tupleCollection)
            {
                collection.Add(tuple);
            }
            
            tupleCollection = collection;
        }

        void ToReadOptimizedCollection()
        {
            ReadOptimizedCollection collection = new();

            foreach (var tuple in tupleCollection)
            {
                collection.Add(tuple);
            }

            tupleCollection = collection;
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
                        tupleCollection.Add(action.Tuple);
                        await streamChannel.WriteIfNotNull(action.Tuple);
                    }
                    break;
                case TupleActionType.Remove:
                    tupleCollection.Remove(action.Tuple);
                    break;
                case TupleActionType.Clear:
                    tupleCollection.Clear();
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

        tupleCollection.Add(tuple);
    }

    public ValueTask EvaluateAsync(Func<Task<SpaceTuple>> evaluation)
    {
        if (evaluation == null) throw new ArgumentNullException(nameof(evaluation));
        return evaluationChannel.Writer.WriteAsync(evaluation);
    }

    public ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template)
    {
        SpaceTuple tuple = tupleCollection.FirstOrDefault(template.Matches);
        return new(tuple);
    }

    public async ValueTask PeekAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        SpaceTuple tuple = tupleCollection.FirstOrDefault(template.Matches);

        if (tuple.IsEmpty)
        {
            callbackRegistry.Add(template, new(callback, false));
            return;
        }

        await callback(tuple);
    }

    public async ValueTask<SpaceTuple> PopAsync(SpaceTemplate template)
    {
        SpaceTuple tuple = tupleCollection.FirstOrDefault(template.Matches);

        if (!tuple.IsEmpty)
        {
            await tupleStore.Remove(new(agentId, tuple, TupleActionType.Remove));
            tupleCollection.Remove(tuple);
        }

        return tuple;
    }

    public async ValueTask PopAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        SpaceTuple tuple = tupleCollection.FirstOrDefault(template.Matches);

        if (tuple.IsEmpty)
        {
            callbackRegistry.Add(template, new(callback, true));
            return;
        }

        await callback(tuple);
        await tupleStore.Remove(new(agentId, tuple, TupleActionType.Remove));

        tupleCollection.Remove(tuple);
    }

    public ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template)
    {
        var result = tupleCollection.FindAll(template);
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
                    SingleReader = !options.AllowMultipleAgentStreamConsumers,
                    SingleWriter = true
                });


                foreach (var tuple in tupleCollection)
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

    public ValueTask<int> CountAsync() => new(tupleCollection.Count);

    public async Task ClearAsync()
    {
        await tupleStore.RemoveAll(agentId);
        tupleCollection.Clear();
    }

    #endregion
}