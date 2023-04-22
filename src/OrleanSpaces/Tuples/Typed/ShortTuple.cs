using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct ShortTuple : INumericTuple<short, ShortTuple>, ITupleFieldFormater<short>
{
    private readonly short[] fields;

    public short this[int index] => fields[index];
    public Span<short> Fields => fields.AsSpan();
    public int Length => fields.Length;

    public ShortTuple() : this(Array.Empty<short>()) { }
    public ShortTuple(params short[] fields) => this.fields = fields;

    public static bool operator ==(ShortTuple left, ShortTuple right) => left.Equals(right);
    public static bool operator !=(ShortTuple left, ShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ShortTuple tuple && Equals(tuple);
    public bool Equals(ShortTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(ShortTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public bool TryFormat(Span<char> destination, out int charsWritten)
      => this.TryFormatTuple(destination, out charsWritten);

    public bool TryFormat(int index, Span<char> destination, out int charsWritten)
        => this.TryFormatTupleField(index, destination, out charsWritten);

    Span<short> INumericTuple<short, ShortTuple>.Fields => fields.AsSpan();

    static int ITupleFieldFormater<short>.MaxCharsWrittable => 11;

    static bool ITupleFieldFormater<short>.TryFormat(short field, Span<char> destination, out int charsWritten)
        => field.TryFormat(destination, out charsWritten);

    public override string ToString() => $"({string.Join(", ", fields)})";
}
