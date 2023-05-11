using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct IntTuple : INumericTuple<int, IntTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-2147483648</example>
    internal const int MaxFieldCharLength = 11;

    private readonly int[] fields;

    public ref readonly int this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<int> INumericTuple<int, IntTuple>.Fields => fields.AsSpan();

    public IntTuple() : this(Array.Empty<int>()) { }
    public IntTuple(params int[] fields) => this.fields = fields;

    public static bool operator ==(IntTuple left, IntTuple right) => left.Equals(right);
    public static bool operator !=(IntTuple left, IntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is IntTuple tuple && Equals(tuple);
    public bool Equals(IntTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(IntTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<int>.Enumerator GetEnumerator() => new ReadOnlySpan<int>(fields).GetEnumerator();
}