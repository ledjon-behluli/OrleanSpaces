using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct UShortTuple : INumericTuple<ushort, UShortTuple>, ITupleFieldFormater<ushort>
{
    private readonly ushort[] fields;

    public ushort this[int index] => fields[index];
    public int Length => fields.Length;

    public UShortTuple() : this(Array.Empty<ushort>()) { }
    public UShortTuple(params ushort[] fields) => this.fields = fields;

    public static bool operator ==(UShortTuple left, UShortTuple right) => left.Equals(right);
    public static bool operator !=(UShortTuple left, UShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UShortTuple tuple && Equals(tuple);
    public bool Equals(UShortTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(UShortTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public bool TryFormat(Span<char> destination, out int charsWritten)
     => this.TryFormatTuple(destination, out charsWritten);

    public bool TryFormat(int index, Span<char> destination, out int charsWritten)
        => this.TryFormatTupleField(index, destination, out charsWritten);

    Span<ushort> INumericTuple<ushort, UShortTuple>.Fields => fields.AsSpan();

    static int ITupleFieldFormater<ushort>.MaxCharsWrittable => 11;

    static bool ITupleFieldFormater<ushort>.TryFormat(ushort field, Span<char> destination, out int charsWritten)
        => field.TryFormat(destination, out charsWritten);

    public override string ToString() => $"({string.Join(", ", fields)})";
}
