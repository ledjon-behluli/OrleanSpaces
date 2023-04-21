using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct TimeSpanTuple : ISpaceTuple<TimeSpan, TimeSpanTuple>, ITupleFieldFormater
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
      => this.TryFormatTuple(this, destination, out charsWritten);

    public bool TryFormat(int index, Span<char> destination, out int charsWritten)
        => this.TryFormatTupleField(this, index, destination, out charsWritten);

    int ITupleFieldFormater.MaxCharsWrittable => 11;  //TODO: Fix

    bool ITupleFieldFormater.TryFormat(int index, Span<char> destination, out int charsWritten)
        => fields[index].TryFormat(destination, out charsWritten);

    public override string ToString() => $"({string.Join(", ", fields)})";
}