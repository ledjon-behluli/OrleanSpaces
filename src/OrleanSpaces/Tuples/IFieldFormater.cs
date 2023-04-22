namespace OrleanSpaces.Tuples;

internal interface IFieldFormater<T> where T : notnull
{
    static abstract int MaxCharsWrittable { get; }
    static abstract bool TryFormat(T field, Span<char> destination, out int charsWritten);
}