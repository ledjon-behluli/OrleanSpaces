namespace OrleanSpaces.Tuples;

public interface ISpaceFormattable
{
    bool TryFormat(Span<char> destination, out int charsWritten);
}