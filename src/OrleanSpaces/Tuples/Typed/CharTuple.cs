using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct CharTuple : IValueTuple<char, CharTuple>, ISpanFormattable
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>a</example>
    internal const int MaxFieldCharLength = 1;

    private readonly char[] fields;

    public ref readonly char this[int index] => ref fields[index];
    public int Length => fields.Length;

    public CharTuple() : this(Array.Empty<char>()) { }
    public CharTuple(params char[] fields) => this.fields = fields;

    public static bool operator ==(CharTuple left, CharTuple right) => left.Equals(right);
    public static bool operator !=(CharTuple left, CharTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is CharTuple tuple && Equals(tuple);

    public bool Equals(CharTuple other)
    {
        //if (Vector.IsHardwareAccelerated)
        //{
        //    int low = Vector128.IsHardwareAccelerated

        //if (Vector256.IsHardwareAccelerated)
        //    {
        //        return Length switch
        //        {
        //            >= 16 => new NumericMarshaller<char, ushort>(fields.AsSpan(), other.fields.AsSpan()).TryParallelEquals(out bool result) ? result : this.SequentialEquals(other),
        //            >= 8 and < 16 => new NumericMarshaller<char, uint>(fields.AsSpan(), other.fields.AsSpan()).TryParallelEquals(out bool result) ? result : this.SequentialEquals(other),
        //            >= 4 and < 8 => new NumericMarshaller<char, ulong>(fields.AsSpan(), other.fields.AsSpan()).TryParallelEquals(out bool result) ? result : this.SequentialEquals(other),
        //            _ => this.SequentialEquals(other)
        //        };
        //    }
        //Vector64.IsHardwareAccelerated
        //    if (Vector128.IsHardwareAccelerated)
        //    {
        //        return Length switch
        //        {
        //            >= 8 => new NumericMarshaller<char, ushort>(fields.AsSpan(), other.fields.AsSpan()).TryParallelEquals(out bool result) ? result : this.SequentialEquals(other),
        //            >= 4 and < 8 => new NumericMarshaller<char, uint>(fields.AsSpan(), other.fields.AsSpan()).TryParallelEquals(out bool result) ? result : this.SequentialEquals(other),
        //            >= 2 and < 4 => new NumericMarshaller<char, ulong>(fields.AsSpan(), other.fields.AsSpan()).TryParallelEquals(out bool result) ? result : this.SequentialEquals(other),
        //            _ => this.SequentialEquals(other)
        //        };
        //    }
        //}

        // Since 'char' is not a number (doesn't implement INumber<T>), we are transforming it into a type which does implement INumber<T>.
        // The sizeof(char) = 2 Bytes, therefor it can be represented by many number types, but the lowest possible (the one that provides the best parallelization)
        // number type that can fully represent any type of 'char', is 'short/ushort'.

        // In systems where 128-bit vector operations are subject to hardware acceleration, a total of 8 operations can be performed on 'ushort's
        //      128 / (2 * 8) = 128 / 16 = 8  --> means: we can compare 8 chars at the same time!

        // In systems where 256-bit vector operations are subject to hardware acceleration, a total of 16 operations can be performed on 'ushort's
        //      256 / (2 * 8) = 256 / 16 = 16 --> means: we can compare 16 chars at the same time!


        NumericMarshaller<char, ushort> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public int CompareTo(CharTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => this.TryFormatTuple(MaxFieldCharLength, destination, out charsWritten);

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => TryFormat(destination, out charsWritten);

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();
}