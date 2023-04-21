using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct IntTuple : INumericTuple<int, IntTuple>, ISpaceFormattable
{
    /// <summary>
    /// An <see cref="int"/> is a 32-bit signed integer, which means it can represent values ranging from -2,147,483,648 to 2,147,483,647 (inclusive).
    /// We need a minimum of 11 characters to represent the number, including a possible negative sign.
    /// </summary>
    internal const int MaxCharsWrittable = 11;

    private readonly int[] fields;

    public int this[int index] => fields[index];
    public int Length => fields.Length;

    Span<int> INumericTuple<int, IntTuple>.Fields => fields.AsSpan();

    public IntTuple() : this(Array.Empty<int>()) { }
    public IntTuple(params int[] fields) => this.fields = fields;

    public static bool operator ==(IntTuple left, IntTuple right) => left.Equals(right);
    public static bool operator !=(IntTuple left, IntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is IntTuple tuple && Equals(tuple);
    public bool Equals(IntTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(IntTuple other) => Length.CompareTo(other.Length);
    public override int GetHashCode() => fields.GetHashCode();

    public bool TryFormat(Span<char> destination, out int charsWritten) => this.TryFormat(destination, out charsWritten);
    //{
    //    SpanFormatProps props = new(Length, MaxCharsWrittable);
    //    if (destination.Length < props.TotalLength)
    //    {
    //        charsWritten = 0;
    //        return false;
    //    }

    //    Span<char> span = stackalloc char[props.DestinationSpanLength];
    //    this.SpanFormat(span, in props, out charsWritten);

    //    span[..charsWritten].CopyTo(destination);

    //    return true;
    //}

    public bool TryFormat(int index, Span<char> destination, out int charsWritten)
    {
        charsWritten = 0;

        if (index < 0 || index >= fields.Length)
        {
            return false;
        }

        return fields[index].TryFormat(destination, out charsWritten);
    }

    public override string ToString() => $"({string.Join(", ", fields)})";
}