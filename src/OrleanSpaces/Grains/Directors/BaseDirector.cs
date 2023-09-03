using Orleans.Runtime;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;

namespace OrleanSpaces.Grains.Directors;

[GenerateSerializer]
internal sealed class DirectorState
{
    [Id(0)] public Guid CurrentStoreId { get; set; }
    [Id(1)] public HashSet<string> StoreKeys { get; set; } = new();
}

internal class BaseDirector<TTuple, TStore> : Grain
    where TTuple : ISpaceTuple
    where TStore : ITupleStore<TTuple>, IGrainWithStringKey
{
    private readonly string realmKey;
    private readonly IPersistentState<DirectorState> state;

    private Guid CurrentStoreId
    {
        get => state.State.CurrentStoreId;
        set => state.State.CurrentStoreId = value;
    }
    private HashSet<string> StoreKeys => state.State.StoreKeys;

    public BaseDirector(string realmKey, IPersistentState<DirectorState> state)
    {
        this.realmKey = realmKey ?? throw new ArgumentNullException(nameof(realmKey));
        this.state = state ?? throw new ArgumentNullException(nameof(state));
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (state.State.StoreKeys.Count == 0)
        {
            await AddNewStore();
        }
    }

    public async Task<ImmutableArray<TupleAddress<TTuple>>> GetAll()
    {
        List<Task<StoreContent<TTuple>>> tasks = new();
        foreach (string storeKey in StoreKeys)
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
        bool success = await GrainFactory.GetGrain<TStore>(CreateStoreKey(CurrentStoreId)).Insert(action);
        if (!success)
        {
            await AddNewStore();
            await GrainFactory.GetGrain<TStore>(CreateStoreKey(CurrentStoreId)).Insert(action);
        }

        return CurrentStoreId;
    }

    public async Task Remove(TupleAction<TTuple> action)
    {
        string storeKey = CreateStoreKey(action.Address.StoreId);
        if (!StoreKeys.Any(x => x.Equals(storeKey)))
        {
            return;
        }

        int remaning = await GrainFactory.GetGrain<TStore>(storeKey).Remove(action);
        if (remaning == 0)
        {
            StoreKeys.Remove(storeKey);
            await (StoreKeys.Count > 0 ? state.WriteStateAsync() : AddNewStore());
        }
    }

    public async Task RemoveAll(Guid agentId)
    {
        List<Task> tasks = new();
        foreach (string storeKey in StoreKeys)
        {
            tasks.Add(GrainFactory.GetGrain<TStore>(storeKey).RemoveAll(agentId));
        }

        await Task.WhenAll();
        await AddNewStore();
    }

    private async Task AddNewStore()
    {
        CurrentStoreId = Guid.NewGuid();
        StoreKeys.Add(CreateStoreKey(CurrentStoreId));

        await state.WriteStateAsync();
    }

    private string CreateStoreKey(Guid id) => $"{realmKey}-{id:N}";
    private static Guid ParseStoreKey(string storeKey) => Guid.ParseExact(storeKey[^32..], "N");
}