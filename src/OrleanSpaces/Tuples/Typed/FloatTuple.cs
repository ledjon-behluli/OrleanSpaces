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
public readonly struct FloatTemplate : ISpaceTemplate<float>
{
    private readonly float?[] fields;

    public ref readonly float? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public FloatTemplate([AllowNull] params float?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new float?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<float>
        => Helpers.Matches(this, tuple);

    ISpaceTuple<float> ISpaceTemplate<float>.Create(float[] fields) => new FloatTuple(fields);
}
