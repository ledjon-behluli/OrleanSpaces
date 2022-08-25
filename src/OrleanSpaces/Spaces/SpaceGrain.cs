using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Primitives;
using OrleanSpaces.Utils;

namespace OrleanSpaces.Spaces;

[Serializable]
internal class SpaceState
{
    public List<SpaceTuple> Tuples { get; set; } = new();
}

internal class SpaceGrain : Grain, ISpaceGrain
{
    private readonly IPersistentState<SpaceState> space;

    private IAsyncStream<SpaceTuple> stream;

#nullable disable
    public SpaceGrain([PersistentState("tupleSpace", Constants.StorageProviderName)] IPersistentState<SpaceState> space)
#nullable enable
    {
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public override Task OnActivateAsync()
    {
        var provider = GetStreamProvider(Constants.StreamProviderName);
        stream = provider.GetStream<SpaceTuple>(this.GetPrimaryKey(), Constants.StreamNamespace);

        return base.OnActivateAsync();
    }

    public ValueTask<Guid> ConnectAsync() => new(stream.Guid);

    public async Task WriteAsync(SpaceTuple tuple)
    {
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

    public ValueTask<int> CountAsync() => new(space.State.Tuples.Count);

    public ValueTask<int> CountAsync(SpaceTemplate template) =>
        new(space.State.Tuples.Count(sp => sp.Length == template.Length && TupleMatcher.IsMatch(sp, template)));
}
