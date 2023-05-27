using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct ShortTuple : INumericTuple<short>, IEquatable<ShortTuple>, IComparable<ShortTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-32768</example>
    internal const int MaxFieldCharLength = 6;

    private readonly short[] fields;

    public ref readonly short this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<short> INumericTuple<short>.Fields => fields.AsSpan();

    public ShortTuple() : this(Array.Empty<short>()) { }
    public ShortTuple(params short[] fields) => this.fields = fields;

    public static bool operator ==(ShortTuple left, ShortTuple right) => left.Equals(right);
    public static bool operator !=(ShortTuple left, ShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ShortTuple tuple && Equals(tuple);
    public bool Equals(ShortTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(ShortTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<short>.Enumerator GetEnumerator() => new ReadOnlySpan<short>(fields).GetEnumerator();
}

[Immutable]
public readonly struct SpaceShort
{
    public readonly short Value;

    internal static readonly SpaceShort Default = new();

    public SpaceShort(short value) => Value = value;

    public static implicit operator SpaceShort(short value) => new(value);
    public static implicit operator short(SpaceShort value) => value.Value;
}

[Immutable]
public readonly struct ShortTemplate : ISpaceTemplate<ShortTuple>
{
    private readonly SpaceShort[] fields;

    public ShortTemplate([AllowNull] params SpaceShort[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new SpaceShort[1] { new SpaceUnit() } : fields;
}
