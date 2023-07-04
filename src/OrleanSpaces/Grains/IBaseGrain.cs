using Orleans.Runtime;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;

namespace OrleanSpaces.Grains;

internal interface IBaseGrain<T> where T : ISpaceTuple
{
    ValueTask<StreamId> GetStreamId();

    ValueTask<ImmutableArray<T>> GetAsync();
    Task AddAsync(TupleAction<T> action);
    Task RemoveAsync(TupleAction<T> action);
}
