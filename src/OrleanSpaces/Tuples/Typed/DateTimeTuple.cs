using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct DateTimeTuple : ISpaceTuple<DateTime>, IEquatable<DateTimeTuple>, IComparable<DateTimeTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>12/31/9999 11:59:59 PM</example>
    internal const int MaxFieldCharLength = 22;

    private readonly DateTime[] fields;

    public ref readonly DateTime this[int index] => ref fields[index];
    public int Length => fields.Length;

    public DateTimeTuple() : this(Array.Empty<DateTime>()) { }
    public DateTimeTuple(params DateTime[] fields) => this.fields = fields;

    public static bool operator ==(DateTimeTuple left, DateTimeTuple right) => left.Equals(right);
    public static bool operator !=(DateTimeTuple left, DateTimeTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is DateTimeTuple tuple && Equals(tuple);

    public bool Equals(DateTimeTuple other)
    {
        NumericMarshaller<DateTime, long> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public int CompareTo(DateTimeTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<DateTime>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTime>(fields).GetEnumerator();
}