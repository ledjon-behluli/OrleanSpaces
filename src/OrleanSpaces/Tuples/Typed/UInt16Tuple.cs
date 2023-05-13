using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct UInt16Tuple : INumericTuple<ushort, UInt16Tuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>65535</example>
    internal const int MaxFieldCharLength = 5;

    private readonly ushort[] fields;

    public ref readonly ushort this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<ushort> INumericTuple<ushort, UInt16Tuple>.Fields => fields.AsSpan();

    public UInt16Tuple() : this(Array.Empty<ushort>()) { }
    public UInt16Tuple(params ushort[] fields) => this.fields = fields;

    public static bool operator ==(UInt16Tuple left, UInt16Tuple right) => left.Equals(right);
    public static bool operator !=(UInt16Tuple left, UInt16Tuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UInt16Tuple tuple && Equals(tuple);
    public bool Equals(UInt16Tuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(UInt16Tuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<ushort>.Enumerator GetEnumerator() => new ReadOnlySpan<ushort>(fields).GetEnumerator();
}
