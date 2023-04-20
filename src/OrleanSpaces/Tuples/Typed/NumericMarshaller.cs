using System.Numerics;
using System.Runtime.InteropServices;

namespace OrleanSpaces.Tuples.Typed;

internal readonly ref struct NumericMarshaller<TIn, TOut>
    where TIn : struct
    where TOut : struct, INumber<TOut>
{
    public readonly ReadOnlySpan<TOut> Left;
    public readonly ReadOnlySpan<TOut> Right;

    public NumericMarshaller(ReadOnlySpan<TIn> left, ReadOnlySpan<TIn> right)
    {
        Left = MemoryMarshal.Cast<TIn, TOut>(left);
        Right = MemoryMarshal.Cast<TIn, TOut>(right);
    }
}