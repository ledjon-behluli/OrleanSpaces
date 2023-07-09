using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Helpers;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Grains;

internal abstract class Grain<T> : Grain
    where T : struct, ISpaceTuple, IEquatable<T>
{
    private readonly IPersistentState<HashSet<T>> space;
    private readonly StreamId streamId;

    [AllowNull] private IAsyncStream<TupleAction<T>> stream;

    public Grain(string key, IPersistentState<HashSet<T>> space)
    {
        streamId = StreamId.Create(Constants.StreamName, key ?? throw new ArgumentNullException(nameof(key)));
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        stream = this.GetStream<T>(streamId);
        return Task.CompletedTask;
    }

    public ValueTask<StreamId> GetStreamId() => new(streamId);
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

    public async Task RemoveAll()
    {
        space.State.Clear();
        await space.WriteStateAsync();
    }
}