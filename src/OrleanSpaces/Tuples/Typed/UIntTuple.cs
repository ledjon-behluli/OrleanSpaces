using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[GenerateSerializer, Immutable]
public readonly struct UIntTuple : INumericTuple<uint>, IEquatable<UIntTuple>
{
    [Id(0), JsonProperty] private readonly uint[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly uint this[int index] => ref fields[index];

    Span<uint> INumericTuple<uint>.Fields => fields.AsSpan();

    public UIntTuple() : this(Array.Empty<uint>()) { }
    public UIntTuple(params uint[] fields) => this.fields = fields;

    public static bool operator ==(UIntTuple left, UIntTuple right) => left.Equals(right);
    public static bool operator !=(UIntTuple left, UIntTuple right) => !(left == right);

    public static explicit operator UIntTemplate(UIntTuple tuple)
    {
        ref uint?[] fields = ref TupleHelpers.CastAs<uint[], uint?[]>(tuple.fields);
        return new UIntTemplate(fields);
    }

    public override bool Equals(object? obj) => obj is UIntTuple tuple && Equals(tuple);
    public bool Equals(UIntTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    ISpaceTemplate<uint> ISpaceTuple<uint>.ToTemplate() => (UIntTemplate)this;
    static ISpaceTuple<uint> ISpaceTuple<uint>.Create(uint[] fields) => new UIntTuple(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_UInt);
    public ReadOnlySpan<uint>.Enumerator GetEnumerator() => new ReadOnlySpan<uint>(fields).GetEnumerator();
}

public readonly record struct UIntTemplate : ISpaceTemplate<uint>
{
    private readonly uint?[] fields;

    public ref readonly uint? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public UIntTemplate([AllowNull] params uint?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new uint?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<uint>
        => TupleHelpers.Matches<uint, UIntTuple>(this, tuple);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<uint?>.Enumerator GetEnumerator() => new ReadOnlySpan<uint?>(fields).GetEnumerator();
}