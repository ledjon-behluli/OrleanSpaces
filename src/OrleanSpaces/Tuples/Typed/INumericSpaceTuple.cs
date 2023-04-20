using System.Numerics;

namespace OrleanSpaces.Tuples.Typed;

internal interface INumericSpaceTuple<T, TSelf> : ISpaceTuple<T, TSelf>
    where T : struct, INumber<T>
    where TSelf : ISpaceTuple<T, TSelf>
{
    Span<T> Fields { get; }
}