using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct StringTuple : IObjectTuple<string, StringTuple>, ISpanFormattable
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

    public int CompareTo(StringTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";


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

            int capacity = 0;

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

                capacity += 2 * thisCharLength;
            }

            return new Comparer(this, other).Execute(capacity);
        }

        return this.SequentialEquals(other);
    }

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        destination.Clear();  // we don't know if the memory represented by the span might contain garbage values, so we clear it.
        charsWritten = 0;

        int tupleLength = Length;
        if (tupleLength == 0)
        {
            destination[charsWritten++] = '(';
            destination[charsWritten++] = ')';

            return true;
        }

        int capacity = CalculateTotalLength(this);
        return new Formatter(this).Execute(capacity);

        static int CalculateTotalLength(StringTuple tuple)
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

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public ReadOnlySpan<string>.Enumerator GetEnumerator() => new ReadOnlySpan<string>(fields).GetEnumerator();

    internal readonly struct Formatter : IBufferConsumer<char>
    {
        private readonly StringTuple tuple;

        public Formatter(StringTuple tuple)
        {
            this.tuple = tuple;
        }

        public bool Consume(ref Span<char> buffer)
        {
            int charsWritten = 0;
            int tupleLength = tuple.Length;

            if (tupleLength == 1)
            {
                buffer[charsWritten++] = '(';

                string field = tuple[0];    // no allocation, as we are accessing the reference to the string field at the 0-th position in the array.
                if (!field.AsSpan().TryCopyTo(buffer[charsWritten..]))
                {
                    return false;
                }

                charsWritten += field.Length;
                buffer[charsWritten++] = ')';

                return true;
            }

            buffer[charsWritten++] = '(';

            for (int i = 0; i < tupleLength; i++)
            {
                if (i > 0)
                {
                    buffer[charsWritten++] = ',';
                    buffer[charsWritten++] = ' ';
                }

                string field = tuple[i];    // no allocation, as we are accessing the reference to the string field at the i-th position in the array.
                if (!field.AsSpan().TryCopyTo(buffer[charsWritten..]))
                {
                    return false;
                }

                charsWritten += field.Length;
            }

            buffer[charsWritten++] = ')';

            return true;
        }
    }

    internal readonly struct Comparer : IBufferConsumer<char>
    {
        private readonly StringTuple left;
        private readonly StringTuple right;

        public Comparer(StringTuple left, StringTuple right)
        {
            this.left = left;
            this.right = right;
        }

        public bool Consume(ref Span<char> buffer)
        {   
            int tupleLength = left.Length;
            int bufferHalfLength = buffer.Length / 2;

            Span<char> leftSpan = buffer[..bufferHalfLength];
            Span<char> rightSpan = buffer[bufferHalfLength..];

            int cursor = 0;

            for (int i = 0; i < tupleLength; i++)
            {
                ReadOnlySpan<char> thisFieldSpan = left[i].AsSpan();
                ReadOnlySpan<char> otherFieldSpan = right.fields[i].AsSpan();

                int spanLength = thisFieldSpan.Length;

                thisFieldSpan.CopyTo(leftSpan.Slice(cursor, spanLength));
                otherFieldSpan.CopyTo(rightSpan.Slice(cursor, spanLength));

                cursor += spanLength;
            }

            return new NumericMarshaller<char, ushort>(leftSpan, rightSpan).ParallelEquals();
        }
    }
}