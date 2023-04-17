using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Tuples;

internal static class Extensions
{
    public static bool TryParallelEquals<T, H>(this INumericSpaceTuple<T, H> left, INumericSpaceTuple<T, H> right, out bool equalityResult)
        where T : struct, INumber<T>
        where H : ISpaceTuple<T, H>
    {
        equalityResult = false;

        if (!Vector.IsHardwareAccelerated)
        {
            return false;
        }

        int length = left.Length / Vector<T>.Count;
        if (length == 0)
        {
            return false;
        }

        equalityResult = ParallelEquals(left.Data, right.Data, length);
        return true;
    }

    public static bool TryParallelEquals<TIn, TOut>(this NumericMarshaller<TIn, TOut> marshaller, out bool equalityResult)
        where TIn : struct
        where TOut : struct, INumber<TOut>
    {
        equalityResult = false;

        if (!Vector.IsHardwareAccelerated)
        {
            return false;
        }

        int length = marshaller.Left.Length / Vector<TOut>.Count;
        if (length == 0)
        {
            return false;
        }

        equalityResult = ParallelEquals(marshaller.Left, marshaller.Right, length);
        return true;
    }

    public static bool SequentialEquals<T, H>(this ISpaceTuple<T, H> left, ISpaceTuple<T, H> right)
         where T : notnull
         where H : ISpaceTuple<T, H>
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
    private static bool ParallelEquals<TOut>(Span<TOut> left, Span<TOut> right, int length) where TOut : struct, INumber<TOut>
    {
        if (left.Length != right.Length)
        {
            return false;
        }

        ref TOut rLeft = ref GetRef(left);
        ref TOut rRight = ref GetRef(right);

        int i = 0;

        ref Vector<TOut> vLeft = ref AsRef<TOut, Vector<TOut>>(in rLeft);
        ref Vector<TOut> vRight = ref AsRef<TOut, Vector<TOut>>(in rRight);

        for (; i < length; i++)
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