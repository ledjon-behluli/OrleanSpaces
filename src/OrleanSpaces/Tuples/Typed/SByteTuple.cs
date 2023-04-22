using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct SByteTuple : INumericTuple<sbyte, SByteTuple>, IFieldFormater<sbyte>
{
    private readonly sbyte[] fields;

    public sbyte this[int index] => fields[index];
    public Span<sbyte> Fields => fields.AsSpan();
    public int Length => fields.Length;

    public SByteTuple() : this(Array.Empty<sbyte>()) { }
    public SByteTuple(params sbyte[] fields) => this.fields = fields;

    public static bool operator ==(SByteTuple left, SByteTuple right) => left.Equals(right);
    public static bool operator !=(SByteTuple left, SByteTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is SByteTuple tuple && Equals(tuple);
    public bool Equals(SByteTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(SByteTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => this.TryFormatTuple(destination, out charsWritten);

    Span<sbyte> INumericTuple<sbyte, SByteTuple>.Fields => fields.AsSpan();

    static int IFieldFormater<sbyte>.MaxCharsWrittable => 11;

    static bool IFieldFormater<sbyte>.TryFormat(sbyte field, Span<char> destination, out int charsWritten)
        => field.TryFormat(destination, out charsWritten);

    public override string ToString() => $"({string.Join(", ", fields)})";
}
