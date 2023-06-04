using System.Numerics;
using System.Runtime.InteropServices;

namespace OrleanSpaces.Tuples;

internal readonly ref struct NumericMarshaller<TIn, TOut>
    where TIn : unmanaged
    where TOut : unmanaged, INumber<TOut>
{
    public readonly Span<TOut> Left;
    public readonly Span<TOut> Right;

    public NumericMarshaller(Span<TIn> left, Span<TIn> right)
    {
        Left = MemoryMarshal.Cast<TIn, TOut>(left);
        Right = MemoryMarshal.Cast<TIn, TOut>(right);
    }
}