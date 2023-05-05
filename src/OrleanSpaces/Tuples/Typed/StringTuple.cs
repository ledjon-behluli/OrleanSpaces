using Orleans.Concurrency;
using System;
using System.Numerics;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct StringTuple : IObjectTuple<string, StringTuple>, IMemoryComparer<char, StringTuple>, ISpanFormattable
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>a</example>
    internal const int MaxFieldCharLength = 1;

    private readonly string[] fields;

    public string this[int index] => fields[index];
    public int Length => fields.Length;

    public StringTuple() : this(Array.Empty<string>()) { }
    public StringTuple(params string[] fields) => this.fields = fields;

    public static bool operator ==(StringTuple left, StringTuple right) => left.Equals(right);
    public static bool operator !=(StringTuple left, StringTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is StringTuple tuple && Equals(tuple);

    public bool Equals(StringTuple other)
    {
        if (Vector.IsHardwareAccelerated)
        {
            if (Length != other.Length)
            {
                return false;
            }

            if (Length == 1)
            {
                return fields[0] == other.fields[0];
            }

            int slots = 0;

            for (int i = 0; i < Length; i++)
            {
                int thisCharLength = this[i].Length;
                int otherCharLength = other.fields[i].Length;

                if (thisCharLength != otherCharLength)
                {
                    // If the number of chars found in 'fields[i]' and 'other.fields[i]' are different, we dont need to perform any further equality checks
                    // as its evident that the tuples are different. For example: ("a", "b", "c") != ("a", "bb", "c")

                    return false;
                }

                slots += thisCharLength;
            }

            return Extensions.AllocateMemoryAndCheckEquality<char, StringTuple, StringTuple>(slots, in this, in other);
        }

        return this.SequentialEquals(other);
    }

    public int CompareTo(StringTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        destination.Clear();  // we don't know if the memory represented by the span might contain garbage values so we clear it.
        charsWritten = 0;

        int tupleLength = Length;
        if (tupleLength == 0)
        {
            destination[charsWritten++] = '(';
            destination[charsWritten++] = ')';

            return true;
        }

        int totalLength = CalculateTotalLength(in this);
        Span<char> fieldSpan = stackalloc char[totalLength]; // TODO: Improve

        if (tupleLength == 1)
        {
            destination[charsWritten++] = '(';

            if (!TryFormatField(fields[0], destination, fieldSpan, ref charsWritten))
            {
                return false;
            }

            destination[charsWritten++] = ')';

            return true;
        }

        destination[charsWritten++] = '(';

        for (int i = 0; i < tupleLength; i++)
        {
            if (i > 0)
            {
                destination[charsWritten++] = ',';
                destination[charsWritten++] = ' ';

                fieldSpan.Clear();
            }

            if (!TryFormatField(fields[i], destination, fieldSpan, ref charsWritten))
            {
                return false;
            }
        }

        destination[charsWritten++] = ')';

        return true;

        static bool TryFormatField(string field, Span<char> tupleSpan, Span<char> fieldSpan, ref int charsWritten)
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

        static int CalculateTotalLength(in StringTuple tuple)
        {
            int totalChars = 0;
            int length = tuple.Length;

            for (int i = 0; i < length; i++)
            {
                totalChars += tuple[i].Length;
            }

            int separatorsCount = length == 0 ? 0 : 2 * (length - 1);
            int totalLength = 2 + totalChars + separatorsCount;

            return totalLength;
        }
    }

    // (ab, 123)

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

    static bool IMemoryComparer<char, StringTuple>.Equals(in StringTuple left, ref Span<char> leftSpan, in StringTuple right, ref Span<char> rightSpan)
    {
        int cursor = 0;
        int length = left.Length;

        for (int i = 0; i < length; i++)
        {
            ReadOnlySpan<char> thisFieldSpan = left[i].AsSpan();
            ReadOnlySpan<char> otherFieldSpan = right.fields[i].AsSpan();

            int spanLength = thisFieldSpan.Length;

            thisFieldSpan.CopyTo(leftSpan.Slice(cursor, spanLength));
            otherFieldSpan.CopyTo(rightSpan.Slice(cursor, spanLength));

            cursor += spanLength;
        }

        return new NumericMarshaller<char, ushort>(leftSpan, rightSpan).ParallelEquals();  // See: CharTuple.AllocateMemoryAndCheckEquality for more details
    }

    public ReadOnlySpan<string>.Enumerator GetEnumerator() => new ReadOnlySpan<string>(fields).GetEnumerator();
}