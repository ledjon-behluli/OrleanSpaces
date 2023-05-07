using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct LongTuple : INumericTuple<long, LongTuple>, ISpanFormattable
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-9223372036854775808</example>
    internal const int MaxFieldCharLength = 20;

    private readonly long[] fields;

    public ref readonly long this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<long> INumericTuple<long, LongTuple>.Fields => fields.AsSpan();

    public LongTuple() : this(Array.Empty<long>()) { }
    public LongTuple(params long[] fields) => this.fields = fields;

    public static bool operator ==(LongTuple left, LongTuple right) => left.Equals(right);
    public static bool operator !=(LongTuple left, LongTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is LongTuple tuple && Equals(tuple);
    public bool Equals(LongTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(LongTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => this.TryFormat(MaxFieldCharLength, destination, out charsWritten);

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public ReadOnlySpan<long>.Enumerator GetEnumerator() => new ReadOnlySpan<long>(fields).GetEnumerator();
}
