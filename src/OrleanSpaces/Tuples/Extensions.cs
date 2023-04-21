﻿using System;
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

        if (!left.Fields.IsVectorizable())
        {
            return false;
        }

        equalityResult = ParallelEquals(left.Fields, right.Fields);
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

    /// <remarks><i>Ensure the <see cref="Span{T}.Length"/>(s) of <paramref name="left"/> and <paramref name="right"/> are equal beforehand.</i></remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ParallelEquals<T>(this Span<T> left, Span<T> right)
        where T : struct, INumber<T>
    {
        ref T iLeft = ref left.GetZeroIndex();
        ref T iRight = ref right.GetZeroIndex();

        int i = 0;

        int vCount = Vector<T>.Count;
        int vlength = left.Length / vCount;

        ref Vector<T> vLeft = ref Transform<T, Vector<T>>(in iLeft);
        ref Vector<T> vRight = ref Transform<T, Vector<T>>(in iRight);

        for (; i < vlength; i++)
        {
            if (vLeft.Offset(i) != vRight.Offset(i))
            {
                return false;
            }
        }

        i *= vCount;

        int length = left.Length;

        for (; i < length; i++)
        {
            if (iLeft.Offset(i) != iRight.Offset(i))
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
    public static ref T GetZeroIndex<T>(this Span<T> span) where T : struct
        => ref MemoryMarshal.GetReference(span);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T Offset<T>(this ref T source, int count) where T : struct
        => ref Unsafe.Add(ref source, count);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TOut Transform<TIn, TOut>(in TIn value)
        where TIn : struct
        where TOut : struct
        => ref Unsafe.As<TIn, TOut>(ref Unsafe.AsRef(in value));

    private const string emptyTupleString = "()";
    private const int bracketsCount = 2;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToTupleString<T, TSelf>(this ISpaceTuple<T, TSelf> tuple)
        where T : notnull
        where TSelf : ISpaceTuple<T, TSelf>
    {
        int length = tuple.Length;
        if (length == 0)
        {
            return emptyTupleString;
        }

        if (length == 1)
        {
            return $"({tuple[0]})";
        }

        int separatorsCount = 2 * (length - 1);
        int bufferLength = length + separatorsCount + bracketsCount;

        return string.Create(bufferLength, tuple, (buffer, state) => {

            buffer[0] = '(';
            int i = 1;

            //Span<char> bridgeSpan = stackalloc char[state.MaxCharsWrittable];

            for (int j = 0; j < length; j++)
            {
                if (j > 0)
                {
                    buffer[i++] = ',';
                    buffer[i++] = ' ';
                }


                //state.WriteTo(buffer.Slice(i), j, out int charsWritten);
                //i += charsWritten;

                //state.WriteTo(bridgeSpan, j, out int charsWritten);
                //bridgeSpan.CopyTo(buffer.Slice(i, charsWritten));
                //i += bridgeSpan.Length;

                ReadOnlySpan<char> span = state[j].ToString().AsSpan();
                span.CopyTo(buffer.Slice(i, span.Length));

                //i += span.Length;
            }

            buffer[i] = ')';
        });
    }
}