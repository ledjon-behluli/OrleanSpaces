namespace OrleanSpaces.Tuples;

public interface ISpaceTuple
{

}

public interface ISpaceTuple<T, TSelf> : ISpaceTuple, IEquatable<TSelf>, IComparable<TSelf>
    where T : notnull
    where TSelf : ISpaceTuple<T, TSelf>
{
    ref readonly T this[int index] { get; }
    int Length { get; }

    ReadOnlySpan<char> AsSpan();
}