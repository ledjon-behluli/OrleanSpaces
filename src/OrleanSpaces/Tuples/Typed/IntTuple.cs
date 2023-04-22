using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct IntTuple : INumericTuple<int, IntTuple>, IFieldFormater<int>
{
    private readonly int[] fields;

    public int this[int index] => fields[index];
    public int Length => fields.Length;

    public IntTuple() : this(Array.Empty<int>()) { }
    public IntTuple(params int[] fields) => this.fields = fields;

    public static bool operator ==(IntTuple left, IntTuple right) => left.Equals(right);
    public static bool operator !=(IntTuple left, IntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is IntTuple tuple && Equals(tuple);
    public bool Equals(IntTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(IntTuple other) => Length.CompareTo(other.Length);
    public override int GetHashCode() => fields.GetHashCode();

    public bool TryFormat(Span<char> destination, out int charsWritten) 
        => this.TryFormatTuple(destination, out charsWritten);

    Span<int> INumericTuple<int, IntTuple>.Fields => fields.AsSpan();

    static int IFieldFormater<int>.MaxCharsWrittable => 11;

    static bool IFieldFormater<int>.TryFormat(int field, Span<char> destination, out int charsWritten)
        => field.TryFormat(destination, out charsWritten);

    public override string ToString() => $"({string.Join(", ", fields)})";
}