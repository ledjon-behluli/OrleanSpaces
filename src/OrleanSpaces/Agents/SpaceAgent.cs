﻿using OrleanSpaces.Tuples;
using OrleanSpaces.Helpers;
using System.Threading.Channels;
using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Agents;

internal sealed class SpaceAgent : ISpaceAgent, ISpaceRouter<SpaceTuple, SpaceTemplate>
{
    private readonly static object lockObj = new();
    private readonly Guid agentId = Guid.NewGuid();
    private readonly SpaceClientOptions options;
    private readonly EvaluationChannel<SpaceTuple> evaluationChannel;
    private readonly ObserverRegistry<SpaceTuple> observerRegistry;
    private readonly CallbackRegistry callbackRegistry;
    private readonly TupleCollection collection = new();

    [AllowNull] private IStoreDirector<SpaceTuple> director;
    private Channel<SpaceTuple>? streamChannel;
   
    public SpaceAgent(
        SpaceClientOptions options,
        EvaluationChannel<SpaceTuple> evaluationChannel,
        ObserverRegistry<SpaceTuple> observerRegistry,
        CallbackRegistry callbackRegistry)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
        this.evaluationChannel = evaluationChannel ?? throw new ArgumentNullException(nameof(evaluationChannel));
        this.observerRegistry = observerRegistry ?? throw new ArgumentNullException(nameof(observerRegistry));
        this.callbackRegistry = callbackRegistry ?? throw new ArgumentNullException(nameof(callbackRegistry));
    }

    #region ISpaceRouter

    async ValueTask ISpaceRouter<SpaceTuple, SpaceTemplate>.RouteDirector(IStoreDirector<SpaceTuple> director)
    {
        var addresses = await director.GetAll();
        foreach (var address in addresses)
        {
            collection.Add(address);
        }

        this.director = director;
    }

    async ValueTask ISpaceRouter<SpaceTuple, SpaceTemplate>.RouteAction(TupleAction<SpaceTuple> action)
    {
        if (action.AgentId != agentId)
        {
            switch (action.Type)
            {
                case TupleActionType.Insert:
                    {
                        collection.Add(action.Address);
                        await streamChannel.WriteIfNotNull(action.Address.Tuple);
                    }
                    break;
                case TupleActionType.Remove:
                    collection.Remove(action.Address);
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

        Guid storeId = await director.Insert(new(agentId, new(tuple, Guid.Empty), TupleActionType.Insert));
        await streamChannel.WriteIfNotNull(tuple);

        collection.Add(new(tuple, storeId));
    }

    public ValueTask EvaluateAsync(Func<Task<SpaceTuple>> evaluation)
    {
        if (evaluation == null) throw new ArgumentNullException(nameof(evaluation));
        return evaluationChannel.Writer.WriteAsync(evaluation);
    }

    public ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template)
    {
        var address = collection.Find(template);
        return new(address.Tuple);
    }

    public async ValueTask PeekAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        var address = collection.Find(template);
        if (address.Tuple.IsEmpty)
        {
            callbackRegistry.Add(template, new(callback, false));
            return;
        }

        await callback(address.Tuple);
    }

    public async ValueTask<SpaceTuple> PopAsync(SpaceTemplate template)
    {
        var address = collection.Find(template);

        if (!address.Tuple.IsEmpty)
        {
            await director.Remove(new(agentId, address, TupleActionType.Remove));
            collection.Remove(address);
        }

        return address.Tuple;
    }

    public async ValueTask PopAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        var address = collection.Find(template);
        if (address.Tuple.IsEmpty)
        {
            callbackRegistry.Add(template, new(callback, true));
            return;
        }

        await callback(address.Tuple);
        await director.Remove(new(agentId, address, TupleActionType.Remove));

        collection.Remove(address);
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
                    SingleReader = !options.AllowMultipleAgentStreamConsumers,
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
        await director.RemoveAll(agentId);
        collection.Clear();
    }

    #endregion
}