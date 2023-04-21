using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct IntTuple : INumericSpaceTuple<int, IntTuple>, ISpaceFormattable
{
    internal const int MaxCharsWrittable = 10;

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

    public bool TryFormat(Span<char> destination, out int charsWritten)
    {
        SpanFormatProps props = new(Length, MaxCharsWrittable);
        if (destination.Length < props.TotalLength)
        {
            charsWritten = 0;
            return false;
        }

        Span<char> span = stackalloc char[MaxCharsWrittable * Length];
        this.SpanFormat(this, span, in props, out charsWritten);

        span[..charsWritten].CopyTo(destination);

        return true;
    }

    public bool TryFormat(int index, Span<char> destination, out int charsWritten)
    {
        charsWritten = 0;

        if (index < 0 || index >= fields.Length)
        {
            return false;
        }

        return fields[index].TryFormat(destination, out charsWritten);
    }

    public override string ToString()
    {
        Span<char> span = stackalloc char[MaxCharsWrittable * Length];
        SpanFormatProps props = new(Length, MaxCharsWrittable);

        this.SpanFormat(this, span, in props, out int charsWritten);

        return span[..charsWritten].ToString();
    }
}