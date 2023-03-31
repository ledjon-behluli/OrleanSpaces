using System.Numerics;

namespace OrleanSpaces.Tuples.Numerics;

internal interface INumericTuple<T, H> : ISpaceTuple<T, H>
    where T : struct, INumber<T>
    where H : ISpaceTuple<T, H>
{
    T[] Fields { get; }
}
