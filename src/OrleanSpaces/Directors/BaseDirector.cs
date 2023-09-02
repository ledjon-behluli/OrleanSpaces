using Orleans.Runtime;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;

namespace OrleanSpaces.Directors;

internal class BaseDirector<TTuple, TStore> : Grain
    where TTuple : ISpaceTuple
    where TStore : ITupleStore<TTuple>, IGrainWithStringKey
{
    private readonly string storeKey;
    private readonly IPersistentState<HashSet<string>> storeFullKeys;

    private Guid currentStoreId;
   

    public BaseDirector(string storeKey, IPersistentState<HashSet<string>> storeFullKeys)
    {
        this.storeKey = storeKey ?? throw new ArgumentNullException(nameof(storeKey));
        this.storeFullKeys = storeFullKeys ?? throw new ArgumentNullException(nameof(storeFullKeys));
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (storeFullKeys.State is null || storeFullKeys.State.Count == 0)
        {
            await AddNewStore();
        }
        else
        {
            currentStoreId = Guid.Parse(storeFullKeys.State.Last().Split('-')[1]);
        }
    }

    public async Task<ImmutableArray<TTuple>> GetAll()
    {
        List<Task<ImmutableArray<TTuple>>> tasks = new();
        foreach (string fullKey in storeFullKeys.State)
        {
            tasks.Add(GrainFactory.GetGrain<TStore>(fullKey).GetAll());
        }

        var results = await Task.WhenAll(tasks);
        var merged = ImmutableArray<TTuple>.Empty;

        foreach (var result in results)
        {
            merged = merged.AddRange(result);
        }

        return merged;
    }

    public async Task<Guid> Insert(TupleAction<TTuple> action)
    {
        bool success = await GrainFactory.GetGrain<TStore>(CreateFullKey(currentStoreId)).Insert(action);
        if (!success)
        {
            await AddNewStore();
            await GrainFactory.GetGrain<TStore>(CreateFullKey(currentStoreId)).Insert(action);
        }

        return currentStoreId;
    }

    public async Task Remove(TupleAction<TTuple> action)
    {
        string fullKey = CreateFullKey(action.Address.StoreId);
        if (!storeFullKeys.State.Any(x => x.Equals(fullKey)))
        {
            return;
        }

        int remaning = await GrainFactory.GetGrain<TStore>(fullKey).Remove(action);
        if (remaning == 0)
        {
            storeFullKeys.State.Remove(fullKey);
            await (storeFullKeys.State.Count > 0 ? storeFullKeys.WriteStateAsync() : AddNewStore());
        }
    }

    public async Task RemoveAll(Guid agentId)
    {
        List<Task> tasks = new();
        foreach (string fullKey in storeFullKeys.State)
        {
            tasks.Add(GrainFactory.GetGrain<TStore>(fullKey).RemoveAll(agentId));
        }
        
        await Task.WhenAll();
        await AddNewStore();
    }

    private async Task AddNewStore()
    {
        currentStoreId = Guid.NewGuid();
        storeFullKeys.State = new HashSet<string> { CreateFullKey(currentStoreId) };

        await storeFullKeys.WriteStateAsync();
    }

    private string CreateFullKey(Guid id) => $"{storeKey}-{id:N}";
}