using System.Numerics;

namespace OrleanSpaces.Tuples.Typed;

internal interface INumericSpaceTuple<T, H> : ISpaceTuple<T, H>
    where T : struct, INumber<T>
    where H : ISpaceTuple<T, H>
{
    Span<T> Data { get; }
}