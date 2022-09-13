using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Primitives;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace OrleanSpaces;

internal interface ISpaceGrain : IGrainWithGuidKey
{
    ValueTask<Guid> ListenAsync();

    Task WriteAsync(SpaceTuple tuple);
    ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template);
    Task<SpaceTuple> PopAsync(SpaceTemplate template);
    ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template);
    ValueTask<int> CountAsync(SpaceTemplate? template);
}

internal sealed class SpaceGrain : Grain, ISpaceGrain
{
    private readonly IPersistentState<SpaceState> space;

    [AllowNull] private IAsyncStream<ITuple> stream;

    public SpaceGrain([PersistentState("TupleSpace", StorageNames.TupleSpaceStore)] IPersistentState<SpaceState> space)
    {
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public override Task OnActivateAsync()
    {
        var provider = GetStreamProvider(StreamNames.PubSubProvider);
        stream = provider.GetStream<ITuple>(this.GetPrimaryKey(), StreamNamespaces.Tuple);

        return base.OnActivateAsync();
    }

    public ValueTask<Guid> ListenAsync() => new(stream.Guid);

    public async Task WriteAsync(SpaceTuple tuple)
    {
        if (tuple.IsUnit)
        {
            throw new ArgumentException("Unit tuples are not allowed to be writen in the tuple space.");
        }

        space.State.Tuples.Add(tuple);

        await space.WriteStateAsync();
        await stream.OnNextAsync(tuple);
    }

    public ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template)
    {
        IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

        foreach (var tuple in tuples)
        {
            if (template.IsSatisfiedBy(tuple))
            {
                return new(tuple);
            }
        }

        return new(SpaceTuple.Unit);
    }

    public async Task<SpaceTuple> PopAsync(SpaceTemplate template)
    {
        IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

        foreach (var tuple in tuples)
        {
            if (template.IsSatisfiedBy(tuple))
            {
                space.State.Tuples.Remove(tuple);
                await space.WriteStateAsync();

                if (space.State.Tuples.Count == 0)
                {
                    await stream.OnNextAsync(SpaceUnit.Null);
                }

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
            if (template.IsSatisfiedBy(tuple))
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

        return new(space.State.Tuples.Count(tuple => ((SpaceTemplate)template).IsSatisfiedBy(tuple)));
    }

    [Serializable]
    internal sealed class SpaceState
    {
        public List<SpaceTuple> Tuples { get; set; } = new();
    }
}
