using System.Numerics;

namespace OrleanSpaces.Tuples.Typed;

internal interface INumericSpaceTuple<TValue, TType> : ISpaceTuple<TValue, TType>
    where TValue : struct, INumber<TValue>
    where TType : ISpaceTuple<TValue, TType>
{
    Span<TValue> Data { get; }
}