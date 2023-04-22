using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct TimeSpanTuple : ISpaceTuple<TimeSpan, TimeSpanTuple>, ITupleFieldFormater<TimeSpan>
{
    private readonly TimeSpan[] fields;

    public TimeSpan this[int index] => fields[index];
    public int Length => fields.Length;

    public TimeSpanTuple() : this(Array.Empty<TimeSpan>()) { }
    public TimeSpanTuple(params TimeSpan[] fields) => this.fields = fields;

    public static bool operator ==(TimeSpanTuple left, TimeSpanTuple right) => left.Equals(right);
    public static bool operator !=(TimeSpanTuple left, TimeSpanTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is TimeSpanTuple tuple && Equals(tuple);

    public bool Equals(TimeSpanTuple other)
    {
        NumericMarshaller<TimeSpan, long> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public int CompareTo(TimeSpanTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public bool TryFormat(Span<char> destination, out int charsWritten)
      => this.TryFormatTuple(destination, out charsWritten);

    public bool TryFormat(int index, Span<char> destination, out int charsWritten)
        => this.TryFormatTupleField(index, destination, out charsWritten);

    static int ITupleFieldFormater<TimeSpan>.MaxCharsWrittable => 11;

    static bool ITupleFieldFormater<TimeSpan>.TryFormat(TimeSpan field, Span<char> destination, out int charsWritten)
        => field.TryFormat(destination, out charsWritten);

    public override string ToString() => $"({string.Join(", ", fields)})";
}