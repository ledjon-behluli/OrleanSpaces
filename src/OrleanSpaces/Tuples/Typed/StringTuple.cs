using Orleans.Concurrency;
using System.Numerics;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct StringTuple : IObjectTuple<string, StringTuple>, ITupleEqualityComparer<char, StringTuple>, ISpanFormattable
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>a</example>
    internal const int MaxFieldCharLength = 1; // TODO: Look at me!

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

            return Extensions.AreEqual<char, StringTuple, StringTuple>(slots, in this, in other);
        }

        return this.SequentialEquals(other);
    }

    public int CompareTo(StringTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public bool TryFormat(Span<char> destination, out int charsWritten)
    //  => this.TryFormatTuple(MaxFieldCharLength, destination, out charsWritten);
    {
        charsWritten = 0;
        return true;
    }
     
    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => TryFormat(destination, out charsWritten);

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

    static bool ITupleEqualityComparer<char, StringTuple>.Equals(StringTuple left, Span<char> leftSpan, StringTuple right, Span<char> rightSpan)
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

        return new NumericMarshaller<char, ushort>(leftSpan, rightSpan).ParallelEquals();  // See: CharTuple.AreEqual for more details
    }

    public ReadOnlySpan<string>.Enumerator GetEnumerator() => new ReadOnlySpan<string>(fields).GetEnumerator();
}