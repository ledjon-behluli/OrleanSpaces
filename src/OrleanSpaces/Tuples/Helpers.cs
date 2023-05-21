using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tuples;

internal static class Helpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSupportedType(this Type type) =>
       type.IsPrimitive ||
       type.IsEnum ||
       type == typeof(string) ||
       type == typeof(decimal) ||
       type == typeof(Int128) ||
       type == typeof(UInt128) ||
       type == typeof(DateTime) ||
       type == typeof(DateTimeOffset) ||
       type == typeof(TimeSpan) ||
       type == typeof(Guid);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParallelEquals<T>(this INumericTuple<T> left, INumericTuple<T> right, out bool result)
        where T : unmanaged, INumber<T>
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

        result = ParallelEquals(left.Fields, right.Fields);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParallelEquals<TIn, TOut>(this NumericMarshaller<TIn, TOut> marshaller, out bool result)
        where TIn : struct
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

        result = ParallelEquals(marshaller.Left, marshaller.Right);
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
                return CastAs<T, Vector<T>>(in span[0]);
            }

            // In cases where [sLength < vLength], if we try to create a vector from the span directly, it will result in a vector which has the first N items the same as the span,
            // but the rest will not be consistent for subsequent calls, even if a second span over a memory that contains the same values of 'TValue' as the first span did.
            // That is why we need to create a temporary span with the length equal to that of the vLength for the given type 'TValue', initialize all items to TValue.Zero a.k.a 'the default',
            // proceed to copy the contents of the original span into it. This is necessary because if we created the vector directly from the original span, and since [sLength < vLength],
            // the vector may contain garbage values in its remaining elements, which could cause incorrect results when compared with another vector which may contain other garbage values.

            Span<T> tempSpan = stackalloc T[vLength];
            tempSpan.Fill(T.Zero);
            span.CopyTo(tempSpan);
            
            return CastAs<T, Vector<T>>(in tempSpan[0]);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequentialEquals<T>(this ISpaceTuple<T> left, ISpaceTuple<T> right)
         where T : struct
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
    public static bool SequentialEquals(this ISpaceTuple left, ISpaceTuple right)
    {
        int length = left.Length;
        if (length != right.Length)
        {
            return false;
        }

        for (int i = 0; i < length; i++)
        {
            if (!left.Equals(right))
            {
                return false;
            }
        }

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> AsSpan<T>(this ISpaceTuple<T> tuple, int maxFieldCharLength)
       where T : struct, ISpanFormattable
    {
        int capacity = GetCharCount(tuple.Length, maxFieldCharLength);
        char[] array = Allocate<char>(capacity, out bool rented);

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

        if (rented)
        {
            ArrayPool<char>.Shared.Return(array);
        }

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
    public static bool AllocateAndExecute<T>(this IBufferConsumer<T> consumer, int capacity)
       where T : unmanaged
    {
        if (capacity * Unsafe.SizeOf<T>() <= 1024) // It is good practice not to allocate more than 1 kilobyte of memory on the stack
        {
            Span<T> buffer = stackalloc T[capacity];
            return consumer.Consume(ref buffer);
        }
        else
        {
            T[] array = Allocate<T>(capacity, out bool rented);
            Span<T> buffer = array.AsSpan();

            bool result = consumer.Consume(ref buffer);

            if (rented)
            {
                ArrayPool<T>.Shared.Return(array);
            }

            return result;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] Allocate<T>(int capacity, out bool rented)
    {
        if (capacity <= 1048576)  // 1,048,576 is the maximum array length of ArrayPool.Shared
        {
            rented = true;
            return ArrayPool<T>.Shared.Rent(capacity);
        }
        else
        {
            rented = false;
            return new T[capacity];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TOut CastAs<TIn, TOut>(in TIn value) // 'value' is passed using 'in' to avoid defensive copying.
       where TIn : struct
       where TOut : struct
       => ref Unsafe.As<TIn, TOut>(ref Unsafe.AsRef(in value));
}
