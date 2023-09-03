using Orleans.Concurrency;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;

namespace OrleanSpaces;

internal interface ITupleStore<T> where T : ISpaceTuple
{
    [ReadOnly] Task<ImmutableArray<T>> GetAll();
    Task<bool> Insert(TupleAction<T> action);
    Task<int> Remove(TupleAction<T> action);
    Task RemoveAll(Guid agentId);
}