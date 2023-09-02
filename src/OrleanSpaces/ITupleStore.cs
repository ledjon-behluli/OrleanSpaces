using OrleanSpaces.Tuples;
using System.Collections.Immutable;

namespace OrleanSpaces;

internal interface ITupleStore<T> where T : ISpaceTuple
{
    Task<ImmutableArray<T>> GetAll();
    /// <returns>Wether the insert operation succeeded or not.</returns>
    Task<bool> Insert(TupleAction<T> action);
    /// <returns>The number of tuples left.</returns>
    Task<int> Remove(TupleAction<T> action);
    Task RemoveAll(Guid agentId);
}