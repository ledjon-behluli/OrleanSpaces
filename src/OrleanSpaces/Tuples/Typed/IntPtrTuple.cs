using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct IntPtrTuple : INumericTuple<nint, IntPtrTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-2147483648</example>
    internal const int MaxFieldCharLength = 11;

    private readonly nint[] fields;

    public ref readonly nint this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<nint> INumericTuple<nint, IntPtrTuple>.Fields => fields.AsSpan();

    public IntPtrTuple() : this(Array.Empty<nint>()) { }
    public IntPtrTuple(params nint[] fields) => this.fields = fields;

    public static bool operator ==(IntPtrTuple left, IntPtrTuple right) => left.Equals(right);
    public static bool operator !=(IntPtrTuple left, IntPtrTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is IntPtrTuple tuple && Equals(tuple);
    public bool Equals(IntPtrTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(IntPtrTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<nint>.Enumerator GetEnumerator() => new ReadOnlySpan<nint>(fields).GetEnumerator();
}