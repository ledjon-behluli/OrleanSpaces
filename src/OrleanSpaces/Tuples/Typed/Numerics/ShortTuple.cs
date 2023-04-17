using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed.Numerics;

[Immutable]
public readonly struct ShortTuple : INumericSpaceTuple<short, ShortTuple>
{
    private readonly short[] fields;

    public short this[int index] => fields[index];
    public int Length => fields.Length;

    Span<short> INumericSpaceTuple<short, ShortTuple>.Data => fields.AsSpan();

    public ShortTuple() : this(Array.Empty<short>()) { }
    public ShortTuple(params short[] fields) => this.fields = fields;

    public static bool operator ==(ShortTuple left, ShortTuple right) => left.Equals(right);
    public static bool operator !=(ShortTuple left, ShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ShortTuple tuple && Equals(tuple);
    public bool Equals(ShortTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(ShortTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}
