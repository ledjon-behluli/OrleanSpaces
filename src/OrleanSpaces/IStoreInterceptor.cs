using OrleanSpaces.Tuples;
using System.Collections.Immutable;

namespace OrleanSpaces;

internal interface IStoreInterceptor<T>
     where T : ISpaceTuple
{
    Task<ImmutableArray<T>> GetAll();

    Task<Guid> Insert(TupleAction<T> action);
    Task Remove(TupleAction<T> action);
    Task RemoveAll(Guid agentId);

    Task OnNewStoreCreated(Guid storeId);
}