using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct TimeSpanTuple : ISpaceTuple<TimeSpan, TimeSpanTuple>, IEquatable<TimeSpanTuple>, IComparable<TimeSpanTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-10675199.02:48:05.4775808</example>
    internal const int MaxFieldCharLength = 26;

    private readonly TimeSpan[] fields;

    public ref readonly TimeSpan this[int index] => ref fields[index];
    public int Length => fields.Length;

    public TimeSpanTuple() : this(Array.Empty<TimeSpan>()) { }
    public TimeSpanTuple(params TimeSpan[] fields) => this.fields = fields;

    public static bool operator ==(TimeSpanTuple left, TimeSpanTuple right) => left.Equals(right);
    public static bool operator !=(TimeSpanTuple left, TimeSpanTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is TimeSpanTuple tuple && Equals(tuple);

    public bool Equals(TimeSpanTuple other)
    {
        NumericMarshaller<TimeSpan, long> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public int CompareTo(TimeSpanTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<TimeSpan>.Enumerator GetEnumerator() => new ReadOnlySpan<TimeSpan>(fields).GetEnumerator();
}