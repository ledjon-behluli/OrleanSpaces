using Microsoft.Extensions.DependencyInjection;
using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Grains.Stores;

internal abstract class BaseStore<T> : Grain
    where T : struct, ISpaceTuple, IEquatable<T>
{
    private readonly string realmKey;
    private readonly IPersistentState<List<T>> state;

    private int partitionThreshold;
    [AllowNull] private IAsyncStream<TupleAction<T>> stream;

    public BaseStore(string realmKey, IPersistentState<List<T>> state)
    {
        this.realmKey = realmKey ?? throw new ArgumentNullException(nameof(realmKey));
        this.state = state ?? throw new ArgumentNullException(nameof(state));
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        partitionThreshold = ServiceProvider.GetRequiredService<SpaceServerOptions>().PartitionThreshold;
        stream = this.GetStreamProvider(Constants.PubSubProvider)
            .GetStream<TupleAction<T>>(StreamId.Create(Constants.StreamName, realmKey));

        return Task.CompletedTask;
    }

    public Task<StoreContent<T>> GetAll() => 
        Task.FromResult(new StoreContent<T>(this.GetPrimaryKeyString(), state.State.ToImmutableArray()));

    public async Task<bool> Insert(TupleAction<T> action)
    {
        if (state.State.Count == partitionThreshold)
        {
            return false;
        }

        state.State.Add(action.Address.Tuple);

        await state.WriteStateAsync();
        await stream.OnNextAsync(action);

        return true;
    }

    public async Task<int> Remove(TupleAction<T> action)
    {
        var storedTuple = state.State.FirstOrDefault(x => x.Equals(action.Address.Tuple));
        if (!storedTuple.IsEmpty)
        {
            state.State.Remove(storedTuple);
            if (state.State.Count == 0)
            {
                await state.ClearStateAsync();
                DeactivateOnIdle();

                return 0;
            }

            await state.WriteStateAsync();
            await stream.OnNextAsync(action);
        }

        return state.State.Count;
    }

    public async Task RemoveAll(Guid agentId)
    {
        await stream.OnNextAsync(new(agentId, new(), TupleActionType.Clear));
        await state.ClearStateAsync();

        DeactivateOnIdle();
    }
}