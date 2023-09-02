using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Helpers;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;

namespace OrleanSpaces.Interceptors;

internal class BaseInterceptor<TTuple, TStore> : Grain, IAsyncObserver<Guid>
    where TTuple : ISpaceTuple
    where TStore : ITupleStore<TTuple>, IGrainWithStringKey
{
    private readonly string storeKey;
    private readonly IPersistentState<HashSet<Guid>> storeIds;

    private Guid currentStoreId;
    private TStore CurrentStore => GrainFactory.GetGrain<TStore>($"{storeKey}-{currentStoreId:N}");

    public BaseInterceptor(string storeKey, IPersistentState<HashSet<Guid>> storeIds)
    {
        this.storeKey = storeKey ?? throw new ArgumentNullException(nameof(storeKey));
        this.storeIds = storeIds ?? throw new ArgumentNullException(nameof(storeIds));
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (storeIds.State is null || storeIds.State.Count == 0)
        {
            Guid id = Guid.NewGuid();
            storeIds.State = new HashSet<Guid> { id };
            currentStoreId = id;
            
            await storeIds.WriteStateAsync();
        }

        var stream = this
           .GetStreamProvider(Constants.PubSubProvider)
           .GetStream<Guid>(StreamId.Create(Constants.StreamName, storeKey));

        await stream.SubscribeOrResumeAsync(this);
    }

    public async Task<ImmutableArray<TTuple>> GetAll()
    {
        List<Task<ImmutableArray<TTuple>>> tasks = new();

        foreach (var storeId in storeIds.State)
        {
            tasks.Add(GrainFactory.GetGrain<TStore>($"{storeKey}-{storeId:N}").GetAll());
        }

        var results = await Task.WhenAll(tasks);
        var mergedResult = ImmutableArray<TTuple>.Empty;

        foreach (var result in results)
        {
            mergedResult = mergedResult.AddRange(result);
        }

        return mergedResult;
    }

    public async Task<Guid> Insert(TupleAction<TTuple> action)
    {
        await CurrentStore.Insert(action);
        return currentStoreId;
    }

    public Task Remove(TupleAction<TTuple> action) => CurrentStore.Remove(action);
    public Task RemoveAll(Guid agentId) => CurrentStore.RemoveAll(agentId);

    public async Task OnNewStoreCreated(Guid storeId)
    {
        currentStoreId = storeId;
        storeIds.State.Add(storeId);

        await storeIds.WriteStateAsync();
    }

    public async Task OnNextAsync(Guid item, StreamSequenceToken? token = null)
    {
        storeIds.State.Remove(item);
        await storeIds.WriteStateAsync();
    }

    public Task OnCompletedAsync() => Task.CompletedTask;
    public Task OnErrorAsync(Exception ex) => Task.CompletedTask;
}