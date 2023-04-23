namespace OrleanSpaces.Tuples;

public interface ISpaceTuple
{
    
}

public interface ISpaceTuple<T, TSelf> : ISpaceTuple, IEquatable<TSelf>, IComparable<TSelf>
    where T : notnull
    where TSelf : ISpaceTuple<T, TSelf>
{
    T this[int index] { get; }
    int Length { get; }
}