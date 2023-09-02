using OrleanSpaces.Tuples;

namespace OrleanSpaces;

[GenerateSerializer, Immutable]
internal readonly record struct TupleAction<T>(Guid AgentId, TupleAddressPair<T> Pair, TupleActionType Type) where T : ISpaceTuple;

[GenerateSerializer]
internal enum TupleActionType
{
    Insert,
    Remove,
    Clear
}
