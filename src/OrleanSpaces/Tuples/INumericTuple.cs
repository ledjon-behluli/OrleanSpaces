using System.Numerics;

namespace OrleanSpaces.Tuples;

internal interface INumericTuple<T, TSelf> : ISpaceTuple<T, TSelf>
    where T : struct, INumber<T>
    where TSelf : INumericTuple<T, TSelf>
{
    Span<T> Fields { get; }
}