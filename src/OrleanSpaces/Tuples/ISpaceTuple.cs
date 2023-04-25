using System.Numerics;

namespace OrleanSpaces.Tuples;

public interface ISpaceTuple
{
    int Length { get; }   
}

public interface IObjectTuple<T, TSelf> : ISpaceTuple, IEquatable<TSelf>, IComparable<TSelf>
    where T : notnull
    where TSelf : IObjectTuple<T, TSelf>
{
    T this[int index] { get; }
}

public interface IValueTuple<T, TSelf> : ISpaceTuple, IEquatable<TSelf>, IComparable<TSelf>
    where T : struct
    where TSelf : IValueTuple<T, TSelf>
{
    ref readonly T this[int index] { get; }
}

internal interface INumericValueTuple<T, TSelf> : IValueTuple<T, TSelf>
    where T : struct, INumber<T>
    where TSelf : INumericValueTuple<T, TSelf>
{
    Span<T> Fields { get; }
}