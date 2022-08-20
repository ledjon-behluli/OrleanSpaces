using Orleans;
using Orleans.Runtime;
using OrleanSpaces.Core;
using OrleanSpaces.Core.Internals;

namespace OrleanSpaces.Hosts.Internals;

internal class SpaceGrain : Grain, ISpaceGrain
{
    private readonly IPersistentState<SpaceState> space;

    public SpaceGrain([PersistentState("tupleSpace", "tupleSpaceStore")] IPersistentState<SpaceState> space)
    {
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public async Task WriteAsync(SpaceTuple tuple)
    {
        space.State.Tuples.Add(tuple);
        await space.WriteStateAsync();
    }

    public Task EvaluateAsync(byte[] serializedFunc) => Task.CompletedTask;

    public ValueTask<SpaceTuple?> PeekAsync(SpaceTemplate template)
    {
        IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

        foreach (var tuple in tuples)
        {
            if (TupleMatcher.IsMatch(tuple, template))
            {
                return new(tuple);
            }
        }

        return default;
    }

    public async Task<SpaceTuple?> PopAsync(SpaceTemplate template)
    {
        IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

        foreach (var tuple in tuples)
        {
            if (TupleMatcher.IsMatch(tuple, template))
            {
                space.State.Tuples.Remove(tuple);
                await space.WriteStateAsync();

                return tuple;
            }
        }

        return default;
    }

    public ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template = default)
    {
        List<SpaceTuple> results = new();
        IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

        foreach (var tuple in tuples)
        {
            if (TupleMatcher.IsMatch(tuple, template))
            {
                results.Add(tuple);
            }
        }

        return new(results);
    }

    public ValueTask<int> CountAsync() => new(space.State.Tuples.Count);

    public ValueTask<int> CountAsync(SpaceTemplate template) =>
        new(space.State.Tuples.Count(sp => sp.Length == template.Length && TupleMatcher.IsMatch(sp, template)));
}
