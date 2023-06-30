using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Helpers;
using OrleanSpaces.Tuples.Typed;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Grains;

internal interface IIntGrain : IGrainWithStringKey 
{
    const string Name = "IntGrain";

    ValueTask<ImmutableArray<IntTuple>> GetAsync();
    Task AddAsync(TupleAction<IntTuple> action);
    Task RemoveAsync(TupleAction<IntTuple> action);
}

internal sealed class IntGrain : Grain, IIntGrain
{
    private readonly IPersistentState<List<IntTuple>> space;
    [AllowNull] private IAsyncStream<TupleAction<IntTuple>> stream;

    public IntGrain(
        [PersistentState(IIntGrain.Name, Constants.StorageName)] 
        IPersistentState<List<IntTuple>> space)
    {
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        stream = this.GetStream<IntTuple>(StreamId.Create(Constants.StreamName, IIntGrain.Name));
        return Task.CompletedTask;
    }

    public ValueTask<ImmutableArray<IntTuple>> GetAsync()
      => new(space.State.ToImmutableArray());

    public async Task AddAsync(TupleAction<IntTuple> action)
    {
        space.State.Add(action.Tuple);

        await space.WriteStateAsync();
        await stream.OnNextAsync(action);
    }

    public async Task RemoveAsync(TupleAction<IntTuple> action)
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