using OrleanSpaces.Tuples;
using System.Collections.Immutable;

namespace OrleanSpaces;

internal interface ITupleStore<T> where T : ISpaceTuple
{
    ValueTask<ImmutableArray<T>> GetAll();
    Task Insert(TupleAction<T> action);
    Task Remove(TupleAction<T> action);
    Task RemoveAll(Guid agentId);
}
