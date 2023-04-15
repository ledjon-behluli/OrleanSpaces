using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OrleanSpaces.Tuples;

internal static class Extensions
{
    public static bool Equals<T, H>(INumericTuple<T, H> left, INumericTuple<T, H> right)
       where T : struct, INumber<T>
       where H : ISpaceTuple<T, H>
    {
        if (left.Length != right.Length)
        {
            return false;
        }

        if (!Vector.IsHardwareAccelerated)
        {
            return RegularEquals(left, right);
        }

        int length = left.Length / Vector<T>.Count;
        if (length == 0)
        {
            return RegularEquals(left, right);
        }

        ref T rLeft = ref GetRef(left);
        ref T rRight = ref GetRef(right);

        int i = 0;

        ref Vector<T> vLeft = ref AsVector(in rLeft);
        ref Vector<T> vRight = ref AsVector(in rRight);

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

    private static bool RegularEquals<T, H>(INumericTuple<T, H> left, INumericTuple<T, H> right)
         where T : struct, INumber<T>
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
    private static ref T GetRef<T, H>(INumericTuple<T, H> tuple)
        where T : struct, INumber<T>
        where H : ISpaceTuple<T, H>
        => ref MemoryMarshal.GetReference(tuple.Span);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref T Offset<T>(this ref T source, int count) where T : struct
        => ref Unsafe.Add(ref source, count);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref Vector<T> AsVector<T>(in T value) where T : struct, INumber<T>
        => ref Unsafe.As<T, Vector<T>>(ref Unsafe.AsRef(in value));
}