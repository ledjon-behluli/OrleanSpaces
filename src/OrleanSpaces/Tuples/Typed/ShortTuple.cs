using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[GenerateSerializer, Immutable]
public readonly struct ShortTuple : INumericTuple<short>, IEquatable<ShortTuple>
{
    [Id(0), JsonProperty] private readonly short[] fields;
    [JsonProperty] public int Length => fields.Length;

    public ref readonly short this[int index] => ref fields[index];

    Span<short> INumericTuple<short>.Fields => fields.AsSpan();

    public ShortTuple() : this(Array.Empty<short>()) { }
    public ShortTuple(params short[] fields) => this.fields = fields;

    public static bool operator ==(ShortTuple left, ShortTuple right) => left.Equals(right);
    public static bool operator !=(ShortTuple left, ShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ShortTuple tuple && Equals(tuple);
    public bool Equals(ShortTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Short);

    public ReadOnlySpan<short>.Enumerator GetEnumerator() => new ReadOnlySpan<short>(fields).GetEnumerator();
}

public readonly record struct ShortTemplate : ISpaceTemplate<short>
{
    private readonly short?[] fields;

    public ref readonly short? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public ShortTemplate([AllowNull] params short?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new short?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<short>
        => TupleHelpers.Matches(this, tuple);

    ISpaceTuple<short> ISpaceTemplate<short>.Create(short[] fields) => new ShortTuple(fields);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<short?>.Enumerator GetEnumerator() => new ReadOnlySpan<short?>(fields).GetEnumerator();
}
