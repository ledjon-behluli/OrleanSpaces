using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Grains;

internal interface IBaseGrain<T> where T : ISpaceTuple
{
    ValueTask<ImmutableArray<T>> GetAsync();
    Task AddAsync(TupleAction<T> action);
    Task RemoveAsync(TupleAction<T> action);
}

internal interface ISpaceGrain : IBaseGrain<SpaceTuple>, IGrainWithStringKey
{
    const string Name = "SpaceGrain";
    static StreamId GetStreamId() => StreamId.Create(Constants.StreamName, Name);
}

//internal interface ISpaceGrain : IGrainWithStringKey
//{
//    const string Name = "SpaceGrain";
//    static StreamId GetStreamId() => StreamId.Create(Constants.StreamName, Name);

//    ValueTask<ImmutableArray<SpaceTuple>> GetAsync();
//    Task AddAsync(TupleAction<SpaceTuple> action);
//    Task RemoveAsync(TupleAction<SpaceTuple> action);
//}

internal sealed class SpaceGrain : Grain, ISpaceGrain
{
    private readonly IPersistentState<List<SpaceTuple>> space;

    [AllowNull] private IAsyncStream<TupleAction<SpaceTuple>> stream;

    public SpaceGrain(
        [PersistentState(ISpaceGrain.Name, Constants.StorageName)]
        IPersistentState<List<SpaceTuple>> space)
    {
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        var provider = this.GetStreamProvider(Constants.PubSubProvider);
        stream = provider.GetStream<TupleAction<SpaceTuple>>(ISpaceGrain.GetStreamId());

        return Task.CompletedTask;
    }

    public ValueTask<ImmutableArray<SpaceTuple>> GetAsync()
      => new(space.State.ToImmutableArray());

    public async Task AddAsync(TupleAction<SpaceTuple> action)
    {
        space.State.Add(action.Tuple);

        await space.WriteStateAsync();
        await stream.OnNextAsync(action);
    }

    public async Task RemoveAsync(TupleAction<SpaceTuple> action)
    {
        var storedTuple = space.State.FirstOrDefault(x => x == action.Tuple);
        if (storedTuple.Length > 0)
        {
            space.State.Remove(storedTuple);

            await space.WriteStateAsync();
            await stream.OnNextAsync(action);
        }
    }
}

internal sealed class SpaceGrainState
{
    public List<Tuple> Tuples { get; set; } = new();

    public class Tuple
    {
        public List<object> Fields { get; set; } = new();

        public static implicit operator Tuple(SpaceTuple tuple)
        {
            Tuple dto = new();

            for (int i = 0; i < tuple.Length; i++)
            {
                dto.Fields.Add(tuple[i]);
            }

            return dto;
        }

        public static implicit operator SpaceTuple(Tuple tuple) =>
            new(tuple.Fields.ToArray());
    }
}