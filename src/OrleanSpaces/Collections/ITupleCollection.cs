using OrleanSpaces.Tuples;

namespace OrleanSpaces.Collections;

internal interface ITupleCollection<TTuple, TTemplate> : IEnumerable<TTuple>
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    int Count { get; }

    void Add(TTuple tuple);
    void Remove(TTuple tuple);
    void Clear();
    TTuple Find(TTemplate template);
    IEnumerable<TTuple> FindAll(TTemplate template);
}