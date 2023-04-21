using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct ShortTuple : INumericTuple<short, ShortTuple>, ITupleFieldFormater
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
      => this.TryFormatTuple(this, destination, out charsWritten);

    public bool TryFormat(int index, Span<char> destination, out int charsWritten)
        => this.TryFormatTupleField(this, index, destination, out charsWritten);

    Span<short> INumericTuple<short, ShortTuple>.Fields => fields.AsSpan();

    int ITupleFieldFormater.MaxCharsWrittable => 11;  //TODO: Fix

    bool ITupleFieldFormater.TryFormat(int index, Span<char> destination, out int charsWritten)
        => fields[index].TryFormat(destination, out charsWritten);

    public override string ToString() => $"({string.Join(", ", fields)})";
}
