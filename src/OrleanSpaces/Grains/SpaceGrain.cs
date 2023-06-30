using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Tuples;
using OrleanSpaces.Helpers;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Grains;

internal interface ISpaceGrain : IGrainWithStringKey
{
    const string Id = "SpaceGrain";

    ValueTask<ImmutableArray<SpaceTuple>> GetAsync();
    Task AddAsync(TupleAction<SpaceTuple> action);
    Task RemoveAsync(TupleAction<SpaceTuple> action);
}

internal sealed class SpaceGrain : Grain, ISpaceGrain
{
    private readonly IPersistentState<List<SpaceTuple>> space;
    [AllowNull] private IAsyncStream<TupleAction<SpaceTuple>> stream;

    public SpaceGrain(
        [PersistentState(ISpaceGrain.Id, Constants.StorageName)]
        IPersistentState<List<SpaceTuple>> space)
    {
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        stream = this.GetStream<SpaceTuple>(StreamId.Create(Constants.StreamName, ISpaceGrain.Id));
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