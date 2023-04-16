using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OrleanSpaces.Tuples.Typed.Numerics;

namespace OrleanSpaces.Tuples;

internal static class Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ParallelEquals<T, H>(this INumericTuple<T, H> left, INumericTuple<T, H> right)
       where T : struct, INumber<T>
       where H : ISpaceTuple<T, H>
    {
        if (left.Length != right.Length)
        {
            return false;
        }

        if (!Vector.IsHardwareAccelerated)
        {
            return SequentialEquals(left, right);
        }

        int length = left.Length / Vector<T>.Count;
        if (length == 0)
        {
            return SequentialEquals(left, right);
        }

        ref T rLeft = ref GetRef(left.Data);
        ref T rRight = ref GetRef(right.Data);

        int i = 0;

        ref Vector<T> vLeft = ref AsRef<T, Vector<T>>(in rLeft);
        ref Vector<T> vRight = ref AsRef<T, Vector<T>>(in rRight);

        for (; i < length; i++)
        {
            if (vLeft.Offset(i) != vRight.Offset(i))
            {
                return false;
            }
        }

        i *= Vector<T>.Count;

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
    public static bool SequentialEquals<T, H>(this ISpaceTuple<T, H> left, ISpaceTuple<T, H> right)
         where T : struct
         where H : ISpaceTuple<T, H>
    {
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