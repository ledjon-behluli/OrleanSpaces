namespace OrleanSpaces.Tuples;

internal readonly ref struct SpanFormatProps
{
    public readonly int MaxCharsWrittable;
    public readonly int TupleLength;
    public readonly int DestinationSpanLength;
    public readonly int TotalLength;

    public SpanFormatProps(int tupleLength, int maxCharsWrittable)
    {
        int separatorsCount = tupleLength == 0 ? 0 : 2 * (tupleLength - 1);
        int destinationSpanLength = tupleLength == 0 ? 2 : maxCharsWrittable * tupleLength;
        int totalLength = tupleLength == 0 ? 2 : destinationSpanLength + separatorsCount + 2;

        MaxCharsWrittable = maxCharsWrittable;
        TupleLength = tupleLength;
        DestinationSpanLength = destinationSpanLength;
        TotalLength = totalLength;
    }
}