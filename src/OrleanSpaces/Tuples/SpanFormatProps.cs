namespace OrleanSpaces.Tuples;

internal readonly ref struct SpanFormatProps
{
    internal const int BracketsCount = 2;

    public readonly int MaxCharsWrittable;
    public readonly int TupleLength;
    public readonly int DestinationSpanLength;
    public readonly int TotalLength;

    public SpanFormatProps(int tupleLength, int maxCharsWrittable)
    {
        int separatorsCount = 2 * (tupleLength - 1);
        int destinationSpanLength = maxCharsWrittable * tupleLength;
        int totalLength = destinationSpanLength + separatorsCount + BracketsCount;

        MaxCharsWrittable = maxCharsWrittable;
        TupleLength = tupleLength;
        DestinationSpanLength = destinationSpanLength;
        TotalLength = totalLength;
    }
}