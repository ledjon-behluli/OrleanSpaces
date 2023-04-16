using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed.Numerics;

[Immutable]
public readonly struct SByteTuple : INumericTuple<sbyte, SByteTuple>
{
    private readonly sbyte[] fields;

    public sbyte this[int index] => fields[index];
    public int Length => fields.Length;

    Span<sbyte> INumericTuple<sbyte, SByteTuple>.Data => fields.AsSpan();

    public SByteTuple() : this(Array.Empty<sbyte>()) { }
    public SByteTuple(sbyte[] fields) => this.fields = fields;

    public static bool operator ==(SByteTuple left, SByteTuple right) => left.Equals(right);
    public static bool operator !=(SByteTuple left, SByteTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is SByteTuple tuple && Equals(tuple);
    public bool Equals(SByteTuple other) => this.ParallelEquals(other);

    public int CompareTo(SByteTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}
