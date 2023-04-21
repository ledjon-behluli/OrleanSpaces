using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct IntTuple : INumericSpaceTuple<int, IntTuple>, IFieldFormater, ISpanFormattable
{
    private readonly int[] fields;

    public int this[int index] => fields[index];
    public int Length => fields.Length;

    Span<int> INumericSpaceTuple<int, IntTuple>.Fields => fields.AsSpan();

    public IntTuple() : this(Array.Empty<int>()) { }
    public IntTuple(params int[] fields) => this.fields = fields;

    public static bool operator ==(IntTuple left, IntTuple right) => left.Equals(right);
    public static bool operator !=(IntTuple left, IntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is IntTuple tuple && Equals(tuple);
    public bool Equals(IntTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(IntTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

   

    private const int maxCharsWrittable = 10;

    void IFieldFormater.Format(int index, Span<char> destination, out int charsWritten)
        => fields[index].TryFormat(destination, out charsWritten);

    public override string ToString()
    {
        Span<char> span = stackalloc char[maxCharsWrittable * Length];
        SpanFormatProps props = new(Length, maxCharsWrittable);

        this.SpanFormat(this, span, in props, out int charsWritten);

        return span[..charsWritten].ToString();
    }

    public bool TryFormat(Span<char> destination, out int charsWritten)
    {
        SpanFormatProps props = new(Length, maxCharsWrittable);
        if (destination.Length < props.TotalLength)
        {
            charsWritten = 0;
            return false;
        }

        Span<char> span = stackalloc char[maxCharsWrittable * Length];
        this.SpanFormat(this, span, in props, out charsWritten);

        span[..charsWritten].CopyTo(destination);

        return true;
    }

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormat(destination, out charsWritten);
    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();
}