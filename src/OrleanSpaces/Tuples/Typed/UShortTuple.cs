using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct UShortTuple : INumericSpaceTuple<ushort, UShortTuple>
{
    private readonly ushort[] fields;

    public ushort this[int index] => fields[index];
    public int Length => fields.Length;

    Span<ushort> INumericSpaceTuple<ushort, UShortTuple>.Fields => fields.AsSpan();

    public UShortTuple() : this(Array.Empty<ushort>()) { }
    public UShortTuple(params ushort[] fields) => this.fields = fields;

    public static bool operator ==(UShortTuple left, UShortTuple right) => left.Equals(right);
    public static bool operator !=(UShortTuple left, UShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UShortTuple tuple && Equals(tuple);
    public bool Equals(UShortTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(UShortTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => this.ToTupleString();
}
