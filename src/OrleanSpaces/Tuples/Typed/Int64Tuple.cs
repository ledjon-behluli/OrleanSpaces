using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct Int64Tuple : INumericTuple<long, Int64Tuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-9223372036854775808</example>
    internal const int MaxFieldCharLength = 20;

    private readonly long[] fields;

    public ref readonly long this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<long> INumericTuple<long, Int64Tuple>.Fields => fields.AsSpan();

    public Int64Tuple() : this(Array.Empty<long>()) { }
    public Int64Tuple(params long[] fields) => this.fields = fields;

    public static bool operator ==(Int64Tuple left, Int64Tuple right) => left.Equals(right);
    public static bool operator !=(Int64Tuple left, Int64Tuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is Int64Tuple tuple && Equals(tuple);
    public bool Equals(Int64Tuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(Int64Tuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<long>.Enumerator GetEnumerator() => new ReadOnlySpan<long>(fields).GetEnumerator();
}
