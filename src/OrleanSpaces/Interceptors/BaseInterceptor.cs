using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Helpers;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;

namespace OrleanSpaces.Interceptors;

internal class BaseInterceptor<TTuple, TStore> : Grain, IAsyncObserver<string>
    where TTuple : ISpaceTuple
    where TStore : ITupleStore<TTuple>, IGrainWithStringKey
{
    private readonly string storeKey;
    private readonly IPersistentState<HashSet<string>> storeIds;

    private Guid currentStoreId;

    private string CurrentStoreKey => $"{storeKey}-{currentStoreId:N}";
    private TStore CurrentStore => GrainFactory.GetGrain<TStore>(CurrentStoreKey);

    public BaseInterceptor(string storeKey, IPersistentState<HashSet<string>> storeIds)
    {
        this.storeKey = storeKey ?? throw new ArgumentNullException(nameof(storeKey));
        this.storeIds = storeIds ?? throw new ArgumentNullException(nameof(storeIds));
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (storeIds.State is null || storeIds.State.Count == 0)
        {
            currentStoreId = Guid.NewGuid();
            storeIds.State = new HashSet<string> { CurrentStoreKey };
            
            await storeIds.WriteStateAsync();
        }

        var stream = this
           .GetStreamProvider(Constants.PubSubProvider)
           .GetStream<string>(StreamId.Create(Constants.StoreMetadata_StreamNamespace, storeKey));

        await stream.SubscribeOrResumeAsync(this);
    }

    public async Task<ImmutableArray<TTuple>> GetAll()
    {
        List<Task<ImmutableArray<TTuple>>> tasks = new();

        foreach (var storeId in storeIds.State)
        {
            tasks.Add(GrainFactory.GetGrain<TStore>(storeId).GetAll());
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
        bool success = await CurrentStore.Insert(action);
        if (!success)
        {
            currentStoreId = Guid.NewGuid();
            await CurrentStore.Insert(action);

            storeIds.State.Add(CurrentStoreKey);
            await storeIds.WriteStateAsync();
        }

        return currentStoreId;
    }

    public Task Remove(TupleAction<TTuple> action) => CurrentStore.Remove(action);
    public Task RemoveAll(Guid agentId) => CurrentStore.RemoveAll(agentId);

    public async Task OnNextAsync(string item, StreamSequenceToken? token = null)
    {
        storeIds.State.Remove(item);
        await storeIds.WriteStateAsync();
    }

    public Task OnCompletedAsync() => Task.CompletedTask;
    public Task OnErrorAsync(Exception ex) => Task.CompletedTask;
}