using Orleans.Concurrency;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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

    public override bool Equals(object? obj) => obj is StringTuple tuple && Equals(tuple);

    public bool Equals(StringTuple other)
    {
        if (Vector.IsHardwareAccelerated)
        {
            if (Length != other.Length)
            {
                return false;
            }

            //if (Length == 1)
            //{
            //    return fields[0] == other.fields[0];
            //}

            int totalCharLength = 0;

            for (int i = 0; i < Length; i++)
            {
                int thisCharLength = fields[i].Length;
                int otherCharLength = other.fields[i].Length;

                if (thisCharLength != otherCharLength)
                {
                    // If the number of chars found in 'fields[i]' and 'other.fields[i]' are different, we dont need to perform any further equality checks
                    // as its evident that the tuples are different. For example: ("a", "b", "c") != ("a", "bb", "c")

                    return false;
                }

                totalCharLength += thisCharLength;
            }

            Span<char> thisSpan = stackalloc char[totalCharLength];
            Span<char> otherSpan = stackalloc char[totalCharLength];

            ref string thisFirstItemRef = ref MemoryMarshal.GetArrayDataReference(fields);
            ref string otherFirstItemRef = ref MemoryMarshal.GetArrayDataReference(other.fields);

            int cursor = 0;

            for (int i = 0; i < Length; i++)
            {
                ReadOnlySpan<char> thisFieldSpan = Unsafe.Add(ref thisFirstItemRef, i).AsSpan();
                ReadOnlySpan<char> otherFieldSpan = Unsafe.Add(ref otherFirstItemRef, i).AsSpan();

                int spanLength = thisFieldSpan.Length;

                thisFieldSpan.CopyTo(thisSpan.Slice(cursor, spanLength));
                otherFieldSpan.CopyTo(otherSpan.Slice(cursor, spanLength));

                cursor += spanLength;
            }

            return new NumericMarshaller<char, ushort>(thisSpan, otherSpan).ParallelEquals();
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

    public ReadOnlySpan<string>.Enumerator GetEnumerator() => new ReadOnlySpan<string>(fields).GetEnumerator();
}