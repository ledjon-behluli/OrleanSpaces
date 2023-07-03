using OrleanSpaces.Tuples;
using System.Collections.Immutable;

namespace OrleanSpaces.Grains;

internal interface IBaseGrain<T> where T : ISpaceTuple
{
    static abstract string Id { get; }

    ValueTask<ImmutableArray<T>> GetAsync();
    Task AddAsync(TupleAction<T> action);
    Task RemoveAsync(TupleAction<T> action);
}
