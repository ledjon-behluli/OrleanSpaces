namespace OrleanSpaces.Tuples;

public interface ISpaceTuple<TValue, TType> : IEquatable<TType>, IComparable<TType>
    where TValue : notnull
    where TType : ISpaceTuple<TValue, TType>
{
    TValue this[int index] { get; }
    int Length { get; }
}