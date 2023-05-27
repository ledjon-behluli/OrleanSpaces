using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct ULongTuple : INumericTuple<ulong>, IEquatable<ULongTuple>, IComparable<ULongTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>18446744073709551615</example>
    internal const int MaxFieldCharLength = 20;

    private readonly ulong[] fields;

    public ref readonly ulong this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<ulong> INumericTuple<ulong>.Fields => fields.AsSpan();

    public ULongTuple() : this(Array.Empty<ulong>()) { }
    public ULongTuple(params ulong[] fields) => this.fields = fields;

    public static bool operator ==(ULongTuple left, ULongTuple right) => left.Equals(right);
    public static bool operator !=(ULongTuple left, ULongTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ULongTuple tuple && Equals(tuple);
    public bool Equals(ULongTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(ULongTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<ulong>.Enumerator GetEnumerator() => new ReadOnlySpan<ulong>(fields).GetEnumerator();
}

[Immutable]
public readonly struct SpaceULong
{
    public readonly ulong Value;

    internal static readonly SpaceULong Default = new();

    public SpaceULong(ulong value) => Value = value;

    public static implicit operator SpaceULong(ulong value) => new(value);
    public static implicit operator ulong(SpaceULong value) => value.Value;
}

[Immutable]
public readonly struct ULongTemplate : ISpaceTemplate<ULongTuple>
{
    private readonly SpaceULong[] fields;

    public ULongTemplate([AllowNull] params SpaceULong[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new SpaceULong[1] { new SpaceUnit() } : fields;
}
