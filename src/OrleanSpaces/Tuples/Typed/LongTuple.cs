using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct LongTuple : INumericTuple<long>, IEquatable<LongTuple>, IComparable<LongTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-9223372036854775808</example>
    internal const int MaxFieldCharLength = 20;

    private readonly long[] fields;

    public ref readonly long this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<long> INumericTuple<long>.Fields => fields.AsSpan();

    public LongTuple() : this(Array.Empty<long>()) { }
    public LongTuple(params long[] fields) => this.fields = fields;

    public static bool operator ==(LongTuple left, LongTuple right) => left.Equals(right);
    public static bool operator !=(LongTuple left, LongTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is LongTuple tuple && Equals(tuple);
    public bool Equals(LongTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(LongTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<long>.Enumerator GetEnumerator() => new ReadOnlySpan<long>(fields).GetEnumerator();
}

[Immutable]
public readonly struct SpaceLong
{
    public readonly long Value;

    internal static readonly SpaceLong Default = new();

    public SpaceLong(long value) => Value = value;

    public static implicit operator SpaceLong(long value) => new(value);
    public static implicit operator long(SpaceLong value) => value.Value;
}

[Immutable]
public readonly struct LongTemplate : ISpaceTemplate<LongTuple>
{
    private readonly SpaceLong[] fields;

    public LongTemplate([AllowNull] params SpaceLong[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new SpaceLong[1] { new SpaceUnit() } : fields;
}
