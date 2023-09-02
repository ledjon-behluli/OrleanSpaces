using Orleans.Runtime;
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
    
    [AllowNull] private IAsyncStream<TupleAction<T>> storeStream;
    [AllowNull] private IAsyncStream<string> interceptorStream;

    public BaseGrain(string storeKey, IPersistentState<List<T>> space)
    {
        this.storeKey = storeKey ?? throw new ArgumentNullException(nameof(storeKey));
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        StreamId storeStreamId = StreamId.Create(Constants.Store_StreamNamespace, storeKey);
        StreamId interceptorStreamId = StreamId.Create(Constants.StoreMetadata_StreamNamespace, storeKey);

        var streamProvider = this.GetStreamProvider(Constants.PubSubProvider);

        storeStream = streamProvider.GetStream<TupleAction<T>>(storeStreamId);
        interceptorStream = streamProvider.GetStream<string>(interceptorStreamId);

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
        await storeStream.OnNextAsync(action);

        return true;
    }

    public async Task Remove(TupleAction<T> action)
    {
        var storedTuple = space.State.FirstOrDefault(x => x.Equals(action.Address.Tuple));
        if (!storedTuple.IsEmpty)
        {
            space.State.Remove(storedTuple);

            await space.WriteStateAsync();
            await storeStream.OnNextAsync(action);

            if (space.State.Count == 0)
            {
                await TerminateAsync();
            }
        }
    }

    public async Task RemoveAll(Guid agentId)
    {
        space.State.Clear();

        await space.WriteStateAsync();
        await storeStream.OnNextAsync(new(agentId, new(), TupleActionType.Clear));

        await TerminateAsync();
    }

    private async Task TerminateAsync()
    {
        await interceptorStream.OnNextAsync(this.GetPrimaryKeyString());
        DeactivateOnIdle();
    }
}