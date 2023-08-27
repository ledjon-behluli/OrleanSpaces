using OrleanSpaces.Tuples;

namespace OrleanSpaces.Collections;

internal interface ITupleCollection : IEnumerable<SpaceTuple>
{
    int Count { get; }
    void Add(SpaceTuple tuple);
    void Remove(SpaceTuple tuple);
    void Clear();
    SpaceTuple Find(SpaceTemplate template);
    IEnumerable<SpaceTuple> FindAll(SpaceTemplate template);

}

internal interface ITupleCollection<T, TTuple, TTemplate> : IEnumerable<TTuple>
    where T : unmanaged
    where TTuple : struct, ISpaceTuple<T>
    where TTemplate : struct, ISpaceTemplate<T>, ISpaceMatchable<T, TTuple>
{
    int Count { get; }
    void Add(TTuple tuple);
    void Remove(TTuple tuple);
    void Clear();
    TTuple Find(TTemplate template);
    IEnumerable<TTuple> FindAll(TTemplate template);
}
