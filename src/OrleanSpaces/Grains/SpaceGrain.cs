using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Grains;

internal interface ISpaceGrain : IGrainWithGuidKey
{
    ValueTask<ImmutableArray<SpaceTuple>> GetAsync();
    Task AddAsync(SpaceTuple tuple);
    Task RemoveAsync(SpaceTuple tuple);
}

internal sealed class SpaceGrain : Grain, ISpaceGrain
{
    private readonly IPersistentState<SpaceGrainState> space;

    [AllowNull] private IAsyncStream<TupleAction<SpaceTuple>> stream;

    public SpaceGrain(
        [PersistentState(Constants.SpaceGrainStorage, Constants.TupleSpacesStore)]
        IPersistentState<SpaceGrainState> space)
    {
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public override Task OnActivateAsync()
    {
        var provider = GetStreamProvider(Constants.PubSubProvider);
        stream = provider.GetStream<TupleAction<SpaceTuple>>(this.GetPrimaryKey(), Constants.SpaceStream);

        return base.OnActivateAsync();
    }

    public ValueTask<ImmutableArray<SpaceTuple>> GetAsync()
      => new(space.State.Tuples.Select(x => (SpaceTuple)x).ToImmutableArray());

    public async Task AddAsync(SpaceTuple tuple)
    {
        ThrowHelpers.EmptyTuple(tuple);

        space.State.Tuples.Add(tuple);

        await space.WriteStateAsync();
        await stream.OnNextAsync(new(tuple, TupleActionType.Added));
    }

    public async Task RemoveAsync(SpaceTuple tuple)
    {
        var storedTuple = space.State.Tuples.FirstOrDefault(x => x == tuple);
        if (storedTuple != null)
        {
            space.State.Tuples.Remove(storedTuple);

            await space.WriteStateAsync();
            await stream.OnNextAsync(new(tuple, TupleActionType.Removed));
        }
    }
}

internal sealed class SpaceGrainState
{
    public List<Tuple> Tuples { get; set; } = new();

    public class Tuple
    {
        public List<object> Fields { get; set; } = new();

        public static implicit operator Tuple(SpaceTuple tuple)
        {
            Tuple dto = new();

            for (int i = 0; i < tuple.Length; i++)
            {
                dto.Fields.Add(tuple[i]);
            }

            return dto;
        }

        public static implicit operator SpaceTuple(Tuple tuple) =>
            new(tuple.Fields.ToArray());
    }
}