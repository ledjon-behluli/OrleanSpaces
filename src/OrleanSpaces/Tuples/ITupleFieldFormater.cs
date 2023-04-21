namespace OrleanSpaces.Tuples;

internal interface ITupleFieldFormater
{
    int MaxCharsWrittable { get; }
    bool TryFormat(int index, Span<char> destination, out int charsWritten);
}