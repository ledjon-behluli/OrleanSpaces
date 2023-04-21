using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct SByteTuple : INumericTuple<sbyte, SByteTuple>, ITupleFieldFormater
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
    public bool Equals(SByteTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(SByteTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public bool TryFormat(Span<char> destination, out int charsWritten)
       => this.TryFormatTuple(this, destination, out charsWritten);

    public bool TryFormat(int index, Span<char> destination, out int charsWritten)
        => this.TryFormatTupleField(this, index, destination, out charsWritten);

    Span<sbyte> INumericTuple<sbyte, SByteTuple>.Fields => fields.AsSpan();

    int ITupleFieldFormater.MaxCharsWrittable => 11;  //TODO: Fix

    bool ITupleFieldFormater.TryFormat(int index, Span<char> destination, out int charsWritten)
        => fields[index].TryFormat(destination, out charsWritten);

    public override string ToString() => $"({string.Join(", ", fields)})";
}
