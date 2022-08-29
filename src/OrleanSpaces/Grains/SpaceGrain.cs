using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Primitives;
using OrleanSpaces.Utils;

namespace OrleanSpaces.Grains;

internal class SpaceGrain : Grain, ISpaceGrain
{
    private readonly IPersistentState<SpaceState> space;

    private IAsyncStream<SpaceTuple> stream;

#nullable disable
    public SpaceGrain([PersistentState("TupleSpace", StorageNames.TupleSpaceStore)] IPersistentState<SpaceState> space)
#nullable enable
    {
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public override Task OnActivateAsync()
    {
        var provider = GetStreamProvider(StreamNames.PubSubProvider);
        stream = provider.GetStream<SpaceTuple>(this.GetPrimaryKey(), StreamNamespaces.TupleWrite);

        return base.OnActivateAsync();
    }

    public ValueTask<Guid> ConnectAsync() => new(stream.Guid);

    public async Task WriteAsync(SpaceTuple tuple)
    {
        if (tuple.IsEmpty)
        {
            throw new ArgumentException("Empty tuples are not allowed to be placed in the space.");
        }

        space.State.Tuples.Add(tuple);

        await space.WriteStateAsync();
        await stream.OnNextAsync(tuple);
    }

    public async Task EvaluateAsync(byte[] serializedFunc)
    {
        Func<SpaceTuple> function = LambdaSerializer.Deserialize(serializedFunc);
        object result = function.DynamicInvoke();

        if (result is SpaceTuple tuple)
        {
            await WriteAsync(tuple);
        }
    }

    public ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template)
    {
        IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

        foreach (var tuple in tuples)
        {
            if (TupleMatcher.IsMatch(tuple, template))
            {
                return new(tuple);
            }
        }

        return new(new SpaceTuple());
    }

    public async Task<SpaceTuple> PopAsync(SpaceTemplate template)
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

        return new();
    }

    public ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template)
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

    public ValueTask<int> CountAsync(SpaceTemplate? template)
    {
        if (template == null)
        {
            return new(space.State.Tuples.Count);
        }

        return new(space.State.Tuples.Count(sp =>
            sp.Length == ((SpaceTemplate)template).Length && TupleMatcher.IsMatch(sp, (SpaceTemplate)template)));
    }
}
