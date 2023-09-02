﻿using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Grains;

internal abstract class BaseGrain<T> : Grain
    where T : struct, ISpaceTuple, IEquatable<T>
{
    private readonly string storeKey;
    private readonly IPersistentState<List<T>> space;

    [AllowNull] private IAsyncStream<TupleAction<T>> stream;

    public BaseGrain(string storeKey, IPersistentState<List<T>> space)
    {
        this.storeKey = storeKey ?? throw new ArgumentNullException(nameof(storeKey));
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        stream = this
            .GetStreamProvider(Constants.PubSubProvider)
            .GetStream<TupleAction<T>>(StreamId.Create(Constants.StreamName, storeKey));

        return Task.CompletedTask;
    }

    public Task<ImmutableArray<T>> GetAll() => Task.FromResult(space.State.ToImmutableArray());

    public async Task<bool> Insert(TupleAction<T> action)
    {
        if (space.State.Count == Constants.MaxTuplesPerShard)
        {
            return false;
        }

        space.State.Add(action.Address.Tuple);

        await space.WriteStateAsync();
        await stream.OnNextAsync(action);

        return true;
    }

    public async Task<int> Remove(TupleAction<T> action)
    {
        var storedTuple = space.State.FirstOrDefault(x => x.Equals(action.Address.Tuple));
        if (!storedTuple.IsEmpty)
        {
            space.State.Remove(storedTuple);
            if (space.State.Count == 0)
            {
                await space.ClearStateAsync();
                DeactivateOnIdle();

                return 0;
            }

            await space.WriteStateAsync();
            await stream.OnNextAsync(action);
        }

        return space.State.Count;
    }

    public async Task RemoveAll(Guid agentId)
    {
        await stream.OnNextAsync(new(agentId, new(), TupleActionType.Clear));
        await space.ClearStateAsync();

        DeactivateOnIdle();
    }
}