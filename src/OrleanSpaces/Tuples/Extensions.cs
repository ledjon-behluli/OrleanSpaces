using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OrleanSpaces.Tuples;

internal static class Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParallelEquals<T, TSelf>(this INumericValueTuple<T, TSelf> left, INumericValueTuple<T, TSelf> right, out bool result)
        where T : struct, INumber<T>
        where TSelf : INumericValueTuple<T, TSelf>
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
        where TOut : struct, INumber<TOut>
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

    /// <remarks><i>Ensure the <see cref="NumericMarshaller{TIn, TOut}.Left"/> and <see cref="NumericMarshaller{TIn, TOut}.Right"/> are of equal slots beforehand.</i></remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ParallelEquals<TIn, TOut>(this NumericMarshaller<TIn, TOut> marshaller)
        where TIn : struct
        where TOut : struct, INumber<TOut>
        => ParallelEquals(marshaller.Left, marshaller.Right);

    /// <remarks><i>Ensure the <see cref="Span{T}.Length"/>(s) of <paramref name="left"/> and <paramref name="right"/> are of equal slots beforehand.</i></remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ParallelEquals<T>(this Span<T> left, Span<T> right)
        where T : struct, INumber<T>
    {
        int length = left.Length;
        if (length == 0)
        {
            return true;  // no elements, therefor 'right' & 'right' are equal.
        }

        if (length == 1)
        {
            return left[0] == right[0];  // avoiding overhead by doing a non-vectorized equality check, as its a single operation eitherway.
        }

        Vector<T> vLeft = left.AsVector();
        Vector<T> vRight = right.AsVector();

        int vCount = Vector<T>.Count;
        int vLength = length / vCount;

        if (vLength == 0)
        {
            vLength = 1;
        }

        int i = 0;

        for (; i < vLength; i++)
        {
            if (vLeft.Offset(i) != vRight.Offset(i))
            {
                return false;
            }
        }

        i *= vCount;

        int remaining = length - i;
        if (remaining < 1)
        {
            return true;  // means [slots % vCount = 0] therefor all elements have been compared (in parallel), and none were different (otherwise 'false' would have been returned) 
        }

        if (remaining == 1)
        {
            return left[i] == right[i];  // avoiding overhead by doing a non-vectorized equality check, as its a single operation eitherway.
        }

        vLeft = left.Slice(i, remaining).AsVector();   // vector will have [i + vCount - remaining] elements set to default(T)
        vRight = right.Slice(i, remaining).AsVector(); // vector will have [i + vCount - remaining] elements set to default(T)

        return vLeft == vRight;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequentialEquals<T, TSelf>(this IObjectTuple<T, TSelf> left, IObjectTuple<T, TSelf> right)
        where T : notnull
        where TSelf : IObjectTuple<T, TSelf>
    {
        int length = left.Length;
        if (length != right.Length)
        {
            return false;
        }

        for (int i = 0; i < length; i++)
        {
            if (!left[i].Equals(right[i]))
            {
                return false;
            }
        }

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequentialEquals<T, TSelf>(this IValueTuple<T, TSelf> left, IValueTuple<T, TSelf> right)
         where T : struct
         where TSelf : IValueTuple<T, TSelf>
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
    public static bool TryFormatTuple<T, TSelf>(this IValueTuple<T, TSelf> tuple, int maxFieldCharLength, Span<char> destination, out int charsWritten)
        where T : struct, ISpanFormattable
        where TSelf : IValueTuple<T, TSelf>
    {
        charsWritten = 0;

        int tupleLength = tuple.Length;
        int totalLength = CalculateTotalLength(tupleLength, maxFieldCharLength);

        if (destination.Length < totalLength)
        {
            charsWritten = 0;
            return false;
        }

        if (tupleLength == 0)
        {
            destination[charsWritten++] = '(';
            destination[charsWritten++] = ')';

            return true;
        }

        Span<char> tupleSpan = stackalloc char[totalLength];
        Span<char> fieldSpan = stackalloc char[maxFieldCharLength];

        if (tupleLength == 1)
        {
            tupleSpan[charsWritten++] = '(';

            if (!TryFormatField(in tuple[0], tupleSpan, fieldSpan, ref charsWritten))
            {
                return false;
            }

            tupleSpan[charsWritten++] = ')';
            tupleSpan[..(charsWritten + 1)].CopyTo(destination);

            return true;
        }

        tupleSpan[charsWritten++] = '(';

        for (int i = 0; i < tupleLength; i++)
        {
            if (i > 0)
            {
                tupleSpan[charsWritten++] = ',';
                tupleSpan[charsWritten++] = ' ';

                fieldSpan.Clear();
            }

            if (!TryFormatField(in tuple[i], tupleSpan, fieldSpan, ref charsWritten))
            {
                return false;
            }
        }

        tupleSpan[charsWritten++] = ')';
        tupleSpan[..(charsWritten + 1)].CopyTo(destination);

        return true;

        static bool TryFormatField(in T field, Span<char> tupleSpan, Span<char> fieldSpan, ref int charsWritten)
        {
            if (!field.TryFormat(fieldSpan, out int fieldCharsWritten, default, null))
            {
                charsWritten = 0;
                return false;
            }

            fieldSpan[..fieldCharsWritten].CopyTo(tupleSpan.Slice(charsWritten, fieldCharsWritten));
            charsWritten += fieldCharsWritten;

            return true;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CalculateTotalLength(int tupleLength, int maxFieldCharLength)
    {
        int separatorsCount = tupleLength == 0 ? 0 : 2 * (tupleLength - 1);
        int destinationSpanLength = tupleLength == 0 ? 2 : maxFieldCharLength * tupleLength;
        int totalLength = tupleLength == 0 ? 2 : destinationSpanLength + separatorsCount + 2;

        return totalLength;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Vector<T> AsVector<T>(this Span<T> span)
        where T : struct
    {
        int sLength = span.Length;
        int vLength = Vector<T>.Count;

        ref T first = ref span[0];
        ref Vector<T> vector = ref CastAs<T, Vector<T>>(in first);

        if (vLength > sLength)
        {
            Span<T> tempSpan = MemoryMarshal.CreateSpan(ref first, vLength);
            span.CopyTo(tempSpan);
            tempSpan[span.Length..].Clear();  // We slice the tempSpan from 'span.Length' to the end, and then use 'Clear' which initializes the memory in a given Span<T> to its default value.
            vector = ref Unsafe.As<T, Vector<T>>(ref MemoryMarshal.GetReference(tempSpan));
        }

        return ref vector;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T Offset<T>(this ref T source, int count) where T : struct
        => ref Unsafe.Add(ref source, count);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TOut CastAs<TIn, TOut>(in TIn value) // 'value' is passed using 'in' to avoid defensive copying.
        where TIn : struct
        where TOut : struct
        => ref Unsafe.As<TIn, TOut>(ref Unsafe.AsRef(in value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AreEqual<TValue, TValueType, TEquator>(int slots, in TValueType left, in TValueType right)  // 'left' and 'right' are passed using 'in' to avoid defensive copying.
        where TValue : unmanaged
        where TValueType : struct
        where TEquator : ISpanEquatable<TValue, TValueType>
    {
        int totalSlots = 2 * slots;  // 2x because we need to stack allocate memory for 'left' and 'right'
        if (totalSlots <= 1024)  // Its good practice not to allocate more than 1 kilobyte of memory on the stack 
        {
            Span<TValue> leftSpan = stackalloc TValue[slots];
            Span<TValue> rightSpan = stackalloc TValue[slots];

            return TEquator.Equals(left, leftSpan, right, rightSpan);
        }
        else if (totalSlots <= 1_048_576)  // 1,048,576 is the maximum array length of ArrayPool.Shared
        {
            TValue[] buffer = ArrayPool<TValue>.Shared.Rent(totalSlots);

            Span<TValue> leftSpan = new(buffer, 0, slots);
            Span<TValue> rightSpan = new(buffer, slots, slots);

            // Since 'ArrayPool.Shared' could be used from user code, we need to be sure that the Span<TValue>(s) are cleared, before handing them out.
            leftSpan.Clear();
            rightSpan.Clear();

            bool result = TEquator.Equals(left, leftSpan, right, rightSpan);
            ArrayPool<TValue>.Shared.Return(buffer);  // No need to clear the array, since it will be cleared by the Span<TValue>(s).

            return result;
        }
        else
        {
            Span<TValue> leftSpan = new TValue[slots];
            Span<TValue> rightSpan = new TValue[slots];

            return TEquator.Equals(left, leftSpan, right, rightSpan);
        }
    }
}