using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct BoolTuple : ISpaceTuple<bool, BoolTuple>, ITupleFieldFormater
{
    private readonly bool[] fields;

    public bool this[int index] => fields[index];
    public int Length => fields.Length;

    public BoolTuple() : this(Array.Empty<bool>()) { }
    public BoolTuple(params bool[] fields) => this.fields = fields;

    public static bool operator ==(BoolTuple left, BoolTuple right) => left.Equals(right);
    public static bool operator !=(BoolTuple left, BoolTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is BoolTuple tuple && Equals(tuple);

    public bool Equals(BoolTuple other)
    {
        NumericMarshaller<bool, byte> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public int CompareTo(BoolTuple other) => Length.CompareTo(other.Length);

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