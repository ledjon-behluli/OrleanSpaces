using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct UInt32Tuple : INumericTuple<uint, UInt32Tuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>4294967295</example>
    internal const int MaxFieldCharLength = 10;

    private readonly uint[] fields;

    public ref readonly uint this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<uint> INumericTuple<uint, UInt32Tuple>.Fields => fields.AsSpan();

    public UInt32Tuple() : this(Array.Empty<uint>()) { }
    public UInt32Tuple(params uint[] fields) => this.fields = fields;

    public static bool operator ==(UInt32Tuple left, UInt32Tuple right) => left.Equals(right);
    public static bool operator !=(UInt32Tuple left, UInt32Tuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UInt32Tuple tuple && Equals(tuple);
    public bool Equals(UInt32Tuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(UInt32Tuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<uint>.Enumerator GetEnumerator() => new ReadOnlySpan<uint>(fields).GetEnumerator();
}
