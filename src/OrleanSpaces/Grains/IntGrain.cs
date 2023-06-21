using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Tuples.Typed;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Grains;

internal interface IIntGrain : IGrainWithGuidKey
{
    Task AddAsync(IntTuple tuple);
    Task RemoveAsync(IntTuple tuple);
    ValueTask<List<IntTuple>> GetAsync(IntTemplate template);
}

internal sealed class IntGrain : Grain, IIntGrain
{
    private readonly IPersistentState<IntGrainState> space;

    [AllowNull] private IAsyncStream<TupleAction<IntTuple>> stream;

    public IntGrain([PersistentState("IntGrain", "IntGrainStore")] IPersistentState<IntGrainState> space)
    {
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public override Task OnActivateAsync()
    {
        var provider = GetStreamProvider(Constants.PubSubProvider);
        stream = provider.GetStream<TupleAction<IntTuple>>(this.GetPrimaryKey(), "IntStream");

        return base.OnActivateAsync();
    }

    public async Task AddAsync(IntTuple tuple)
    {
        if (tuple == IntTuple.Empty) ThrowHelpers.EmptyTuple();

        space.State.Tuples.Add(tuple);

        await space.WriteStateAsync();
        await stream.OnNextAsync(new(tuple, TupleActionType.Added));
    }

    public async Task RemoveAsync(IntTuple tuple)
    {
        var storedTuple = space.State.Tuples.FirstOrDefault(x => x.Equals(tuple));
        if (storedTuple != null)
        {
            space.State.Tuples.Remove(storedTuple);

            await space.WriteStateAsync();
            await stream.OnNextAsync(new(tuple, TupleActionType.Removed));
        }
    }

    public ValueTask<List<IntTuple>> GetAsync(IntTemplate template)
    {
        List<IntTuple> results = new();

        IEnumerable<IntGrainState.Tuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

        foreach (var tuple in tuples)
        {
            if (template.Matches((IntTuple)tuple))
            {
                results.Add(tuple);
            }
        }

        return new(results);
    }
}

internal sealed class IntGrainState
{
    public List<Tuple> Tuples { get; set; } = new();

    public class Tuple
    {
        public List<int> Fields { get; set; } = new();
        public int Length => Fields.Count;

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