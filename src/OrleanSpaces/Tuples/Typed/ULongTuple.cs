using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct ULongTuple : INumericTuple<ulong, ULongTuple>, IFieldFormater<ulong>
{
    private readonly ulong[] fields;

    public ulong this[int index] => fields[index];
    public Span<ulong> Fields => fields.AsSpan();
    public int Length => fields.Length;

    public ULongTuple() : this(Array.Empty<ulong>()) { }
    public ULongTuple(params ulong[] fields) => this.fields = fields;

    public static bool operator ==(ULongTuple left, ULongTuple right) => left.Equals(right);
    public static bool operator !=(ULongTuple left, ULongTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ULongTuple tuple && Equals(tuple);
    public bool Equals(ULongTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(ULongTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => this.TryFormatTuple(destination, out charsWritten);

    Span<ulong> INumericTuple<ulong, ULongTuple>.Fields => fields.AsSpan();

    static int IFieldFormater<ulong>.MaxCharsWrittable => 11;

    static bool IFieldFormater<ulong>.TryFormat(ulong field, Span<char> destination, out int charsWritten)
        => field.TryFormat(destination, out charsWritten);

    public override string ToString() => $"({string.Join(", ", fields)})";
}
