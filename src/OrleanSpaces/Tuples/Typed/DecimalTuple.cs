using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct DecimalTuple : INumericSpaceTuple<decimal, DecimalTuple>
{
    private readonly decimal[] fields;

    public decimal this[int index] => fields[index];
    public int Length => fields.Length;

    Span<decimal> INumericSpaceTuple<decimal, DecimalTuple>.Data => fields.AsSpan();

    public DecimalTuple() : this(Array.Empty<decimal>()) { }
    public DecimalTuple(params decimal[] fields) => this.fields = fields;

    public static bool operator ==(DecimalTuple left, DecimalTuple right) => left.Equals(right);
    public static bool operator !=(DecimalTuple left, DecimalTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is DecimalTuple tuple && Equals(tuple);
    public bool Equals(DecimalTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(DecimalTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}
