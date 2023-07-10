using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly struct DateTimeOffsetTuple :
    IEquatable<DateTimeOffsetTuple>,
    ISpaceTuple<DateTimeOffset>, 
    ISpaceFactory<DateTimeOffset, DateTimeOffsetTuple>,
    ISpaceConvertible<DateTimeOffset, DateTimeOffsetTemplate>
{
    [Id(0), JsonProperty] private readonly DateTimeOffset[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly DateTimeOffset this[int index] => ref fields[index];

    public DateTimeOffsetTuple() => fields = Array.Empty<DateTimeOffset>();
    public DateTimeOffsetTuple([AllowNull] params DateTimeOffset[] fields)
        => this.fields = fields is null ? Array.Empty<DateTimeOffset>() : fields;

    public static bool operator ==(DateTimeOffsetTuple left, DateTimeOffsetTuple right) => left.Equals(right);
    public static bool operator !=(DateTimeOffsetTuple left, DateTimeOffsetTuple right) => !(left == right);

    public DateTimeOffsetTemplate ToTemplate()
    {
        int length = Length;
        DateTimeOffset?[] fields = new DateTimeOffset?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new DateTimeOffsetTemplate(fields);
    }

    public override bool Equals(object? obj) => obj is DateTimeOffsetTuple tuple && Equals(tuple);

    public bool Equals(DateTimeOffsetTuple other)
    {
        NumericMarshaller<DateTimeOffset, long> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static DateTimeOffsetTuple ISpaceFactory<DateTimeOffset, DateTimeOffsetTuple>.Create(DateTimeOffset[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_DateTimeOffset);
    public ReadOnlySpan<DateTimeOffset>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTimeOffset>(fields).GetEnumerator();
}

public readonly record struct DateTimeOffsetTemplate : ISpaceTemplate<DateTimeOffset>, ISpaceMatchable<DateTimeOffset, DateTimeOffsetTuple>
{
    private readonly DateTimeOffset?[] fields;

    public ref readonly DateTimeOffset? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public DateTimeOffsetTemplate() => fields = Array.Empty<DateTimeOffset?>();
    public DateTimeOffsetTemplate([AllowNull] params DateTimeOffset?[] fields) =>
        this.fields = fields is null ? Array.Empty<DateTimeOffset?>() : fields;

    public bool Matches(DateTimeOffsetTuple tuple) => SpaceHelpers.Matches<DateTimeOffset, DateTimeOffsetTuple>(this, tuple);

    public override string ToString() => SpaceHelpers.ToString(fields);
    public ReadOnlySpan<DateTimeOffset?>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTimeOffset?>(fields).GetEnumerator();
}
