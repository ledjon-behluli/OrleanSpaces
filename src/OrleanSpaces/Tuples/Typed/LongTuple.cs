using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct LongTuple : INumericTuple<long, LongTuple>, ITupleFieldFormater<long>
{
    private readonly long[] fields;

    public long this[int index] => fields[index];
    public int Length => fields.Length;
   
    public LongTuple() : this(Array.Empty<long>()) { }
    public LongTuple(params long[] fields) => this.fields = fields;

    public static bool operator ==(LongTuple left, LongTuple right) => left.Equals(right);
    public static bool operator !=(LongTuple left, LongTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is LongTuple tuple && Equals(tuple);
    public bool Equals(LongTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(LongTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public bool TryFormat(Span<char> destination, out int charsWritten)
       => this.TryFormatTuple(destination, out charsWritten);

    public bool TryFormat(int index, Span<char> destination, out int charsWritten)
        => this.TryFormatTupleField(index, destination, out charsWritten);

    Span<long> INumericTuple<long, LongTuple>.Fields => fields.AsSpan();

    static int ITupleFieldFormater<long>.MaxCharsWrittable => 11;

    static bool ITupleFieldFormater<long>.TryFormat(long field, Span<char> destination, out int charsWritten)
        => field.TryFormat(destination, out charsWritten);

    public override string ToString() => $"({string.Join(", ", fields)})";
}
