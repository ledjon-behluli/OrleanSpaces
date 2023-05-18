using Orleans.Concurrency;

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
