using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct UIntTuple : INumericTuple<uint, UIntTuple>
{
    private readonly uint[] fields;

    public uint this[int index] => fields[index];
    public Span<uint> Fields => fields.AsSpan();
    public int Length => fields.Length;

    Span<uint> INumericTuple<uint, UIntTuple>.Fields => fields.AsSpan();

    public UIntTuple() : this(Array.Empty<uint>()) { }
    public UIntTuple(params uint[] fields) => this.fields = fields;

    public static bool operator ==(UIntTuple left, UIntTuple right) => left.Equals(right);
    public static bool operator !=(UIntTuple left, UIntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UIntTuple tuple && Equals(tuple);
    public bool Equals(UIntTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(UIntTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => this.ToTupleString();
}
