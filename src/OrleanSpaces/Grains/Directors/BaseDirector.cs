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
        if (storeKeys.State.Count == 0)
        {
            await AddNewStore();
        }
        else
        {
            currentStoreId = Guid.Parse(storeKeys.State.Last().Split('-')[1]);
        }
    }

    public async Task<ImmutableArray<TupleAddress<TTuple>>> GetAll()
    {
        List<Task<StoreContent<TTuple>>> tasks = new();
        foreach (string storeKey in storeKeys.State)
        {
            tasks.Add(GrainFactory.GetGrain<TStore>(storeKey).GetAll());
        }

        StoreContent<TTuple>[] contents = await Task.WhenAll(tasks);
        List<TupleAddress<TTuple>> result = new();

        foreach (var content in contents)
        {
            Guid storeId = ParseStoreKey(content.StoreKey);
            foreach (var tuple in content.Tuples)
            {
                result.Add(new(tuple, storeId));
            }
        }

        return result.ToImmutableArray();
    }

    public async Task<Guid> Insert(TupleAction<TTuple> action)
    {
        bool success = await GrainFactory.GetGrain<TStore>(CreateStoreKey(currentStoreId)).Insert(action);
        if (!success)
        {
            await AddNewStore();
            await GrainFactory.GetGrain<TStore>(CreateStoreKey(currentStoreId)).Insert(action);
        }

        return currentStoreId;
    }

    public async Task Remove(TupleAction<TTuple> action)
    {
        string storeKey = CreateStoreKey(action.Address.StoreId);
        if (!storeKeys.State.Any(x => x.Equals(storeKey)))
        {
            return;
        }

        int remaning = await GrainFactory.GetGrain<TStore>(storeKey).Remove(action);
        if (remaning == 0)
        {
            storeKeys.State.Remove(storeKey);
            await (storeKeys.State.Count > 0 ? storeKeys.WriteStateAsync() : AddNewStore());
        }
    }

    public async Task RemoveAll(Guid agentId)
    {
        List<Task> tasks = new();
        foreach (string storeKey in storeKeys.State)
        {
            tasks.Add(GrainFactory.GetGrain<TStore>(storeKey).RemoveAll(agentId));
        }

        await Task.WhenAll();
        await AddNewStore();
    }

    private async Task AddNewStore()
    {
        currentStoreId = Guid.NewGuid();
        storeKeys.State = new HashSet<string> { CreateStoreKey(currentStoreId) };

        await storeKeys.WriteStateAsync();
    }

    private string CreateStoreKey(Guid id) => $"{realmKey}-{id:N}";
    private static Guid ParseStoreKey(string storeKey) => Guid.Parse(storeKey.Split('-')[1]);
}