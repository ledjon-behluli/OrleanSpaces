using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct UIntPtrTuple : INumericTuple<nuint, UIntPtrTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-2147483648</example>
    internal const int MaxFieldCharLength = 11;

    private readonly nuint[] fields;

    public ref readonly nuint this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<nuint> INumericTuple<nuint, UIntPtrTuple>.Fields => fields.AsSpan();

    public UIntPtrTuple() : this(Array.Empty<nuint>()) { }
    public UIntPtrTuple(params nuint[] fields) => this.fields = fields;

    public static bool operator ==(UIntPtrTuple left, UIntPtrTuple right) => left.Equals(right);
    public static bool operator !=(UIntPtrTuple left, UIntPtrTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UIntPtrTuple tuple && Equals(tuple);
    public bool Equals(UIntPtrTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(UIntPtrTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<nuint>.Enumerator GetEnumerator() => new ReadOnlySpan<nuint>(fields).GetEnumerator();
}