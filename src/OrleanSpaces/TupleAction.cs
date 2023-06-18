namespace OrleanSpaces;

internal readonly struct TupleAction<T>
{
    public readonly T Tuple;
    public readonly TupleActionType Type;

    public TupleAction(T tuple, TupleActionType type)
    {
        Tuple = tuple;
        Type = type;
    }
}

public enum TupleActionType
{
    Added,
    Removed,
    Cleaned
}
