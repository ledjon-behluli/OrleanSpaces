using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct UIntTuple : INumericTuple<uint>, IEquatable<UIntTuple>, IComparable<UIntTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>4294967295</example>
    internal const int MaxFieldCharLength = 10;

    private readonly uint[] fields;

    public ref readonly uint this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<uint> INumericTuple<uint>.Fields => fields.AsSpan();

    public UIntTuple() : this(Array.Empty<uint>()) { }
    public UIntTuple(params uint[] fields) => this.fields = fields;

    public static bool operator ==(UIntTuple left, UIntTuple right) => left.Equals(right);
    public static bool operator !=(UIntTuple left, UIntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UIntTuple tuple && Equals(tuple);
    public bool Equals(UIntTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(UIntTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<uint>.Enumerator GetEnumerator() => new ReadOnlySpan<uint>(fields).GetEnumerator();
}

[Immutable]
public readonly struct SpaceUInt
{
    public readonly uint Value;

    internal static readonly SpaceUInt Default = new();

    public SpaceUInt(uint value) => Value = value;

    public static implicit operator SpaceUInt(uint value) => new(value);
    public static implicit operator uint(SpaceUInt value) => value.Value;
}

[Immutable]
public readonly struct UIntTemplate : ISpaceTemplate<UIntTuple>
{
    private readonly SpaceUInt[] fields;

    public UIntTemplate([AllowNull] params SpaceUInt[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new SpaceUInt[1] { new SpaceUnit() } : fields;
}
