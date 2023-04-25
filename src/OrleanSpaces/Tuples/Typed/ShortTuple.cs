using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct ShortTuple : INumericValueTuple<short, ShortTuple>, ISpanFormattable
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-32768</example>
    internal const int MaxFieldCharLength = 6;

    private readonly short[] fields;

    public ref readonly short this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<short> INumericValueTuple<short, ShortTuple>.Fields => fields.AsSpan();

    public ShortTuple() : this(Array.Empty<short>()) { }
    public ShortTuple(params short[] fields) => this.fields = fields;

    public static bool operator ==(ShortTuple left, ShortTuple right) => left.Equals(right);
    public static bool operator !=(ShortTuple left, ShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ShortTuple tuple && Equals(tuple);
    public bool Equals(ShortTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(ShortTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => this.TryFormatTuple(MaxFieldCharLength, destination, out charsWritten);

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => TryFormat(destination, out charsWritten);

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();
}
