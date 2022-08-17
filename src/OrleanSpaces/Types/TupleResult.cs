namespace OrleanSpaces.Types;

[Serializable]
public readonly struct TupleResult
{
    public bool Result => Tuple != null;
    public SpaceTuple? Tuple { get; }

    public static TupleResult Empty = new();

    public TupleResult(SpaceTuple? tuple)
    {
        Tuple = tuple;
    }
}