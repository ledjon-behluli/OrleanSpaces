using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Tuples;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace OrleanSpaces;

internal sealed class SpaceGrain : Grain, ISpaceGrain
{
    private readonly IPersistentState<TupleSpaceState> space;

    [AllowNull] private IAsyncStream<ITuple> stream;

    public SpaceGrain([PersistentState(Constants.TupleSpaceState, Constants.TupleSpaceStore)] IPersistentState<TupleSpaceState> space)
    {
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public override Task OnActivateAsync()
    {
        var provider = GetStreamProvider(Constants.PubSubProvider);
        stream = provider.GetStream<ITuple>(this.GetPrimaryKey(), Constants.TupleStream);

        return base.OnActivateAsync();
    }

    public ValueTask<Guid> ListenAsync() => new(stream.Guid);

    public async Task WriteAsync(SpaceTuple tuple)
    {
        if (tuple.IsNull)
        {
            throw new ArgumentException("Null tuple is not allowed to be writen in the tuple space.");
        }

        space.State.Tuples.Add(tuple);

        await space.WriteStateAsync();
        await stream.OnNextAsync(tuple);
    }

    public ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template)
    {
        IEnumerable<SpaceTuple> tuples = space.State.Tuples
            .Where(x => x.Length == template.Length)
            .Select(x => (SpaceTuple)x);

        foreach (var tuple in tuples)
        {
            if (template.Matches(tuple))
            {
                return new(tuple);
            }
        }

        return new(SpaceTuple.Null);
    }

    public async ValueTask<SpaceTuple> PopAsync(SpaceTemplate template)
    {
        var tupleDtos = space.State.Tuples.Where(x => x.Length == template.Length);

        foreach (var dto in tupleDtos)
        {
            SpaceTuple tuple = dto;

            if (template.Matches(tuple))
            {
                space.State.Tuples.Remove(dto);

                await space.WriteStateAsync();
                await stream.OnNextAsync(template);

                if (space.State.Tuples.Count == 0)
                {
                    await stream.OnNextAsync(new SpaceUnit());
                }

                return tuple;
            }
        }

        return SpaceTuple.Null;
    }

    public ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template)
    {
        List<SpaceTuple> results = new();

        IEnumerable<SpaceTuple> tuples = space.State.Tuples
            .Where(x => x.Length == template.Length)
            .Select(x => (SpaceTuple)x);

        foreach (var tuple in tuples)
        {
            if (template.Matches(tuple))
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

        return new(space.State.Tuples.Count(tuple => ((SpaceTemplate)template).Matches(tuple)));
    }
}

internal sealed class TupleSpaceState
{
    public List<SpaceTupleState> Tuples { get; set; } = new();

    public class SpaceTupleState
    {
        public List<object> Fields { get; set; } = new();
        public int Length => Fields.Count;

        public static implicit operator SpaceTupleState(SpaceTuple tuple)
        {
            SpaceTupleState dto = new();

            for (int i = 0; i < tuple.Length; i++)
            {
                dto.Fields.Add(tuple[i]);
            }

            return dto;
        }

        public static implicit operator SpaceTuple(SpaceTupleState dto) =>
            new(dto.Fields.ToArray());
    }
}