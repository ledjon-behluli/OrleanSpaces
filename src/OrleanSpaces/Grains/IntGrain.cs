using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Tuples.Typed;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Grains;

internal interface IIntGrain : IGrainWithGuidKey
{
    ValueTask<ImmutableArray<IntTuple>> GetAsync();
    Task AddAsync(TupleAction<IntTuple> action);
    Task RemoveAsync(TupleAction<IntTuple> action);
}

internal sealed class IntGrain : Grain, IIntGrain
{
    private readonly IPersistentState<IntGrainState> space;

    [AllowNull] private IAsyncStream<TupleAction<IntTuple>> stream;

    public IntGrain(
        [PersistentState(Constants.IntGrainStorage, Constants.TupleSpacesStore)] 
        IPersistentState<IntGrainState> space)
    {
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public override Task OnActivateAsync()
    {
        var provider = GetStreamProvider(Constants.PubSubProvider);
        stream = provider.GetStream<TupleAction<IntTuple>>(this.GetPrimaryKey(), Constants.IntStream);

        return base.OnActivateAsync();
    }

    public ValueTask<ImmutableArray<IntTuple>> GetAsync()
      => new(space.State.Tuples.Select(x => (IntTuple)x).ToImmutableArray());

    public async Task AddAsync(TupleAction<IntTuple> action)
    {
        ThrowHelpers.EmptyTuple(action.Tuple);

        space.State.Tuples.Add(action.Tuple);

        await space.WriteStateAsync();
        await stream.OnNextAsync(action);
    }

    public async Task RemoveAsync(TupleAction<IntTuple> action)
    {
        var storedTuple = space.State.Tuples.FirstOrDefault(x => x == action.Tuple);
        if (storedTuple != null)
        {
            space.State.Tuples.Remove(storedTuple);

            await space.WriteStateAsync();
            await stream.OnNextAsync(action);
        }
    }
}

internal sealed class IntGrainState
{
    public List<Tuple> Tuples { get; set; } = new();

    public class Tuple
    {
        public List<int> Fields { get; set; } = new();

        public static implicit operator Tuple(IntTuple tuple)
        {
            Tuple dto = new();

            for (int i = 0; i < tuple.Length; i++)
            {
                dto.Fields.Add(tuple[i]);
            }

            return dto;
        }

        public static implicit operator IntTuple(Tuple tuple) =>
            new(tuple.Fields.ToArray());
    }
}