using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct FloatTuple : INumericTuple<float, FloatTuple>, ITupleFieldFormater
{
    private readonly float[] fields;

    public float this[int index] => fields[index];
    public Span<float> Fields => fields.AsSpan();
    public int Length => fields.Length;

    public FloatTuple() : this(Array.Empty<float>()) { }
    public FloatTuple(params float[] fields) => this.fields = fields;

    public static bool operator ==(FloatTuple left, FloatTuple right) => left.Equals(right);
    public static bool operator !=(FloatTuple left, FloatTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is FloatTuple tuple && Equals(tuple);
    public bool Equals(FloatTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(FloatTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => this.TryFormatTuple(this, destination, out charsWritten);

    public bool TryFormat(int index, Span<char> destination, out int charsWritten)
        => this.TryFormatTupleField(this, index, destination, out charsWritten);

    Span<float> INumericTuple<float, FloatTuple>.Fields => fields.AsSpan();

    int ITupleFieldFormater.MaxCharsWrittable => 11;  //TODO: Fix

    bool ITupleFieldFormater.TryFormat(int index, Span<char> destination, out int charsWritten)
        => fields[index].TryFormat(destination, out charsWritten);

    public override string ToString() => $"({string.Join(", ", fields)})";
}
