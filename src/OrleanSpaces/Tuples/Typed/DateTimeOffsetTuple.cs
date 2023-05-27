using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct DateTimeOffsetTuple : ISpaceTuple<DateTimeOffset>, IEquatable<DateTimeOffsetTuple>, IComparable<DateTimeOffsetTuple>
{
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

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_DateTimeOffset);

    public ReadOnlySpan<DateTimeOffset>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTimeOffset>(fields).GetEnumerator();
}

[Immutable]
public readonly struct SpaceDateTimeOffset
{
    public readonly DateTimeOffset Value;

    internal static readonly SpaceDateTimeOffset Default = new();

    public SpaceDateTimeOffset(DateTimeOffset value) => Value = value;

    public static implicit operator SpaceDateTimeOffset(DateTimeOffset value) => new(value);
    public static implicit operator DateTimeOffset(SpaceDateTimeOffset value) => value.Value;
}

[Immutable]
public readonly struct DateTimeOffsetTemplate : ISpaceTemplate<DateTimeOffsetTuple>
{
    private readonly SpaceDateTimeOffset[] fields;

    public DateTimeOffsetTemplate([AllowNull] params SpaceDateTimeOffset[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new SpaceDateTimeOffset[1] { new SpaceUnit() } : fields;
}
