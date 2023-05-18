namespace OrleanSpaces.Tuples;

public interface ISpaceTuple
{

}

public interface ISpaceTuple<T> : ISpaceTuple 
    where T : notnull
{
    ref readonly T this[int index] { get; }
    int Length { get; }

    ReadOnlySpan<char> AsSpan();
}