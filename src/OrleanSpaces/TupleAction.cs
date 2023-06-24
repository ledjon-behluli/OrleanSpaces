﻿namespace OrleanSpaces;

[GenerateSerializer, Immutable]
internal readonly record struct TupleAction<T>(Guid AgentId, T Tuple, TupleActionType Type);

public enum TupleActionType
{
    Insert,
    Remove,
    Clean
}
