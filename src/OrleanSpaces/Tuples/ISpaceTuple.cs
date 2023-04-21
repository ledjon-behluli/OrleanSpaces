namespace OrleanSpaces.Tuples;

public interface ISpaceTuple
{
    
}

internal interface ISpaceTuple<T, TSelf> : ISpaceTuple, ISpaceFormattable, IEquatable<TSelf>, IComparable<TSelf>
    where T : notnull
    where TSelf : ISpaceTuple<T, TSelf>
{
    T this[int index] { get; }
    int Length { get; }
}
