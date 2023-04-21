namespace OrleanSpaces.Tuples;

public interface IFieldFormater
{
    void Format(int index, Span<char> destination, out int charsWritten);
}