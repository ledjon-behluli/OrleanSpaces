using OrleanSpaces;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[GenerateSerializer, Immutable]
public readonly struct DateTimeOffsetTuple : ISpaceTuple<DateTimeOffset>, IEquatable<DateTimeOffsetTuple>
{
    [Id(0)] private readonly DateTimeOffset[] fields;

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

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_DateTimeOffset);

    public ReadOnlySpan<DateTimeOffset>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTimeOffset>(fields).GetEnumerator();
}

public readonly record struct DateTimeOffsetTemplate : ISpaceTemplate<DateTimeOffset>
{
    private readonly DateTimeOffset?[] fields;

    public ref readonly DateTimeOffset? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public DateTimeOffsetTemplate([AllowNull] params DateTimeOffset?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new DateTimeOffset?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<DateTimeOffset>
        => TupleHelpers.Matches(this, tuple);

    ISpaceTuple<DateTimeOffset> ISpaceTemplate<DateTimeOffset>.Create(DateTimeOffset[] fields) => new DateTimeOffsetTuple(fields);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<DateTimeOffset?>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTimeOffset?>(fields).GetEnumerator();
}
