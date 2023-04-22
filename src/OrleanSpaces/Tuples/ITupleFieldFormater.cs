namespace OrleanSpaces.Tuples;

internal interface ITupleFieldFormater
{
    static abstract int MaxCharsWrittable { get; }
    bool TryFormat(int index, Span<char> destination, out int charsWritten);
}