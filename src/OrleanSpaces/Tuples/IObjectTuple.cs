namespace OrleanSpaces.Tuples;

public interface IObjectTuple<T, TSelf> : ISpaceTuple, IEquatable<TSelf>, IComparable<TSelf>
    where T : notnull
    where TSelf : IObjectTuple<T, TSelf>
{
    T this[int index] { get; }
}
