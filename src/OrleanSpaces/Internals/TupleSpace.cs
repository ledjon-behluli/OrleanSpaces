using Orleans;
using Orleans.Runtime;
using OrleanSpaces.Internals.Functions;
using OrleanSpaces.Types;

namespace OrleanSpaces.Internals;

internal partial class TupleSpace : Grain, ISpaceProvider
{
    private readonly TupleFunctionSerializer serializer;
    private readonly IPersistentState<TupleSpaceState> space;

    public TupleSpace(
        TupleFunctionSerializer serializer,
        [PersistentState("tupleSpace", "tupleSpaceStore")] IPersistentState<TupleSpaceState> space)
    {
        this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public async Task WriteAsync(SpaceTuple tuple)
    {
        space.State.Tuples.Add(tuple);
        await space.WriteStateAsync();
    }

    public Task EvaluateAsync(Func<SpaceTuple> func) => Task.CompletedTask;

    async Task ISpaceProvider.EvaluateAsync(byte[] serializedFunc)
    {
        Func<SpaceTuple>? function = serializer.Deserialize(serializedFunc);
        if (function != null)
        {
            object result = function.DynamicInvoke();
            if (result is SpaceTuple tuple)
            {
                await WriteAsync(tuple);
            }
        }
    }

    public ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template)
    {
        throw new NotImplementedException();
    }

    public ValueTask<SpaceTuple?> TryPeekAsync(SpaceTemplate template)
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

    public Task<SpaceTuple> ExtractAsync(SpaceTemplate template)
    {
        throw new NotImplementedException();
    }

    public async Task<SpaceTuple?> TryExtractAsync(SpaceTemplate template)
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
