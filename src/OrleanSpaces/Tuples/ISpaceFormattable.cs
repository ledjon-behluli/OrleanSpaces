namespace OrleanSpaces.Tuples;

public interface ISpaceFormattable
{
    bool TryFormat(Span<char> destination, out int charsWritten);
    bool TryFormat(int index, Span<char> destination, out int charsWritten);
}