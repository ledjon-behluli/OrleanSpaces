using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Helpers;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Grains;

internal abstract class Grain<T, TTuple, TSelf> : Grain
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
    where TSelf : ITupleStore<TTuple>
{
    private readonly IPersistentState<List<TTuple>> space;
    private readonly StreamId streamId;

    [AllowNull] private IAsyncStream<TupleAction<TTuple>> stream;

    public Grain(string key, IPersistentState<List<TTuple>> space)
    {
        streamId = StreamId.Create(Constants.StreamName, key ?? throw new ArgumentNullException(nameof(key)));
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        stream = this.GetStream<TTuple>(streamId);
        return Task.CompletedTask;
    }

    public ValueTask<StreamId> GetStreamId() => new(streamId);

    public ValueTask<ImmutableArray<TTuple>> GetAll()
      => new(space.State.ToImmutableArray());

    public async Task Insert(TupleAction<TTuple> action)
    {
        space.State.Add(action.Tuple);

        await space.WriteStateAsync();
        await stream.OnNextAsync(action);
    }

    public async Task Remove(TupleAction<TTuple> action)
    {
        var storedTuple = space.State.FirstOrDefault(x => x == action.Tuple);
        if (storedTuple.Length > 0)
        {
            space.State.Remove(storedTuple);

            await space.WriteStateAsync();
            await stream.OnNextAsync(action);
        }
    }
}
