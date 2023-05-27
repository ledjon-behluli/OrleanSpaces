using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct DoubleTuple : INumericTuple<double>, IEquatable<DoubleTuple>, IComparable<DoubleTuple>
{
    private readonly double[] fields;

    public ref readonly double this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<double> INumericTuple<double>.Fields => fields.AsSpan();

    public DoubleTuple() : this(Array.Empty<double>()) { }
    public DoubleTuple(params double[] fields) => this.fields = fields;

    public static bool operator ==(DoubleTuple left, DoubleTuple right) => left.Equals(right);
    public static bool operator !=(DoubleTuple left, DoubleTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is DoubleTuple tuple && Equals(tuple);
    public bool Equals(DoubleTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(DoubleTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Double);

    public ReadOnlySpan<double>.Enumerator GetEnumerator() => new ReadOnlySpan<double>(fields).GetEnumerator();
}

[Immutable]
public readonly struct SpaceDouble
{
    public readonly double Value;

    internal static readonly SpaceDouble Default = new();

    public SpaceDouble(double value) => Value = value;

    public static implicit operator SpaceDouble(double value) => new(value);
    public static implicit operator double(SpaceDouble value) => value.Value;
}

[Immutable]
public readonly struct DoubleTemplate : ISpaceTemplate<DoubleTuple>
{
    private readonly SpaceDouble[] fields;

    public DoubleTemplate([AllowNull] params SpaceDouble[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new SpaceDouble[1] { new SpaceUnit() } : fields;
}
