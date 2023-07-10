using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly struct UIntTuple : INumericTuple<uint>, IEquatable<UIntTuple>
{
    [Id(0), JsonProperty] private readonly uint[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly uint this[int index] => ref fields[index];

    Span<uint> INumericTuple<uint>.Fields => fields.AsSpan();

    public UIntTuple() => fields = Array.Empty<uint>();
    public UIntTuple([AllowNull] params uint[] fields)
        => this.fields = fields is null ? Array.Empty<uint>() : fields;

    public static bool operator ==(UIntTuple left, UIntTuple right) => left.Equals(right);
    public static bool operator !=(UIntTuple left, UIntTuple right) => !(left == right);

    public static explicit operator UIntTemplate(UIntTuple tuple)
    {
        int length = tuple.Length;
        uint?[] fields = new uint?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = tuple[i];
        }

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

public readonly record struct UIntTemplate : ISpaceTemplate<uint>, ITupleMatcher<uint, UIntTuple>
{
    private readonly uint?[] fields;

    public ref readonly uint? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public UIntTemplate() => fields = Array.Empty<uint?>();
    public UIntTemplate([AllowNull] params uint?[] fields)
        => this.fields = fields is null ? Array.Empty<uint?>() : fields;

    public bool Matches(UIntTuple tuple) => TupleHelpers.Matches<uint, UIntTuple>(this, tuple);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<uint?>.Enumerator GetEnumerator() => new ReadOnlySpan<uint?>(fields).GetEnumerator();
}