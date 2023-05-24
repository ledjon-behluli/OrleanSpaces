using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct ULongTuple : INumericTuple<ulong, ULongTuple>, IEquatable<ULongTuple>, IComparable<ULongTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>18446744073709551615</example>
    internal const int MaxFieldCharLength = 20;

    private readonly ulong[] fields;

    public ref readonly ulong this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<ulong> INumericTuple<ulong, ULongTuple>.Fields => fields.AsSpan();

    public ULongTuple() : this(Array.Empty<ulong>()) { }
    public ULongTuple(params ulong[] fields) => this.fields = fields;

    public static bool operator ==(ULongTuple left, ULongTuple right) => left.Equals(right);
    public static bool operator !=(ULongTuple left, ULongTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ULongTuple tuple && Equals(tuple);
    public bool Equals(ULongTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(ULongTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<ulong>.Enumerator GetEnumerator() => new ReadOnlySpan<ulong>(fields).GetEnumerator();
}
