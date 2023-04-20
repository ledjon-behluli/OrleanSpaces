using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct DoubleTuple : INumericSpaceTuple<double, DoubleTuple>
{
    private readonly double[] fields;

    public double this[int index] => fields[index];
    public int Length => fields.Length;

    public DoubleTuple() : this(Array.Empty<double>()) { }
    public DoubleTuple(params double[] fields) => this.fields = fields;

    public static bool operator ==(DoubleTuple left, DoubleTuple right) => left.Equals(right);
    public static bool operator !=(DoubleTuple left, DoubleTuple right) => !(left == right);

    public ReadOnlySpan<double> AsSpan() => fields.AsSpan();

    public override bool Equals(object? obj) => obj is DoubleTuple tuple && Equals(tuple);
    public bool Equals(DoubleTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(DoubleTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => this.ToTupleString();
}