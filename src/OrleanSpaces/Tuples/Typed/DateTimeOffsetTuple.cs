using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct DateTimeOffsetTuple : ISpaceTuple<DateTimeOffset, DateTimeOffsetTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>12/31/9999 11:59:59 PM +00:00</example>
    internal const int MaxFieldCharLength = 29;

    private readonly DateTimeOffset[] fields;

    public ref readonly DateTimeOffset this[int index] => ref fields[index];
    public int Length => fields.Length;

    public DateTimeOffsetTuple() : this(Array.Empty<DateTimeOffset>()) { }
    public DateTimeOffsetTuple(params DateTimeOffset[] fields) => this.fields = fields;

    public static bool operator ==(DateTimeOffsetTuple left, DateTimeOffsetTuple right) => left.Equals(right);
    public static bool operator !=(DateTimeOffsetTuple left, DateTimeOffsetTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is DateTimeOffsetTuple tuple && Equals(tuple);

    public bool Equals(DateTimeOffsetTuple other)
    {
        NumericMarshaller<DateTimeOffset, long> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public int CompareTo(DateTimeOffsetTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<DateTimeOffset>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTimeOffset>(fields).GetEnumerator();
}
