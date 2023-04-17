using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct ULongTuple : INumericSpaceTuple<ulong, ULongTuple>
{
    private readonly ulong[] fields;

    public ulong this[int index] => fields[index];
    public int Length => fields.Length;

    Span<ulong> INumericSpaceTuple<ulong, ULongTuple>.Data => fields.AsSpan();

    public ULongTuple() : this(Array.Empty<ulong>()) { }
    public ULongTuple(params ulong[] fields) => this.fields = fields;

    public static bool operator ==(ULongTuple left, ULongTuple right) => left.Equals(right);
    public static bool operator !=(ULongTuple left, ULongTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ULongTuple tuple && Equals(tuple);
    public bool Equals(ULongTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(ULongTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}
