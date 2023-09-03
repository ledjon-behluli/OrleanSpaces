using Orleans.Runtime;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;

namespace OrleanSpaces.Grains.Directors;

internal class BaseDirector<TTuple, TStore> : Grain
    where TTuple : ISpaceTuple
    where TStore : ITupleStore<TTuple>, IGrainWithStringKey
{
    private readonly string realmKey;
    private readonly IPersistentState<HashSet<string>> storeKeys;

    private Guid currentStoreId;


    public BaseDirector(string realmKey, IPersistentState<HashSet<string>> storeKeys)
    {
        this.realmKey = realmKey ?? throw new ArgumentNullException(nameof(realmKey));
        this.storeKeys = storeKeys ?? throw new ArgumentNullException(nameof(storeKeys));
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (storeKeys.State is null || storeKeys.State.Count == 0)
        {
            await AddNewStore();
        }
        else
        {
            currentStoreId = Guid.Parse(storeKeys.State.Last().Split('-')[1]);
        }
    }

    public async Task<ImmutableArray<TTuple>> GetAll()
    {
        List<Task<ImmutableArray<TTuple>>> tasks = new();
        foreach (string fullKey in storeKeys.State)
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
        if (!storeKeys.State.Any(x => x.Equals(fullKey)))
        {
            return;
        }

        int remaning = await GrainFactory.GetGrain<TStore>(fullKey).Remove(action);
        if (remaning == 0)
        {
            storeKeys.State.Remove(fullKey);
            await (storeKeys.State.Count > 0 ? storeKeys.WriteStateAsync() : AddNewStore());
        }
    }

    public async Task RemoveAll(Guid agentId)
    {
        List<Task> tasks = new();
        foreach (string fullKey in storeKeys.State)
        {
            tasks.Add(GrainFactory.GetGrain<TStore>(fullKey).RemoveAll(agentId));
        }

        await Task.WhenAll();
        await AddNewStore();
    }

    private async Task AddNewStore()
    {
        currentStoreId = Guid.NewGuid();
        storeKeys.State = new HashSet<string> { CreateFullKey(currentStoreId) };

        await storeKeys.WriteStateAsync();
    }

    private string CreateFullKey(Guid id) => $"{realmKey}-{id:N}";
}