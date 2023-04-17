using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed.Numerics;

[Immutable]
public readonly struct ByteTuple : INumericSpaceTuple<byte, ByteTuple>
{
    private readonly byte[] fields;

    public byte this[int index] => fields[index];
    public int Length => fields.Length;

    Span<byte> INumericSpaceTuple<byte, ByteTuple>.Data => fields.AsSpan();

    public ByteTuple() : this(Array.Empty<byte>()) { }
    public ByteTuple(params byte[] fields) => this.fields = fields;

    public static bool operator ==(ByteTuple left, ByteTuple right) => left.Equals(right);
    public static bool operator !=(ByteTuple left, ByteTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ByteTuple tuple && Equals(tuple);
    public bool Equals(ByteTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(ByteTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}
