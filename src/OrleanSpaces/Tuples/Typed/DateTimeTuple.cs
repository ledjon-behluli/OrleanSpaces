using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[GenerateSerializer, Immutable]
public readonly struct DateTimeTuple : ISpaceTuple<DateTime>, IEquatable<DateTimeTuple>
{
    [Id(0), JsonProperty] private readonly DateTime[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly DateTime this[int index] => ref fields[index];

    public DateTimeTuple() => fields = Array.Empty<DateTime>();
    public DateTimeTuple([AllowNull] params DateTime[] fields)
        => this.fields = fields is null ? Array.Empty<DateTime>() : fields;

    public static bool operator ==(DateTimeTuple left, DateTimeTuple right) => left.Equals(right);
    public static bool operator !=(DateTimeTuple left, DateTimeTuple right) => !(left == right);

    public static explicit operator DateTimeTemplate(DateTimeTuple tuple)
    {
        ref DateTime?[] fields = ref TupleHelpers.CastAs<DateTime[], DateTime?[]>(in tuple.fields);
        return new DateTimeTemplate(fields);
    }

    public override bool Equals(object? obj) => obj is DateTimeTuple tuple && Equals(tuple);

    public bool Equals(DateTimeTuple other)
    {
        NumericMarshaller<DateTime, long> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    ISpaceTemplate<DateTime> ISpaceTuple<DateTime>.ToTemplate() => (DateTimeTemplate)this;
    static ISpaceTuple<DateTime> ISpaceTuple<DateTime>.Create(DateTime[] fields) => new DateTimeTuple(fields);
    
    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_DateTime);
    public ReadOnlySpan<DateTime>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTime>(fields).GetEnumerator();
}

public readonly record struct DateTimeTemplate : ISpaceTemplate<DateTime>
{
    private readonly DateTime?[] fields;

    public ref readonly DateTime? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public DateTimeTemplate() => fields = Array.Empty<DateTime?>();
    public DateTimeTemplate([AllowNull] params DateTime?[] fields) =>
        this.fields = fields is null ? Array.Empty<DateTime?>() : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<DateTime>
        => TupleHelpers.Matches<DateTime, DateTimeTuple>(this, tuple);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<DateTime?>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTime?>(fields).GetEnumerator();
}
