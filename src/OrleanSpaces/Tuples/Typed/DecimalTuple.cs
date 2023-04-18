using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct DecimalTuple : ISpaceTuple<decimal, DecimalTuple>
{
    private readonly decimal[] fields;

    public decimal this[int index] => fields[index];
    public int Length => fields.Length;

    public DecimalTuple() : this(Array.Empty<decimal>()) { }
    public DecimalTuple(params decimal[] fields) => this.fields = fields;

    public static bool operator ==(DecimalTuple left, DecimalTuple right) => left.Equals(right);
    public static bool operator !=(DecimalTuple left, DecimalTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is DecimalTuple tuple && Equals(tuple);

    //TODO: Test if it works correctly
    public bool Equals(DecimalTuple other)
    {
        int totalInt32Length = 4 * Length;

        Span<int> thisSpan = stackalloc int[totalInt32Length];
        Span<int> otherSpan = stackalloc int[totalInt32Length];

        for (int i = 0; i < Length; i++)
        {
            decimal.GetBits(this[i], thisSpan.Slice(i * 4, 4));
            decimal.GetBits(other[i], otherSpan.Slice(i * 4, 4));
        }

        return thisSpan.TryParallelEquals(otherSpan, out bool result) ? result : this.SequentialEquals(other);
    }

    public int CompareTo(DecimalTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}
