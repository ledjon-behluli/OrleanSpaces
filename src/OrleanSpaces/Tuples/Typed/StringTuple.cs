using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct StringTuple : ISpaceTuple<char>, IEquatable<StringTuple>, IComparable<StringTuple>
{
    private readonly string[] fields;

    public ref readonly string this[int index] => ref fields[index];
    public int Length => fields.Length;

    ref readonly char ISpaceTuple<char>.this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            for (int i = 0; i < Length; i++)
            {
                if (index < fields[i].Length)
                {
                    ReadOnlySpan<char> span = fields[i].AsSpan();

                    ref char firstItem = ref MemoryMarshal.GetReference(span);
                    ref char item = ref Unsafe.Add(ref firstItem, index);

                    return ref item;
                }
                else
                {
                    index -= fields[i].Length;
                }
            }

            return ref Unsafe.NullRef<char>();
        }
    }
    int ISpaceTuple<char>.Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            int charLength = 0;

            for (int i = 0; i < Length; i++)
            {
                charLength += fields[i].Length;
            }

            return charLength;
        }
    }

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

        int tupleLength = Length;
        int capacity = 0;

        for (int i = 0; i < tupleLength; i++)
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
        int maxFieldCharLength = 0;

        SFString[] sfStrings = new SFString[tupleLength];
        for (int i = 0; i < tupleLength; i++)
        {
            sfStrings[i] = new(this[i]);
            if (sfStrings[i].Value.Length > maxFieldCharLength)
            {
                maxFieldCharLength = sfStrings[i].Value.Length;
            }
        }

        return new SFStringTuple(sfStrings).AsSpan(maxFieldCharLength);
    }

    public ReadOnlySpan<string>.Enumerator GetEnumerator() => new ReadOnlySpan<string>(fields).GetEnumerator();

    readonly record struct SFStringTuple(params SFString[] Values) : ISpaceTuple<SFString>
    {
        public ref readonly SFString this[int index] => ref Values[index];
        public int Length => Values.Length;

        public static ReadOnlySpan<char> AsSpan() => ReadOnlySpan<char>.Empty;
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

[Immutable]
public readonly struct StringTemplate : ISpaceTemplate<char>
{
    private readonly string?[] fields;

    public ref readonly string? this[int index] => ref fields[index];
    public int Length => fields.Length;

    ref readonly char? ISpaceTemplate<char>.this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            for (int i = 0; i < Length; i++)
            {
                if (fields[i] is { } field)
                {
                    if (index < field.Length)
                    {
                        ReadOnlySpan<char> span = field.AsSpan();

                        ref char firstItem = ref MemoryMarshal.GetReference(span);
                        ref char item = ref Unsafe.Add(ref firstItem, index);

                        return ref Unsafe.NullRef<char?>();

                        //return ref item;
                    }
                    else
                    {
                        index -= field.Length;
                    }
                }
            }

            throw new IndexOutOfRangeException();
        }
    }
    int ISpaceTemplate<char>.Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            int charLength = 0;

            for (int i = 0; i < Length; i++)
            {
                charLength += fields[i]?.Length ?? 0;
            }

            return charLength;
        }
    }

    public StringTemplate([AllowNull] params string?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new string?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<char>
        => Helpers.Matches(this, tuple);

    ISpaceTuple<char> ISpaceTemplate<char>.Create(char[] fields)
    {
        return new StringTuple("aaa");
    }
}