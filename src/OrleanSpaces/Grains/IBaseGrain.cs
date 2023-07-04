using Orleans.Runtime;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;

namespace OrleanSpaces.Grains;

internal interface IBaseGrain<T> where T : ISpaceTuple
{
    ValueTask<StreamId> GetStreamId();
    ValueTask<ImmutableArray<T>> GetAll();
    Task Insert(TupleAction<T> action);
    Task Remove(TupleAction<T> action);
}
