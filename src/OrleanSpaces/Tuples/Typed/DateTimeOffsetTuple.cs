using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct DateTimeOffsetTuple : ISpaceTuple<DateTimeOffset, DateTimeOffsetTuple>, ITupleFieldFormater<DateTimeOffset>
{
    private readonly DateTimeOffset[] fields;

    public DateTimeOffset this[int index] => fields[index];
    public int Length => fields.Length;

    public DateTimeOffsetTuple() : this(Array.Empty<DateTimeOffset>()) { }
    public DateTimeOffsetTuple(params DateTimeOffset[] fields) => this.fields = fields;

    public static bool operator ==(DateTimeOffsetTuple left, DateTimeOffsetTuple right) => left.Equals(right);
    public static bool operator !=(DateTimeOffsetTuple left, DateTimeOffsetTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is DateTimeOffsetTuple tuple && Equals(tuple);

    public bool Equals(DateTimeOffsetTuple other)
    {
        NumericMarshaller<DateTimeOffset, long> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public int CompareTo(DateTimeOffsetTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => this.TryFormatTuple(destination, out charsWritten);

    public bool TryFormat(int index, Span<char> destination, out int charsWritten)
        => this.TryFormatTupleField(index, destination, out charsWritten);

    static int ITupleFieldFormater<DateTimeOffset>.MaxCharsWrittable => 11;

    static bool ITupleFieldFormater<DateTimeOffset>.TryFormat(DateTimeOffset field, Span<char> destination, out int charsWritten)
        => field.TryFormat(destination, out charsWritten);

    public override string ToString() => $"({string.Join(", ", fields)})";
}
