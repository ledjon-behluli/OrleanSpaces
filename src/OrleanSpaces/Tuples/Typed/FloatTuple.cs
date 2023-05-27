using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct FloatTuple : INumericTuple<float>, IEquatable<FloatTuple>, IComparable<FloatTuple>
{
    private readonly float[] fields;

    public ref readonly float this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<float> INumericTuple<float>.Fields => fields.AsSpan();

    public FloatTuple() : this(Array.Empty<float>()) { }
    public FloatTuple(params float[] fields) => this.fields = fields;

    public static bool operator ==(FloatTuple left, FloatTuple right) => left.Equals(right);
    public static bool operator !=(FloatTuple left, FloatTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is FloatTuple tuple && Equals(tuple);
    public bool Equals(FloatTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(FloatTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Float);

    public ReadOnlySpan<float>.Enumerator GetEnumerator() => new ReadOnlySpan<float>(fields).GetEnumerator();
}

[Immutable]
public readonly struct SpaceFloat
{
    public readonly float Value;

    internal static readonly SpaceFloat Default = new();

    public SpaceFloat(float value) => Value = value;

    public static implicit operator SpaceFloat(float value) => new(value);
    public static implicit operator float(SpaceFloat value) => value.Value;
}

[Immutable]
public readonly struct FloatTemplate : ISpaceTemplate<FloatTuple>
{
    private readonly SpaceFloat[] fields;

    public FloatTemplate([AllowNull] params SpaceFloat[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new SpaceFloat[1] { new SpaceUnit() } : fields;
}
