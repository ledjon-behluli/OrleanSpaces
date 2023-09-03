using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Grains.Directors;

[GenerateSerializer]
internal sealed class DirectorState
{
    [Id(0)] public Guid CurrentStoreId { get; set; }
    [Id(1)] public HashSet<string> StoreKeys { get; set; } = new();
    [Id(2)] public TransactionState Transaction { get; set; } = new();

    [GenerateSerializer]
    internal sealed class TransactionState
    {
        [Id(0)] public bool IsRunning { get; set; }
        [Id(1)] public Guid AgentId { get; set; }
    }
}

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
    private DirectorState.TransactionState Transaction => state.State.Transaction;

    public BaseDirector(string realmKey, IPersistentState<DirectorState> state)
    {
        this.realmKey = realmKey ?? throw new ArgumentNullException(nameof(realmKey));
        this.state = state ?? throw new ArgumentNullException(nameof(state));
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        stream = this.GetStreamProvider(Constants.PubSubProvider)
           .GetStream<TupleAction<TTuple>>(StreamId.Create(Constants.StreamName, realmKey));

        if (Transaction.IsRunning)
        {
            await RemoveAll(Transaction.AgentId); // method is idempotant so its safe to call it again in case of partial failures.
        }

        if (StoreKeys.Count == 0)
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

        await stream.OnNextAsync(action);
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

        await stream.OnNextAsync(action);
    }

    public async Task RemoveAll(Guid agentId)
    {
        List<Task> tasks = new();
        foreach (string storeKey in StoreKeys)
        {
            tasks.Add(GrainFactory.GetGrain<TStore>(storeKey).RemoveAll());
        }

        Transaction.IsRunning = true;
        await state.WriteStateAsync();

        await Task.WhenAll();

        StoreKeys.Clear();
        Transaction.IsRunning = false;

        await AddNewStore();
        await stream.OnNextAsync(new(agentId, TupleAddress<TTuple>.Empty, TupleActionType.Clear));
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