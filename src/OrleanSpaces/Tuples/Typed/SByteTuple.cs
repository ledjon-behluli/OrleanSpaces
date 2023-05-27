using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct SByteTuple : INumericTuple<sbyte>, IEquatable<SByteTuple>, IComparable<SByteTuple>
{
    private readonly sbyte[] fields;

    public ref readonly sbyte this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<sbyte> INumericTuple<sbyte>.Fields => fields.AsSpan();

    public SByteTuple() : this(Array.Empty<sbyte>()) { }
    public SByteTuple(params sbyte[] fields) => this.fields = fields;

    public static bool operator ==(SByteTuple left, SByteTuple right) => left.Equals(right);
    public static bool operator !=(SByteTuple left, SByteTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is SByteTuple tuple && Equals(tuple);
    public bool Equals(SByteTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(SByteTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_SByte);

    public ReadOnlySpan<sbyte>.Enumerator GetEnumerator() => new ReadOnlySpan<sbyte>(fields).GetEnumerator();
}

[Immutable]
public readonly struct SpaceSByte
{
    public readonly sbyte Value;

    internal static readonly SpaceSByte Default = new();

    public SpaceSByte(sbyte value) => Value = value;

    public static implicit operator SpaceSByte(sbyte value) => new(value);
    public static implicit operator sbyte(SpaceSByte value) => value.Value;
}

[Immutable]
public readonly struct SByteTemplate : ISpaceTemplate<SByteTuple>
{
    private readonly SpaceSByte[] fields;

    public SByteTemplate([AllowNull] params SpaceSByte[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new SpaceSByte[1] { new SpaceUnit() } : fields;
}

