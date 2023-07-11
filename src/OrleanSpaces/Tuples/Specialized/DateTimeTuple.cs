using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly record struct DateTimeTuple :
    IEquatable<DateTimeTuple>,
    ISpaceTuple<DateTime>,
    ISpaceFactory<DateTime, DateTimeTuple>,
    ISpaceConvertible<DateTime, DateTimeTemplate>
{
    [Id(0), JsonProperty] private readonly DateTime[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly DateTime this[int index] => ref fields[index];

    public DateTimeTuple() => fields = Array.Empty<DateTime>();
    public DateTimeTuple([AllowNull] params DateTime[] fields)
        => this.fields = fields is null ? Array.Empty<DateTime>() : fields;

    public DateTimeTemplate ToTemplate()
    {
        int length = Length;
        DateTime?[] fields = new DateTime?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new DateTimeTemplate(fields);
    }

    public bool Equals(DateTimeTuple other)
    {
        NumericMarshaller<DateTime, long> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static DateTimeTuple ISpaceFactory<DateTime, DateTimeTuple>.Create(DateTime[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_DateTime);
    public ReadOnlySpan<DateTime>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTime>(fields).GetEnumerator();
}

public readonly record struct DateTimeTemplate : 
    IEquatable<DateTimeTemplate>,
    ISpaceTemplate<DateTime>, 
    ISpaceMatchable<DateTime, DateTimeTuple>
{
    private readonly DateTime?[] fields;

    public ref readonly DateTime? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public DateTimeTemplate() => fields = Array.Empty<DateTime?>();
    public DateTimeTemplate([AllowNull] params DateTime?[] fields) =>
        this.fields = fields is null ? Array.Empty<DateTime?>() : fields;

    public bool Matches(DateTimeTuple tuple) => this.Matches<DateTime, DateTimeTuple>(tuple);
    public bool Equals(DateTimeTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<DateTime?>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTime?>(fields).GetEnumerator();
}
