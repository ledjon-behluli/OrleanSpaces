using Orleans;
using Orleans.Runtime;
using OrleanSpaces.Core;
using OrleanSpaces.Core.Primitives;
using OrleanSpaces.Core.Utils;

namespace OrleanSpaces.Hosts.Grains;

internal class SpaceGrain : Grain, ISpaceGrain
{
    private readonly FuncSerializer serializer;
    private readonly IPersistentState<SpaceState> space;

    public SpaceGrain(
        FuncSerializer serializer,
        [PersistentState("tupleSpace", "tupleSpaceStore")] IPersistentState<SpaceState> space)
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

    async Task ISpaceWriter.EvaluateAsync(byte[] serializedFunc)
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

    ValueTask ISpaceBlockingReader.PeekAsync(SpaceTemplate template, TuplePromise promise)
    {
        throw new NotImplementedException();
    }

    ValueTask<SpaceTuple?> ISpaceReader.PeekAsync(SpaceTemplate template)
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

    Task ISpaceBlockingReader.PopAsync(SpaceTemplate template, TuplePromise promise)
    {
        throw new NotImplementedException();
    }

    async Task<SpaceTuple?> ISpaceReader.PopAsync(SpaceTemplate template)
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
