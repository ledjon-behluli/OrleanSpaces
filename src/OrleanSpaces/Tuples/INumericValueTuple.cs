using System.Numerics;

namespace OrleanSpaces.Tuples;

internal interface INumericValueTuple<T, TSelf> : IValueTuple<T, TSelf>
    where T : struct, INumber<T>
    where TSelf : INumericValueTuple<T, TSelf>
{
    Span<T> Fields { get; }
}