using System.Numerics;

namespace OrleanSpaces.Tuples;

internal interface INumericSpaceTuple<T, TSelf> : ISpaceTuple<T, TSelf>//, ISpaceFormattable
    where T : struct, INumber<T>
    where TSelf : ISpaceTuple<T, TSelf>
{
    Span<T> Fields { get; }
}