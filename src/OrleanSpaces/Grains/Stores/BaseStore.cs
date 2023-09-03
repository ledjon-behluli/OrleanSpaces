using Microsoft.Extensions.DependencyInjection;
using Orleans.Runtime;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;

namespace OrleanSpaces.Grains.Stores;

internal abstract class BaseStore<T> : Grain
    where T : struct, ISpaceTuple, IEquatable<T>
{
    private readonly IPersistentState<List<T>> state;

    private int partitionThreshold;

    private List<T> Tuples => state.State;

    public BaseStore(IPersistentState<List<T>> state) =>
        this.state = state ?? throw new ArgumentNullException(nameof(state));

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        partitionThreshold = ServiceProvider.GetRequiredService<SpaceServerOptions>().PartitionThreshold;
        return Task.CompletedTask;
    }

    public Task<StoreContent<T>> GetAll() => 
        Task.FromResult(new StoreContent<T>(this.GetPrimaryKeyString(), Tuples.ToImmutableArray()));

    public async Task<bool> Insert(TupleAction<T> action)
    {
        if (Tuples.Count == partitionThreshold)
        {
            return false;
        }

        Tuples.Add(action.Address.Tuple);

        await state.WriteStateAsync();
        return true;
    }

    public async Task<int> Remove(TupleAction<T> action)
    {
        var tuple = Tuples.FirstOrDefault(x => x.Equals(action.Address.Tuple));
        Tuples.Remove(tuple);

        if (Tuples.Count == 0)
        {
            await state.ClearStateAsync();
            DeactivateOnIdle();
            return 0;
        }

        await state.WriteStateAsync();
        return Tuples.Count;
    }

    public async Task RemoveAll()
    {
        await state.ClearStateAsync();
        DeactivateOnIdle();
    }
}