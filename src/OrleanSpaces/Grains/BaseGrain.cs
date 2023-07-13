using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Grains;

internal abstract class BaseGrain<T> : Grain
    where T : struct, ISpaceTuple, IEquatable<T>
{
    private readonly IPersistentState<List<T>> space;
    private readonly StreamId streamId;

    [AllowNull] private IAsyncStream<TupleAction<T>> stream;

    public BaseGrain(string key, IPersistentState<List<T>> space)
    {
        streamId = StreamId.Create(Constants.StreamName, key ?? throw new ArgumentNullException(nameof(key)));
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        stream = this.GetStreamProvider(Constants.PubSubProvider).GetStream<TupleAction<T>>(streamId);
        return Task.CompletedTask;
    }

    public ValueTask<ImmutableArray<T>> GetAll() => new(space.State.ToImmutableArray());

    public async Task Insert(TupleAction<T> action)
    {
        space.State.Add(action.Tuple);

        await space.WriteStateAsync();
        await stream.OnNextAsync(action);
    }

    public async Task Remove(TupleAction<T> action)
    {
        var storedTuple = space.State.FirstOrDefault(x => x.Equals(action.Tuple));
        if (storedTuple.Length > 0)
        {
            space.State.Remove(storedTuple);

            await space.WriteStateAsync();
            await stream.OnNextAsync(action);
        }
    }

    public async Task RemoveAll(Guid agentId)
    {
        space.State.Clear();

        await space.WriteStateAsync();
        await stream.OnNextAsync(new(agentId, new(), TupleActionType.Clear));
    }
}