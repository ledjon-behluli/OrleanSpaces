using OrleanSpaces.Tuples;

namespace OrleanSpaces;

internal interface ITupleActionReceiver<T> where T : ISpaceTuple
{
    void Add(TupleAction<T> action);
    void Remove(TupleAction<T> action);
    void Clear(TupleAction<T> action);
}
