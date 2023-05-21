namespace OrleanSpaces.Tuples;

public interface ISpaceTuple
{
    object this[int index] { get; }
    int Length { get; }
}

public interface ISpaceTuple<T>
    where T : struct
{
    ref readonly T this[int index] { get; }
    int Length { get; }

    ReadOnlySpan<char> AsSpan();
}