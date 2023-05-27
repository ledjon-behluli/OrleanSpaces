using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct DateTimeTuple : ISpaceTuple<DateTime>, IEquatable<DateTimeTuple>, IComparable<DateTimeTuple>
{
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

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_DateTime);

    public ReadOnlySpan<DateTime>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTime>(fields).GetEnumerator();
}

[Immutable]
public readonly struct SpaceDateTime
{
    public readonly DateTime Value;

    internal static readonly SpaceDateTime Default = new();

    public SpaceDateTime(DateTime value) => Value = value;

    public static implicit operator SpaceDateTime(DateTime value) => new(value);
    public static implicit operator DateTime(SpaceDateTime value) => value.Value;
}

[Immutable]
public readonly struct DateTimeTemplate : ISpaceTemplate<DateTimeTuple>
{
    private readonly SpaceDateTime[] fields;

    public DateTimeTemplate([AllowNull] params SpaceDateTime[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new SpaceDateTime[1] { new SpaceUnit() } : fields;
}