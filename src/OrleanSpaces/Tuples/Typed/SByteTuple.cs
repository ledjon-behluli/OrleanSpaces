using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[GenerateSerializer, Immutable]
public readonly struct SByteTuple : INumericTuple<sbyte>, IEquatable<SByteTuple>
{
    [Id(0), JsonProperty] private readonly sbyte[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly sbyte this[int index] => ref fields[index];

    Span<sbyte> INumericTuple<sbyte>.Fields => fields.AsSpan();

    public SByteTuple() : this(Array.Empty<sbyte>()) { }
    public SByteTuple(params sbyte[] fields) => this.fields = fields;

    public static bool operator ==(SByteTuple left, SByteTuple right) => left.Equals(right);
    public static bool operator !=(SByteTuple left, SByteTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is SByteTuple tuple && Equals(tuple);
    public bool Equals(SByteTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_SByte);

    public ReadOnlySpan<sbyte>.Enumerator GetEnumerator() => new ReadOnlySpan<sbyte>(fields).GetEnumerator();
}

public readonly record struct SByteTemplate : ISpaceTemplate<sbyte>
{
    private readonly sbyte?[] fields;

    public ref readonly sbyte? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public SByteTemplate([AllowNull] params sbyte?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new sbyte?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<sbyte>
        => TupleHelpers.Matches(this, tuple);

    ISpaceTuple<sbyte> ISpaceTemplate<sbyte>.Create(sbyte[] fields) => new SByteTuple(fields);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<sbyte?>.Enumerator GetEnumerator() => new ReadOnlySpan<sbyte?>(fields).GetEnumerator();
}

