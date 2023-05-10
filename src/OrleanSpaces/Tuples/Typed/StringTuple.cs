using Orleans.Concurrency;
using System.Numerics;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct StringTuple : ISpaceTuple<string, StringTuple>, ISpanFormattable
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>a</example>
    internal const int MaxFieldCharLength = 1;

    private readonly string[] fields;

    public ref readonly string this[int index] => ref fields[index];
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

            return new Comparer(this, other).Execute(capacity, out _);
        }

        return this.SequentialEquals(other);
    }

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        int tupleLength = Length;
        int totalLength = CalculateTotalLength(this);

        SFString[] sfStrings = new SFString[tupleLength];
        for (int i = 0; i < tupleLength; i++)
        {
            sfStrings[i] = new(this[i]);
        }

        TupleFormatter<SFString, SFStringTuple> formatter = new(new SFStringTuple(sfStrings), MaxFieldCharLength);
        return formatter.TryFormat(totalLength, destination, out charsWritten);

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

    readonly record struct SFStringTuple(params SFString[] Values) : ISpaceTuple<SFString, SFStringTuple>
    {
        public ref readonly SFString this[int index] => ref Values[index];
        public int Length => Values.Length;

        public int CompareTo(SFStringTuple other) => Length.CompareTo(other.Length);
    }

    readonly record struct SFString(string Value) : ISpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => Value;

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            ReadOnlySpan<char> chars = Value.AsSpan();

            if (chars.TryCopyTo(destination))
            {
                charsWritten = chars.Length;
                return true;
            }
            else
            {
                charsWritten = 0;
                return false;
            }
        }
    }

    readonly struct Comparer : IBufferConsumer<char>
    {
        private readonly StringTuple left;
        private readonly StringTuple right;

        public Comparer(StringTuple left, StringTuple right)
        {
            this.left = left;
            this.right = right;
        }

        public bool Consume(ref Span<char> buffer, out int _)
        {
            _ = 0;

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