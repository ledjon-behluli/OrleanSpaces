using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct DoubleTuple : INumericTuple<double, DoubleTuple>, IFieldFormater<double>
{
    private readonly double[] fields;

    public double this[int index] => fields[index];
    public Span<double> Fields => fields.AsSpan();
    public int Length => fields.Length;

    public DoubleTuple() : this(Array.Empty<double>()) { }
    public DoubleTuple(params double[] fields) => this.fields = fields;

    public static bool operator ==(DoubleTuple left, DoubleTuple right) => left.Equals(right);
    public static bool operator !=(DoubleTuple left, DoubleTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is DoubleTuple tuple && Equals(tuple);
    public bool Equals(DoubleTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(DoubleTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => this.TryFormatTuple(destination, out charsWritten);

    Span<double> INumericTuple<double, DoubleTuple>.Fields => fields.AsSpan();

    static int IFieldFormater<double>.MaxCharsWrittable => 11;

    static bool IFieldFormater<double>.TryFormat(double field, Span<char> destination, out int charsWritten)
        => field.TryFormat(destination, out charsWritten);

    public override string ToString() => $"({string.Join(", ", fields)})";
}