using System.Numerics;

namespace OrleanSpaces.Tuples;

internal interface INumericTuple<T> : ISpaceTuple<T>
    where T : struct, INumber<T>
{
    Span<T> Fields { get; }
}