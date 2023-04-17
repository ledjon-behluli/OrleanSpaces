using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct FloatTuple : INumericSpaceTuple<float, FloatTuple>
{
    private readonly float[] fields;

    public float this[int index] => fields[index];
    public int Length => fields.Length;

    Span<float> INumericSpaceTuple<float, FloatTuple>.Data => fields.AsSpan();

    public FloatTuple() : this(Array.Empty<float>()) { }
    public FloatTuple(params float[] fields) => this.fields = fields;

    public static bool operator ==(FloatTuple left, FloatTuple right) => left.Equals(right);
    public static bool operator !=(FloatTuple left, FloatTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is FloatTuple tuple && Equals(tuple);
    public bool Equals(FloatTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(FloatTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}
