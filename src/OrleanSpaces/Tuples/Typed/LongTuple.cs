using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct LongTuple : INumericSpaceTuple<long, LongTuple>
{
    private readonly long[] fields;

    public long this[int index] => fields[index];
    public int Length => fields.Length;

    Span<long> INumericSpaceTuple<long, LongTuple>.Data => fields.AsSpan();

    public LongTuple() : this(Array.Empty<long>()) { }
    public LongTuple(params long[] fields) => this.fields = fields;

    public static bool operator ==(LongTuple left, LongTuple right) => left.Equals(right);
    public static bool operator !=(LongTuple left, LongTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is LongTuple tuple && Equals(tuple);
    public bool Equals(LongTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(LongTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}
