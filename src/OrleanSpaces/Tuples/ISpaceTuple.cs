namespace OrleanSpaces.Tuples;

public interface ISpaceTuple<T, TSelf> : IEquatable<TSelf>, IComparable<TSelf>
    where T : notnull
    where TSelf : ISpaceTuple<T, TSelf>
{
    T this[int index] { get; }
    int Length { get; }
}