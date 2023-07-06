using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[GenerateSerializer, Immutable]
public readonly struct TimeSpanTuple : ISpaceTuple<TimeSpan>, IEquatable<TimeSpanTuple>
{
    [Id(0), JsonProperty] private readonly TimeSpan[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly TimeSpan this[int index] => ref fields[index];

    public TimeSpanTuple() => fields = Array.Empty<TimeSpan>();
    public TimeSpanTuple([AllowNull] params TimeSpan[] fields)
        => this.fields = fields is null ? Array.Empty<TimeSpan>() : fields;

    public static bool operator ==(TimeSpanTuple left, TimeSpanTuple right) => left.Equals(right);
    public static bool operator !=(TimeSpanTuple left, TimeSpanTuple right) => !(left == right);

    public static explicit operator TimeSpanTemplate(TimeSpanTuple tuple)
    {
        ref TimeSpan?[] fields = ref TupleHelpers.CastAs<TimeSpan[], TimeSpan?[]>(in tuple.fields);
        return new TimeSpanTemplate(fields);
    }

    public override bool Equals(object? obj) => obj is TimeSpanTuple tuple && Equals(tuple);

    public bool Equals(TimeSpanTuple other)
    {
        NumericMarshaller<TimeSpan, long> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    ISpaceTemplate<TimeSpan> ISpaceTuple<TimeSpan>.ToTemplate() => (TimeSpanTemplate)this;
    static ISpaceTuple<TimeSpan> ISpaceTuple<TimeSpan>.Create(TimeSpan[] fields) => new TimeSpanTuple(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_TimeSpan);
    public ReadOnlySpan<TimeSpan>.Enumerator GetEnumerator() => new ReadOnlySpan<TimeSpan>(fields).GetEnumerator();
}

public readonly record struct TimeSpanTemplate : ISpaceTemplate<TimeSpan>
{
    private readonly TimeSpan?[] fields;

    public ref readonly TimeSpan? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public TimeSpanTemplate() => fields = Array.Empty<TimeSpan?>();
    public TimeSpanTemplate([AllowNull] params TimeSpan?[] fields) 
        => this.fields = fields is null ? Array.Empty<TimeSpan?>() : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<TimeSpan>
        => TupleHelpers.Matches<TimeSpan, TimeSpanTuple>(this, tuple);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<TimeSpan?>.Enumerator GetEnumerator() => new ReadOnlySpan<TimeSpan?>(fields).GetEnumerator();
}