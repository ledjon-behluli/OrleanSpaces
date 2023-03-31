using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OrleanSpaces.Tuples.Numerics;

namespace OrleanSpaces.Tuples;

internal static class Extensions
{
    public static bool SimdEquals<T, H>(this INumericTuple<T, H> left, INumericTuple<T, H> right)
        where T : struct, INumber<T>
        where H : ISpaceTuple<T, H>
    {
        ref T rLeft = ref GetRef(left);
        ref T rRight = ref GetRef(right);

        int i = 0;

        if (Vector.IsHardwareAccelerated)
        {
            ref Vector<T> vLeft = ref AsVector(rLeft);
            ref Vector<T> vRight = ref AsVector(rRight);

            int length = left.Length / Vector<T>.Count;

            for (; i < length; i++)
            {
                if (vLeft.Offset(i) != vRight.Offset(i))
                {
                    return false;
                }
            }

            i *= Vector<T>.Count;
        }

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
    private static ref T GetRef<T, H>(INumericTuple<T, H> tuple)
        where T : struct, INumber<T>
        where H : ISpaceTuple<T, H>
        => ref MemoryMarshal.GetReference(tuple.Fields.AsSpan());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref T Offset<T>(this ref T source, int count) where T : struct
        => ref Unsafe.Add(ref source, count);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref Vector<T> AsVector<T>(in T value) where T : struct, INumber<T>
        => ref Unsafe.As<T, Vector<T>>(ref Unsafe.AsRef(value));
}