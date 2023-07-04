using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Tuples;
using OrleanSpaces.Helpers;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Grains;

internal interface ISpaceGrain : IBaseGrain<SpaceTuple>, IGrainWithStringKey 
{
    const string Key = "SpaceStore";
}

internal sealed class SpaceGrain : Grain, ISpaceGrain
{
    private readonly IPersistentState<List<SpaceTuple>> space;
    private readonly StreamId streamId = StreamId.Create(Constants.StreamName, ISpaceGrain.Key);

    [AllowNull] private IAsyncStream<TupleAction<SpaceTuple>> stream;

    public SpaceGrain(
        [PersistentState(ISpaceGrain.Key, Constants.StorageName)]
        IPersistentState<List<SpaceTuple>> space)
    {
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        stream = this.GetStream<SpaceTuple>(streamId);
        return Task.CompletedTask;
    }

    public ValueTask<StreamId> GetStreamId() => new(streamId);

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