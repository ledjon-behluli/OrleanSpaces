using OrleanSpaces.Tuples;

namespace OrleanSpaces;

[GenerateSerializer, Immutable]
internal readonly record struct TupleAction<T>(Guid AgentId, TupleAddress<T> Address, TupleActionType Type) where T : ISpaceTuple;

[GenerateSerializer]
internal enum TupleActionType
{
    Insert,
    Remove,
    Clear
}
