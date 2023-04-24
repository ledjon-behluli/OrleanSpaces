using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct FloatTuple : IVectorizableValueTuple<float, FloatTuple>, ISpanFormattable
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-3.4028235E+38</example>
    internal const int MaxFieldCharLength = 24;

    private readonly float[] fields;

    public ref readonly float this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<float> IVectorizableValueTuple<float, FloatTuple>.Fields => fields.AsSpan();

    public FloatTuple() : this(Array.Empty<float>()) { }
    public FloatTuple(params float[] fields) => this.fields = fields;

    public static bool operator ==(FloatTuple left, FloatTuple right) => left.Equals(right);
    public static bool operator !=(FloatTuple left, FloatTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is FloatTuple tuple && Equals(tuple);
    public bool Equals(FloatTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(FloatTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => this.TryFormatTuple(MaxFieldCharLength, destination, out charsWritten);

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => TryFormat(destination, out charsWritten);

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();
}
