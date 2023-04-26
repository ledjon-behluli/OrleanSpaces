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

            if (Length == 1)
            {
                return fields[0] == other.fields[0];
            }

            NumericMarshaller<char, ushort> marshaller = new(ToCharSpan(fields), ToCharSpan(other.fields));
            return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Span<char> ToCharSpan(string[] array)
    {
        ref string firstString = ref MemoryMarshal.GetArrayDataReference(array);
        ref char firstChar = ref MemoryMarshal.GetReference(firstString.AsSpan());

        return MemoryMarshal.CreateSpan(ref firstChar, 111111);
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