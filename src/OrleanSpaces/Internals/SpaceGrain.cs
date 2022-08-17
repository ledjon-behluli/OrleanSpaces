using Orleans;
using Orleans.Runtime;
using OrleanSpaces.Types;

namespace OrleanSpaces.Internals;

internal class SpaceGrain : Grain, IGrainWithGuidKey,
    ISpaceProvider, ISyncSpaceProvider, ITupleFunctionExecutor
{
    private readonly TupleFunctionSerializer serializer;
    private readonly IPersistentState<SpaceState> space;

    public SpaceGrain(
        TupleFunctionSerializer serializer,
        [PersistentState("tupleSpace", "tupleSpaceStore")] IPersistentState<SpaceState> space)
    {
        this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public async Task Write(SpaceTuple tuple)
    {
        space.State.Tuples.Add(tuple);
        await space.WriteStateAsync();
    }

    public Task Evaluate(TupleFunction @delegate) => Task.CompletedTask;

    public async Task Execute(byte[] serializedFunction)
    {
        TupleFunction? function = serializer.Deserialize(serializedFunction);
        if (function != null)
        {
            object result = function.DynamicInvoke(this);
            if (result is SpaceTuple tuple)
            {
                await Write(tuple);
            }
        }
    }

    public ValueTask<SpaceTuple> Peek(SpaceTemplate template)
    {
        throw new NotImplementedException();
    }

    TupleResult ISyncSpaceProvider.TryPeek(SpaceTemplate template)
        => TryPeekInternal(template);

    public ValueTask<TupleResult> TryPeek(SpaceTemplate template)
        => new(TryPeekInternal(template));

    private TupleResult TryPeekInternal(SpaceTemplate template)
    {
        IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

        foreach (var tuple in tuples)
        {
            if (TupleMatcher.IsMatch(tuple, template))
            {
                return new TupleResult(tuple);
            }
        }

        return TupleResult.Empty;
    }

    public Task<SpaceTuple> Extract(SpaceTemplate template)
    {
        throw new NotImplementedException();
    }

    public async Task<TupleResult> TryExtract(SpaceTemplate template)
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

        return TupleResult.Empty;
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
