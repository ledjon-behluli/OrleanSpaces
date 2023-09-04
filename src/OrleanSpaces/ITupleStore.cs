using Orleans.Concurrency;
using OrleanSpaces.Tuples;

namespace OrleanSpaces;

internal interface ITupleStore<T> where T : ISpaceTuple
{
    [ReadOnly] Task<StoreContent<T>> GetAll();
    Task<bool> Insert(T tuple);
    Task<int> Remove(T tuple);
    Task RemoveAll();
}