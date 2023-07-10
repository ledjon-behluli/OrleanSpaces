using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly struct LongTuple :
    IEquatable<LongTuple>,
    INumericTuple<long>, 
    ISpaceFactory<long, LongTuple>,
    ISpaceConvertible<long, LongTemplate>
{
    [Id(0), JsonProperty] private readonly long[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly long this[int index] => ref fields[index];

    Span<long> INumericTuple<long>.Fields => fields.AsSpan();

    public LongTuple() => fields = Array.Empty<long>();
    public LongTuple([AllowNull] params long[] fields)
        => this.fields = fields is null ? Array.Empty<long>() : fields;

    public static bool operator ==(LongTuple left, LongTuple right) => left.Equals(right);
    public static bool operator !=(LongTuple left, LongTuple right) => !(left == right);

    public LongTemplate ToTemplate()
    {
        int length = Length;
        long?[] fields = new long?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new LongTemplate(fields);
    }

    public override bool Equals(object? obj) => obj is LongTuple tuple && Equals(tuple);
    public bool Equals(LongTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    static LongTuple ISpaceFactory<long, LongTuple>.Create(long[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Long);
    public ReadOnlySpan<long>.Enumerator GetEnumerator() => new ReadOnlySpan<long>(fields).GetEnumerator();
}

public readonly record struct LongTemplate : ISpaceTemplate<long>, ISpaceMatchable<long, LongTuple>
{
    private readonly long?[] fields;

    public ref readonly long? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public LongTemplate() => fields = Array.Empty<long?>();
    public LongTemplate([AllowNull] params long?[] fields)
        => this.fields = fields is null ? Array.Empty<long?>() : fields;

    public bool Matches(LongTuple tuple) => TupleHelpers.Matches<long, LongTuple>(this, tuple);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<long?>.Enumerator GetEnumerator() => new ReadOnlySpan<long?>(fields).GetEnumerator();
}
