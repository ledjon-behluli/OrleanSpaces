using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[GenerateSerializer, Immutable]
public readonly struct UShortTuple : INumericTuple<ushort>, IEquatable<UShortTuple>
{
    [Id(0), JsonProperty] private readonly ushort[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly ushort this[int index] => ref fields[index];

    Span<ushort> INumericTuple<ushort>.Fields => fields.AsSpan();

    public UShortTuple() : this(Array.Empty<ushort>()) { }
    public UShortTuple(params ushort[] fields) => this.fields = fields;

    public static bool operator ==(UShortTuple left, UShortTuple right) => left.Equals(right);
    public static bool operator !=(UShortTuple left, UShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UShortTuple tuple && Equals(tuple);
    public bool Equals(UShortTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    static ISpaceTuple<ushort> ISpaceTuple<ushort>.Create(ushort[] fields) => new UShortTuple(fields);
    ISpaceTemplate<ushort> ISpaceTuple<ushort>.ToTemplate()
    {
        ref ushort?[] fields = ref TupleHelpers.CastAs<ushort[], ushort?[]>(in this.fields);
        return new UShortTemplate(fields);
    }

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_UShort);
    public ReadOnlySpan<ushort>.Enumerator GetEnumerator() => new ReadOnlySpan<ushort>(fields).GetEnumerator();
}

public readonly record struct UShortTemplate : ISpaceTemplate<ushort>
{
    private readonly ushort?[] fields;

    public ref readonly ushort? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public UShortTemplate([AllowNull] params ushort?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new ushort?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<ushort>
        => TupleHelpers.Matches<ushort, UShortTuple>(this, tuple);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<ushort?>.Enumerator GetEnumerator() => new ReadOnlySpan<ushort?>(fields).GetEnumerator();
}
