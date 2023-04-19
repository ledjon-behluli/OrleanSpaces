using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Tuples;

internal static class Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParallelEquals<T, TSelf>(this INumericSpaceTuple<T, TSelf> left, INumericSpaceTuple<T, TSelf> right, out bool equalityResult)
        where T : struct, INumber<T>
        where TSelf : ISpaceTuple<T, TSelf>
    {
        equalityResult = false;

        if (left.Length != right.Length)
        {
            return true;
        }

        if (!left.Data.IsVectorizable())
        {
            return false;
        }

        equalityResult = ParallelEquals(left.Data, right.Data);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParallelEquals<TIn, TOut>(this NumericMarshaller<TIn, TOut> marshaller, out bool equalityResult)
        where TIn : struct
        where TOut : struct, INumber<TOut>
    {
        equalityResult = false;

        if (marshaller.Left.Length != marshaller.Right.Length)
        {
            return true;
        }

        if (!marshaller.Left.IsVectorizable())
        {
            return false;
        }

        equalityResult = ParallelEquals(marshaller.Left, marshaller.Right);
        return true;
    }

    /// <remarks><i>Ensure the <see cref="Span{TOut}.Length"/>(s) of <paramref name="left"/> and <paramref name="right"/> are equal beforehand.</i></remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ParallelEquals<TOut>(this Span<TOut> left, Span<TOut> right) 
        where TOut : struct, INumber<TOut>
    {
        ref TOut rLeft = ref GetRef(left);
        ref TOut rRight = ref GetRef(right);

        int i = 0;

        ref Vector<TOut> vLeft = ref AsRef<TOut, Vector<TOut>>(in rLeft);
        ref Vector<TOut> vRight = ref AsRef<TOut, Vector<TOut>>(in rRight);

        for (; i < left.Length; i++)
        {
            if (vLeft.Offset(i) != vRight.Offset(i))
            {
                return false;
            }
        }

        i *= Vector<TOut>.Count;

        for (; i < left.Length; i++)
        {
            if (rLeft.Offset(i) != rRight.Offset(i))
            {
                return false;
            }
        }

        return true;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequentialEquals<T, TSelf>(this ISpaceTuple<T, TSelf> left, ISpaceTuple<T, TSelf> right)
         where T : notnull
         where TSelf : ISpaceTuple<T, TSelf>
    {
        if (left.Length != right.Length)
        {
            return false;
        }

        for (int i = 0; i < left.Length; i++)
        {
            if (!left[i].Equals(right[i]))
            {
                return false;
            }
        }

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsVectorizable<T>(this Span<T> span) where T : struct, INumber<T>
        => Vector.IsHardwareAccelerated && span.Length / Vector<T>.Count > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetRef<T>(Span<T> span) where T : struct
        => ref MemoryMarshal.GetReference(span);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T Offset<T>(this ref T source, int count) where T : struct
        => ref Unsafe.Add(ref source, count);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TOut AsRef<TIn, TOut>(in TIn value)
        where TIn : struct
        where TOut : struct
        => ref Unsafe.As<TIn, TOut>(ref Unsafe.AsRef(in value));
}