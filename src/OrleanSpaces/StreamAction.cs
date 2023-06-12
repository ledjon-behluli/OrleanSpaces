namespace OrleanSpaces;

internal readonly struct StreamAction<T>
{
    public readonly T Tuple;
    public readonly StreamActionType Type;

    public StreamAction(T tuple, StreamActionType type)
    {
        Tuple = tuple;
        Type = type;
    }
}

public enum StreamActionType
{
    Added,
    Removed
}
