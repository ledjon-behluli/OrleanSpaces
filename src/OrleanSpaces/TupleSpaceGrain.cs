using Orleans;
using Orleans.Runtime;

namespace OrleanSpaces;

internal sealed class TupleSpaceGrain : Grain, ITupleSpace
{
    private readonly IPersistentState<TupleSpaceState> space;

    public TupleSpaceGrain([PersistentState("tupleSpace", "tupleSpaceStore")] IPersistentState<TupleSpaceState> space)
    {
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public async Task Write(SpaceTuple tuple)
    {
        space.State.Tuples.Add(tuple);
        await space.WriteStateAsync();
    }

    public ValueTask<SpaceTuple> Peek(SpaceTemplate template)
    {
        throw new System.NotImplementedException();
    }

    public ValueTask<SpaceResult> TryPeek(SpaceTemplate template)
    {
        IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

        foreach (var tuple in tuples)
        {
            if (TupleMatcher.IsMatch(tuple, template))
            {
                return new ValueTask<SpaceResult>(new SpaceResult(tuple));
            }
        }

        return new(SpaceResult.Empty);
    }

    public Task<SpaceTuple> Extract(SpaceTemplate template)
    {
        throw new System.NotImplementedException();
    }

    public async Task<SpaceResult> TryExtract(SpaceTemplate template)
    {
        IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

        foreach (var tuple in tuples)
        {
            if (TupleMatcher.IsMatch(tuple, template))
            {
                space.State.Tuples.Remove(tuple);
                await space.WriteStateAsync();

                return new(tuple);
            }
        }

        return SpaceResult.Empty;
    }

    public IEnumerable<SpaceTuple> Scan(SpaceTemplate template = default)
    {
        IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

        foreach (var tuple in tuples)
        {
            if (TupleMatcher.IsMatch(tuple, template))
            {
                yield return tuple;
            }
        }
    }

    public ValueTask<int> Count() => new(space.State.Tuples.Count);

    public ValueTask<int> Count(SpaceTemplate template) =>
        new(space.State.Tuples.Count(sp => sp.Length == template.Length && TupleMatcher.IsMatch(sp, template)));
}
