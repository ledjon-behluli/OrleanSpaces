using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct SingleTuple : INumericTuple<float, SingleTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-3.4028235E+38</example>
    internal const int MaxFieldCharLength = 24;

    private readonly float[] fields;

    public ref readonly float this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<float> INumericTuple<float, SingleTuple>.Fields => fields.AsSpan();

    public SingleTuple() : this(Array.Empty<float>()) { }
    public SingleTuple(params float[] fields) => this.fields = fields;

    public static bool operator ==(SingleTuple left, SingleTuple right) => left.Equals(right);
    public static bool operator !=(SingleTuple left, SingleTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is SingleTuple tuple && Equals(tuple);
    public bool Equals(SingleTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(SingleTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<float>.Enumerator GetEnumerator() => new ReadOnlySpan<float>(fields).GetEnumerator();
}
