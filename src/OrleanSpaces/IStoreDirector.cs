using Orleans.Concurrency;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;

namespace OrleanSpaces;

internal interface IStoreDirector<T>
     where T : ISpaceTuple
{
    [ReadOnly] Task<ImmutableArray<TupleAddress<T>>> GetAll();

    Task<Guid> Insert(TupleAction<T> action);
    Task Remove(TupleAction<T> action);

   // [Transaction(TransactionOption.Create)] 
    Task RemoveAll(Guid agentId);
}