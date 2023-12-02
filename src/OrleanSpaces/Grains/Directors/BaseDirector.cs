using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Grains.Directors;

internal class BaseDirector<TTuple, TStore> : Grain
    where TTuple : ISpaceTuple
    where TStore : ITupleStore<TTuple>, IGrainWithStringKey
{
    private readonly string realmKey;
    private readonly IPersistentState<DirectorState> state;

    [AllowNull] private IAsyncStream<TupleAction<TTuple>> stream;

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
        stream = this.GetStreamProvider(Constants.PubSubProvider)
           .GetStream<TupleAction<TTuple>>(StreamId.Create(Constants.StreamName, realmKey));

        if (state.State.HasTransaction)
        {
            await RemoveAll(state.State.AgentId); // method is idempotant so its safe to call it again in case of partial failures.
        }

        if (StoreKeys.Count == 0)
        {
            await AddNewStore();
        }
    }

    public async IAsyncEnumerable<ImmutableArray<StoreTuple<TTuple>>> GetBatch()
    {
        foreach (string storeKey in StoreKeys)
        {
            Guid storeId = ParseStoreKey(storeKey);
            var content = await GrainFactory.GetGrain<TStore>(storeKey).GetAll();
            var result = content.Tuples.Select(tuple => new StoreTuple<TTuple>(storeId, tuple)).ToImmutableArray();

            yield return result;
        }
    }

    public async Task<ImmutableArray<StoreTuple<TTuple>>> GetAllBatches()
    {
        List<Task<StoreContent<TTuple>>> tasks = new();
        foreach (string storeKey in StoreKeys)
        {
            tasks.Add(GrainFactory.GetGrain<TStore>(storeKey).GetAll());
        }

        StoreContent<TTuple>[] contents = await Task.WhenAll(tasks);
        List<StoreTuple<TTuple>> result = new();

        foreach (var content in contents)
        {
            Guid storeId = ParseStoreKey(content.StoreKey);
            foreach (var tuple in content.Tuples)
            {
                result.Add(new(storeId, tuple));
            }
        }

        return result.ToImmutableArray();
    }

    public async Task<Guid> Insert(TupleAction<TTuple> action)
    {
        bool success = await GrainFactory.GetGrain<TStore>(CreateStoreKey(CurrentStoreId)).Insert(action.StoreTuple.Tuple);
        if (!success)
        {
            await AddNewStore();
            await GrainFactory.GetGrain<TStore>(CreateStoreKey(CurrentStoreId)).Insert(action.StoreTuple.Tuple);
        }

        TupleAction<TTuple> newAction = new(action.AgentId, new(CurrentStoreId, action.StoreTuple.Tuple), action.Type);
        await stream.OnNextAsync(newAction);

        return CurrentStoreId;
    }

    public async Task Remove(TupleAction<TTuple> action)
    {
        string storeKey = CreateStoreKey(action.StoreTuple.StoreId);
        if (!StoreKeys.Any(x => x.Equals(storeKey)))
        {
            return;
        }

        int remaning = await GrainFactory.GetGrain<TStore>(storeKey).Remove(action.StoreTuple.Tuple);
        if (remaning == 0)
        {
            StoreKeys.Remove(storeKey);
            await (StoreKeys.Count > 0 ? state.WriteStateAsync() : AddNewStore());
        }

        await stream.OnNextAsync(action);
    }

    public async Task RemoveAll(Guid agentId)
    {
        List<Task> tasks = new();
        foreach (string storeKey in StoreKeys)
        {
            tasks.Add(GrainFactory.GetGrain<TStore>(storeKey).RemoveAll());
        }

        state.State.HasTransaction = true;
        await state.WriteStateAsync();

        await Task.WhenAll();

        StoreKeys.Clear();
        state.State.HasTransaction = false;

        await AddNewStore();
        await stream.OnNextAsync(new(agentId, new(), TupleActionType.Clear));
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

[GenerateSerializer]
internal sealed class DirectorState
{
    [Id(0)] public Guid CurrentStoreId { get; set; }
    [Id(1)] public HashSet<string> StoreKeys { get; set; } = new();
    [Id(2)] public bool HasTransaction { get; set; }
    [Id(3)] public Guid AgentId { get; set; }
}