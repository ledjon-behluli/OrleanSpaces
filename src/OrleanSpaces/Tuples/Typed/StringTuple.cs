using Orleans.Concurrency;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct StringTuple : ISpaceTuple<char>, IEquatable<StringTuple>, IComparable<StringTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>a</example>
    internal const int MaxFieldCharLength = 1;

    private readonly string[] fields;

    public ref readonly string this[int index] => ref fields[index];
    ref readonly char ISpaceTuple<char>.this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            for (int i = 0; i < Length; i++)
            {
                ReadOnlySpan<char> span = fields[i].AsSpan();
                if (index < span.Length)
                {
                    return ref span[index];
                }
                else
                {
                    index -= span.Length;
                }
            }

            throw new IndexOutOfRangeException();
        }
    }

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
        if (Length != other.Length)
        {
            return false;
        }

        if (Length == 1)
        {
            return this[0] == other[0];
        }

        if (!Vector.IsHardwareAccelerated)
        {
            return this.SequentialEquals(other);
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

        return new Comparer(this, other).AllocateAndExecute(capacity);
    }

    public ReadOnlySpan<char> AsSpan()
    {
        int tupleLength = Length;

        SFString[] sfStrings = new SFString[tupleLength];
        for (int i = 0; i < tupleLength; i++)
        {
            sfStrings[i] = new(this[i]);
        }

        return new SFStringTuple(sfStrings).AsSpan(MaxFieldCharLength);
    }

    public ReadOnlySpan<string>.Enumerator GetEnumerator() => new ReadOnlySpan<string>(fields).GetEnumerator();

    readonly record struct SFStringTuple(params SFString[] Values) : ISpaceTuple<SFString>
    {
        public ref readonly SFString this[int index] => ref Values[index];
        public int Length => Values.Length;

        public ReadOnlySpan<char> AsSpan() => ReadOnlySpan<char>.Empty;
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

            NumericMarshaller<char, ushort> marshaller = new(leftSpan, rightSpan);
            return marshaller.Left.ParallelEquals(marshaller.Right);
        }
    }
}