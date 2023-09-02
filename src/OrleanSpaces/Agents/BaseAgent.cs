using OrleanSpaces.Channels;
using OrleanSpaces.Helpers;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples;
using System.Diagnostics.CodeAnalysis;
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
    private readonly TupleCollection<T, TTuple, TTemplate> collection = new();

    [AllowNull] private IStoreInterceptor<TTuple> interceptor;
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
    }

    #region ISpaceRouter

    void ISpaceRouter<TTuple, TTemplate>.RouteInterceptor(IStoreInterceptor<TTuple> interceptor)
        => this.interceptor = interceptor;

    async ValueTask ISpaceRouter<TTuple, TTemplate>.RouteAction(TupleAction<TTuple> action)
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

        Guid storeId = await interceptor.Insert(new(agentId, new(tuple, Guid.Empty), TupleActionType.Insert));
        await streamChannel.WriteIfNotNull(tuple);

        collection.Add(new(tuple, storeId));
    }

    public ValueTask EvaluateAsync(Func<Task<TTuple>> evaluation)
    {
        if (evaluation == null) throw new ArgumentNullException(nameof(evaluation));
        return evaluationChannel.Writer.WriteAsync(evaluation);
    }

    public ValueTask<TTuple> PeekAsync(TTemplate template)
    {
        var address = collection.Find(template);
        return new(address.Tuple);
    }

    public async ValueTask PeekAsync(TTemplate template, Func<TTuple, Task> callback)
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

    public async ValueTask<TTuple> PopAsync(TTemplate template)
    {
        var address = collection.Find(template);
        if (!address.Tuple.IsEmpty)
        {
            await interceptor.Remove(new(agentId, address, TupleActionType.Remove));
            collection.Remove(address);
        }

        return address.Tuple;
    }

    public async ValueTask PopAsync(TTemplate template, Func<TTuple, Task> callback)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        var address = collection.Find(template);
        if (address.Tuple.IsEmpty)
        {
            callbackRegistry.Add(template, new(callback, true));
            return;
        }

        await callback(address.Tuple);
        await interceptor.Remove(new(agentId, address, TupleActionType.Remove));

        collection.Remove(address);
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
                    SingleReader = !options.AllowMultipleAgentStreamConsumers,
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
        await interceptor.RemoveAll(agentId);
        collection.Clear();
    }

    #endregion
}
