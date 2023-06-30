using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[GenerateSerializer, Immutable]
public readonly struct DateTimeTuple : ISpaceTuple<DateTime>, IEquatable<DateTimeTuple>
{
    [Id(0), JsonProperty] private readonly DateTime[] fields;
    [JsonProperty] public int Length => fields.Length;

    public ref readonly DateTime this[int index] => ref fields[index];

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

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_DateTime);

    public ReadOnlySpan<DateTime>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTime>(fields).GetEnumerator();
}

public readonly record struct DateTimeTemplate : ISpaceTemplate<DateTime>
{
    private readonly DateTime?[] fields;

    public ref readonly DateTime? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public DateTimeTemplate([AllowNull] params DateTime?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new DateTime?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<DateTime>
        => TupleHelpers.Matches(this, tuple);

    ISpaceTuple<DateTime> ISpaceTemplate<DateTime>.Create(DateTime[] fields) => new DateTimeTuple(fields);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<DateTime?>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTime?>(fields).GetEnumerator();
}
