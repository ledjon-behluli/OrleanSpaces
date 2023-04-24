using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct SByteTuple : IVectorizableValueTuple<sbyte, SByteTuple>, ISpanFormattable
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-128</example>
    internal const int MaxFieldCharLength = 4;

    private readonly sbyte[] fields;

    public ref readonly sbyte this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<sbyte> IVectorizableValueTuple<sbyte, SByteTuple>.Fields => fields.AsSpan();

    public SByteTuple() : this(Array.Empty<sbyte>()) { }
    public SByteTuple(params sbyte[] fields) => this.fields = fields;

    public static bool operator ==(SByteTuple left, SByteTuple right) => left.Equals(right);
    public static bool operator !=(SByteTuple left, SByteTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is SByteTuple tuple && Equals(tuple);
    public bool Equals(SByteTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(SByteTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => this.TryFormatTuple(MaxFieldCharLength, destination, out charsWritten);

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => TryFormat(destination, out charsWritten);

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();
}
