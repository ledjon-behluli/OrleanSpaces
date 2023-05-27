using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct UShortTuple : INumericTuple<ushort>, IEquatable<UShortTuple>, IComparable<UShortTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>65535</example>
    internal const int MaxFieldCharLength = 5;

    private readonly ushort[] fields;

    public ref readonly ushort this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<ushort> INumericTuple<ushort>.Fields => fields.AsSpan();

    public UShortTuple() : this(Array.Empty<ushort>()) { }
    public UShortTuple(params ushort[] fields) => this.fields = fields;

    public static bool operator ==(UShortTuple left, UShortTuple right) => left.Equals(right);
    public static bool operator !=(UShortTuple left, UShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UShortTuple tuple && Equals(tuple);
    public bool Equals(UShortTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(UShortTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<ushort>.Enumerator GetEnumerator() => new ReadOnlySpan<ushort>(fields).GetEnumerator();
}

[Immutable]
public readonly struct SpaceUShort
{
    public readonly ushort Value;

    internal static readonly SpaceUShort Default = new();

    public SpaceUShort(ushort value) => Value = value;

    public static implicit operator SpaceUShort(ushort value) => new(value);
    public static implicit operator ushort(SpaceUShort value) => value.Value;
}

[Immutable]
public readonly struct UShortTemplate : ISpaceTemplate<UShortTuple>
{
    private readonly SpaceUShort[] fields;

    public UShortTemplate([AllowNull] params SpaceUShort[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new SpaceUShort[1] { new SpaceUnit() } : fields;
}
