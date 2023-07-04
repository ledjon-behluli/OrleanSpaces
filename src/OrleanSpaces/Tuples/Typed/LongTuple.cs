using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[GenerateSerializer, Immutable]
public readonly struct LongTuple : INumericTuple<long>, IEquatable<LongTuple>
{
    [Id(0), JsonProperty] private readonly long[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly long this[int index] => ref fields[index];

    Span<long> INumericTuple<long>.Fields => fields.AsSpan();

    public LongTuple() : this(Array.Empty<long>()) { }
    public LongTuple(params long[] fields) => this.fields = fields;

    public static bool operator ==(LongTuple left, LongTuple right) => left.Equals(right);
    public static bool operator !=(LongTuple left, LongTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is LongTuple tuple && Equals(tuple);
    public bool Equals(LongTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    public ISpaceTemplate<long> ToTemplate()
    {
        ref long?[] fields = ref TupleHelpers.CastAs<long[], long?[]>(in this.fields);
        return new LongTemplate(fields);
    }

    static ISpaceTuple<long> ISpaceTuple<long>.Create(long[] fields) => new LongTuple(fields);
    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Long);
    public ReadOnlySpan<long>.Enumerator GetEnumerator() => new ReadOnlySpan<long>(fields).GetEnumerator();
}

public readonly record struct LongTemplate : ISpaceTemplate<long>
{
    private readonly long?[] fields;

    public ref readonly long? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public LongTemplate([AllowNull] params long?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new long?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<long>
        => TupleHelpers.Matches<long, LongTuple>(this, tuple);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<long?>.Enumerator GetEnumerator() => new ReadOnlySpan<long?>(fields).GetEnumerator();
}
