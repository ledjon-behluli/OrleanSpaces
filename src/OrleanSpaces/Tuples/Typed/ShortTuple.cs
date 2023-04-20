using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct ShortTuple : INumericSpaceTuple<short, ShortTuple>
{
    private readonly short[] fields;

    public short this[int index] => fields[index];
    public int Length => fields.Length;

    public ShortTuple() : this(Array.Empty<short>()) { }
    public ShortTuple(params short[] fields) => this.fields = fields;

    public ReadOnlySpan<short> AsSpan() => fields.AsSpan();

    public static bool operator ==(ShortTuple left, ShortTuple right) => left.Equals(right);
    public static bool operator !=(ShortTuple left, ShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ShortTuple tuple && Equals(tuple);
    public bool Equals(ShortTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(ShortTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => this.ToTupleString();
}
