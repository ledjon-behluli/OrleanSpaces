using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly record struct TimeSpanTuple :
    IEquatable<TimeSpanTuple>,
    ISpaceTuple<TimeSpan>, 
    ISpaceFactory<TimeSpan, TimeSpanTuple>,
    ISpaceConvertible<TimeSpan, TimeSpanTemplate>
{
    [Id(0), JsonProperty] private readonly TimeSpan[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly TimeSpan this[int index] => ref fields[index];

    public TimeSpanTuple() => fields = Array.Empty<TimeSpan>();
    public TimeSpanTuple([AllowNull] params TimeSpan[] fields)
        => this.fields = fields is null ? Array.Empty<TimeSpan>() : fields;

    public TimeSpanTemplate ToTemplate()
    {
        int length = Length;
        TimeSpan?[] fields = new TimeSpan?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new TimeSpanTemplate(fields);
    }

    public bool Equals(TimeSpanTuple other)
    {
        NumericMarshaller<TimeSpan, long> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static TimeSpanTuple ISpaceFactory<TimeSpan, TimeSpanTuple>.Create(TimeSpan[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_TimeSpan);
    public ReadOnlySpan<TimeSpan>.Enumerator GetEnumerator() => new ReadOnlySpan<TimeSpan>(fields).GetEnumerator();
}

public readonly record struct TimeSpanTemplate : 
    IEquatable<TimeSpanTemplate>,
    ISpaceTemplate<TimeSpan>, 
    ISpaceMatchable<TimeSpan, TimeSpanTuple>
{
    private readonly TimeSpan?[] fields;

    public ref readonly TimeSpan? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public TimeSpanTemplate() => fields = Array.Empty<TimeSpan?>();
    public TimeSpanTemplate([AllowNull] params TimeSpan?[] fields)
        => this.fields = fields is null ? Array.Empty<TimeSpan?>() : fields;

    public bool Matches(TimeSpanTuple tuple) => this.Matches<TimeSpan, TimeSpanTuple>(tuple);
    public bool Equals(TimeSpanTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<TimeSpan?>.Enumerator GetEnumerator() => new ReadOnlySpan<TimeSpan?>(fields).GetEnumerator();
}