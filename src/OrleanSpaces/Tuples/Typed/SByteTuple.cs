using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct SByteTuple : INumericSpaceTuple<sbyte, SByteTuple>
{
    private readonly sbyte[] fields;

    public sbyte this[int index] => fields[index];
    public int Length => fields.Length;

    public SByteTuple() : this(Array.Empty<sbyte>()) { }
    public SByteTuple(params sbyte[] fields) => this.fields = fields;

    public ReadOnlySpan<sbyte> AsSpan() => fields.AsSpan();

    public static bool operator ==(SByteTuple left, SByteTuple right) => left.Equals(right);
    public static bool operator !=(SByteTuple left, SByteTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is SByteTuple tuple && Equals(tuple);
    public bool Equals(SByteTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(SByteTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => this.ToTupleString();
}
