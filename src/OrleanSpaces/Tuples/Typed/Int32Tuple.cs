using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct Int32Tuple : INumericTuple<int, Int32Tuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-2147483648</example>
    internal const int MaxFieldCharLength = 11;

    private readonly int[] fields;

    public ref readonly int this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<int> INumericTuple<int, Int32Tuple>.Fields => fields.AsSpan();

    public Int32Tuple() : this(Array.Empty<int>()) { }
    public Int32Tuple(params int[] fields) => this.fields = fields;

    public static bool operator ==(Int32Tuple left, Int32Tuple right) => left.Equals(right);
    public static bool operator !=(Int32Tuple left, Int32Tuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is Int32Tuple tuple && Equals(tuple);
    public bool Equals(Int32Tuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(Int32Tuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<int>.Enumerator GetEnumerator() => new ReadOnlySpan<int>(fields).GetEnumerator();
}