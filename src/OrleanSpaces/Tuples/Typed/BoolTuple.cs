using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct BoolTuple : IValueTuple<bool, BoolTuple>, ISpanFormattable
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>False</example>
    internal const int MaxFieldCharLength = 5;

    private readonly bool[] fields;

    public ref readonly bool this[int index] => ref fields[index];
    public int Length => fields.Length;

    public BoolTuple() : this(Array.Empty<bool>()) { }
    public BoolTuple(params bool[] fields) => this.fields = fields;

    public static bool operator ==(BoolTuple left, BoolTuple right) => left.Equals(right);
    public static bool operator !=(BoolTuple left, BoolTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is BoolTuple tuple && Equals(tuple);

    public bool Equals(BoolTuple other)
    {
        NumericMarshaller<bool, byte> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public int CompareTo(BoolTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => TryFormat(destination, out charsWritten);

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public bool TryFormat(Span<char> destination, out int charsWritten)
    {
        // Since `bool` does not implement `ISpanFormattable`, we cant use `Extensions.TryFormatTuple` and are forced
        // to implement it here. See: https://github.com/dotnet/runtime/issues/67388

        charsWritten = 0;

        int tupleLength = Length;
        int totalLength = Extensions.CalculateTotalLength(tupleLength, MaxFieldCharLength);

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
        Span<char> fieldSpan = stackalloc char[MaxFieldCharLength];

        if (tupleLength == 1)
        {
            tupleSpan[charsWritten++] = '(';

            if (!TryFormatField(fields[0], tupleSpan, fieldSpan, ref charsWritten))
            {
                return false;
            }

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

                fieldSpan.Clear();
            }

            if (!TryFormatField(fields[i], tupleSpan, fieldSpan, ref charsWritten))
            {
                return false;
            }
        }

        tupleSpan[charsWritten++] = ')';
        tupleSpan[..(charsWritten + 1)].CopyTo(destination);

        return true;

        static bool TryFormatField(bool field, Span<char> tupleSpan, Span<char> fieldSpan, ref int charsWritten)
        {
            if (!field.TryFormat(fieldSpan, out int fieldCharsWritten))
            {
                charsWritten = 0;
                return false;
            }

            fieldSpan[..fieldCharsWritten].CopyTo(tupleSpan.Slice(charsWritten, fieldCharsWritten));
            charsWritten += fieldCharsWritten;

            return true;
        }
    }
}