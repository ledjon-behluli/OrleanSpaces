using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OrleanSpaces.Tuples;

internal static class Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParallelEquals<T, TSelf>(this INumericTuple<T, TSelf> left, INumericTuple<T, TSelf> right, out bool equalityResult)
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFormatTupleField<T, TSelf>(this ISpaceTuple<T, TSelf> tuple, int index, Span<char> destination, out int charsWritten)
      where T : notnull
      where TSelf : ISpaceTuple<T, TSelf>, ITupleFieldFormater<T>
    {
        charsWritten = 0;

        if (index < 0 || index >= tuple.Length)
        {
            return false;
        }

        return TSelf.TryFormat(tuple[index], destination, out charsWritten);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFormatTuple<T, TSelf>(this ISpaceTuple<T, TSelf> tuple, Span<char> destination, out int charsWritten)
       where T : notnull
       where TSelf : ISpaceTuple<T, TSelf>, ITupleFieldFormater<T>
    {
        charsWritten = 0;

        int tupleLength = tuple.Length;
        int maxCharsWrittable = TSelf.MaxCharsWrittable;
        int totalLength = CalculateTotalLength(tuple.Length, maxCharsWrittable);

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
        Span<char> fieldSpan = stackalloc char[maxCharsWrittable];

        if (tupleLength == 1)
        {
            tupleSpan[charsWritten++] = '(';

            FormatField(tuple[0], tupleSpan, fieldSpan, ref charsWritten);

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
            }

            fieldSpan.Clear();
            FormatField(tuple[i], tupleSpan, fieldSpan, ref charsWritten);
        }

        tupleSpan[charsWritten++] = ')';
        tupleSpan[..(charsWritten + 1)].CopyTo(destination);

        return true;

        static int CalculateTotalLength(int tupleLength, int maxCharsWrittable)
        {
            int separatorsCount = tupleLength == 0 ? 0 : 2 * (tupleLength - 1);
            int destinationSpanLength = tupleLength == 0 ? 2 : maxCharsWrittable * tupleLength;
            int totalLength = tupleLength == 0 ? 2 : destinationSpanLength + separatorsCount + 2;

            return totalLength;
        }

        static void FormatField(T field, Span<char> tupleSpan, Span<char> fieldSpan, ref int charsWritten)
        {
            _ = TSelf.TryFormat(field, fieldSpan, out int fieldCharsWritten);
            fieldSpan[..fieldCharsWritten].CopyTo(tupleSpan.Slice(charsWritten, fieldCharsWritten));
            charsWritten += fieldCharsWritten;
        }
    }
}