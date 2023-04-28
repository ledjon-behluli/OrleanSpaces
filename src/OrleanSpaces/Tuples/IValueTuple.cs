namespace OrleanSpaces.Tuples;

public interface IValueTuple<T, TSelf> : ISpaceTuple, IEquatable<TSelf>, IComparable<TSelf>
    where T : struct
    where TSelf : IValueTuple<T, TSelf>
{
    ref readonly T this[int index] { get; }
}