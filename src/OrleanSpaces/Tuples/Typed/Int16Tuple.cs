using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct Int16Tuple : INumericTuple<short, Int16Tuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-32768</example>
    internal const int MaxFieldCharLength = 6;

    private readonly short[] fields;

    public ref readonly short this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<short> INumericTuple<short, Int16Tuple>.Fields => fields.AsSpan();

    public Int16Tuple() : this(Array.Empty<short>()) { }
    public Int16Tuple(params short[] fields) => this.fields = fields;

    public static bool operator ==(Int16Tuple left, Int16Tuple right) => left.Equals(right);
    public static bool operator !=(Int16Tuple left, Int16Tuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is Int16Tuple tuple && Equals(tuple);
    public bool Equals(Int16Tuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(Int16Tuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<short>.Enumerator GetEnumerator() => new ReadOnlySpan<short>(fields).GetEnumerator();
}