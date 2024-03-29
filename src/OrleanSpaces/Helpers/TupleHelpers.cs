﻿using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using OrleanSpaces.Helpers;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Helpers;

internal static class TupleHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParallelEquals<T, TTuple>(this TTuple left, TTuple right, out bool result)
        where T : unmanaged, INumber<T>
        where TTuple : INumericTuple<T>
    {
        result = false;

        if (left.Length != right.Length)
        {
            return true;
        }

        if (!Vector.IsHardwareAccelerated)
        {
            return false;
        }

        result = left.Fields.ParallelEquals(right.Fields);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParallelEquals<TIn, TOut>(this NumericMarshaller<TIn, TOut> marshaller, out bool result)
        where TIn : unmanaged
        where TOut : unmanaged, INumber<TOut>
    {
        result = false;

        if (marshaller.Left.Length != marshaller.Right.Length)
        {
            return true;
        }

        if (!Vector.IsHardwareAccelerated)
        {
            return false;
        }

        result = marshaller.Left.ParallelEquals(marshaller.Right);
        return true;
    }

    /// <remarks>
    /// <list type="bullet">
    /// <item><description>Ensure the <see cref="Span{T}.Length"/>(s) of <paramref name="left"/> and <paramref name="right"/> are equal beforehand.</description></item>
    /// <item><description>Ensure <see cref="Vector.IsHardwareAccelerated"/> is <see langword="true"/> beforehand.</description></item>
    /// </list>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ParallelEquals<T>(this Span<T> left, Span<T> right)
        where T : unmanaged, INumber<T>
    {
        int length = left.Length;
        if (length == 0)
        {
            return true;  // no elements, therefor 'left' & 'right' are equal.
        }

        if (length == 1)
        {
            return left[0] == right[0];  // avoiding overhead by doing a non-vectorized equality check, as its a single operation eitherway.
        }

        int vCount = Vector<T>.Count;
        int vLength = length / vCount;

        if (vLength == 0)
        {
            vLength = 1;
        }

        int i = 0;

        for (; i < vLength; i++)
        {
            Vector<T> vLeft = CastAsVector(left, i * vCount, vCount);
            Vector<T> vRight = CastAsVector(right, i * vCount, vCount);

            if (vLeft != vRight)
            {
                return false;
            }
        }

        i *= vCount;

        int remaining = length - i;
        if (remaining < 1)
        {
            return true;  // means [capacity % vCount = 0] therefor all elements have been compared (in parallel), and none were different (otherwise 'false' would have been returned) 
        }

        if (remaining == 1)
        {
            return left[i] == right[i];  // avoiding overhead by doing a non-vectorized equality check, as its a single operation eitherway.
        }

        Vector<T> _vLeft = CastAsVector(left, i, remaining);   // vector will have [i + vCount - remaining] elements set to default(TValue)
        Vector<T> _vRight = CastAsVector(right, i, remaining); // vector will have [i + vCount - remaining] elements set to default(TValue)

        return _vLeft == _vRight;

        static Vector<T> CastAsVector(Span<T> span, int start, int count)
        {
            int sLength = span.Length;
            if (sLength > count)
            {
                span = span.Slice(start, count);
            }

            int vLength = Vector<T>.Count;
            if (sLength == vLength)
            {
                // safe to create the vector directly from the span, as the lengths are equal (parent method will never supply a span that is [sLength > vLength])
                return Helpers.CastAs<T, Vector<T>>(in span[0]);
            }

            // In cases where [sLength < vLength], if we try to create a vector from the span directly, it will result in a vector which has the first N items the same as the span,
            // but the rest will not be consistent for subsequent calls, even if a second span over a memory that contains the same values of 'TValue' as the first span did.
            // That is why we need to create a temporary span with the length equal to that of the vLength for the given type 'TValue', initialize all items to TValue.Zero a.k.a 'the default',
            // proceed to copy the contents of the original span into it. This is necessary because if we created the vector directly from the original span, and since [sLength < vLength],
            // the vector may contain garbage values in its remaining elements, which could cause incorrect results when compared with another vector which may contain other garbage values.

            Span<T> tempSpan = stackalloc T[vLength];
            tempSpan.Fill(T.Zero);
            span.CopyTo(tempSpan);

            return Helpers.CastAs<T, Vector<T>>(in tempSpan[0]);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequentialEquals<T, TTuple>(this TTuple left, TTuple right)
        where T : unmanaged
        where TTuple : ISpaceTuple<T>
    {
        int length = left.Length;
        if (length != right.Length)
        {
            return false;
        }

        for (int i = 0; i < length; i++)
        {
            ref readonly T iLeft = ref left[i];
            ref readonly T iRight = ref right[i];

            if (!iLeft.Equals(iRight))
            {
                return false;
            }
        }

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> AsSpan<T, TTuple>(this TTuple tuple, int maxFieldCharLength)
       where T : unmanaged, ISpanFormattable
       where TTuple : ISpaceTuple<T>
    {
        int capacity = GetCharCount(tuple.Length, maxFieldCharLength);
        char[] array = ArrayPool<char>.Shared.Rent(capacity);

        Span<char> buffer = array.AsSpan();

        int index = 0;
        int tupleLength = tuple.Length;

        buffer.Clear(); // we dont know if the memory represented by the span might contain garbage values, so we clear it.

        if (tupleLength == 0)
        {
            buffer[index++] = '(';
            buffer[index++] = ')';
        }
        else
        {
            Span<char> fieldSpan = stackalloc char[maxFieldCharLength]; // its safe to allocate memory on the stack because the maxFieldCharLength is a constant on all tuples, and has a finite value well below 1Kb
            fieldSpan.Clear();

            if (tupleLength == 1)
            {
                buffer[index++] = '(';
                FormatField(in tuple[0], fieldSpan, buffer, ref index);
                buffer[index++] = ')';
            }
            else
            {
                buffer[index++] = '(';

                for (int i = 0; i < tupleLength; i++)
                {
                    if (i > 0)
                    {
                        buffer[index++] = ',';
                        buffer[index++] = ' ';
                        fieldSpan.Clear();
                    }

                    FormatField(in tuple[i], fieldSpan, buffer, ref index);
                }

                buffer[index++] = ')';
            }
        }

        ArrayPool<char>.Shared.Return(array);

        return buffer[..index];

        static void FormatField(in T field, Span<char> fieldSpan, Span<char> destination, ref int charsWritten)
        {
            _ = field.TryFormat(fieldSpan, out int fieldCharsWritten, default, null);
            fieldSpan[..fieldCharsWritten].CopyTo(destination.Slice(charsWritten, fieldCharsWritten));
            charsWritten += fieldCharsWritten;
        }

        static int GetCharCount(int tupleLength, int maxFieldCharLength)
        {
            int separatorsCount = 0;
            int destinationSpanLength = 0;

            if (tupleLength > 0)
            {
                separatorsCount = 2 * (tupleLength - 1);
                destinationSpanLength = maxFieldCharLength * tupleLength;
            }

            int totalLength = 2 + destinationSpanLength + separatorsCount;
            return totalLength;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString<T>(T[] fields) where T : unmanaged
        => fields is null ? "()" : $"({string.Join(", ", fields)})";
}
