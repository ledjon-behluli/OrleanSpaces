using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct UInt64Tuple : INumericTuple<ulong, UInt64Tuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>18446744073709551615</example>
    internal const int MaxFieldCharLength = 20;

    private readonly ulong[] fields;

    public ref readonly ulong this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<ulong> INumericTuple<ulong, UInt64Tuple>.Fields => fields.AsSpan();

    public UInt64Tuple() : this(Array.Empty<ulong>()) { }
    public UInt64Tuple(params ulong[] fields) => this.fields = fields;

    public static bool operator ==(UInt64Tuple left, UInt64Tuple right) => left.Equals(right);
    public static bool operator !=(UInt64Tuple left, UInt64Tuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UInt64Tuple tuple && Equals(tuple);
    public bool Equals(UInt64Tuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(UInt64Tuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<ulong>.Enumerator GetEnumerator() => new ReadOnlySpan<ulong>(fields).GetEnumerator();
}
