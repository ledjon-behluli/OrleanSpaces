using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly struct UIntTuple :
    IEquatable<UIntTuple>,
    INumericTuple<uint>, 
    ISpaceFactory<uint, UIntTuple>,
    ISpaceConvertible<uint, UIntTemplate>
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

    public UIntTemplate ToTemplate()
    {
        int length = Length;
        uint?[] fields = new uint?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new UIntTemplate(fields);
    }

    public override bool Equals(object? obj) => obj is UIntTuple tuple && Equals(tuple);
    public bool Equals(UIntTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    static UIntTuple ISpaceFactory<uint, UIntTuple>.Create(uint[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_UInt);
    public ReadOnlySpan<uint>.Enumerator GetEnumerator() => new ReadOnlySpan<uint>(fields).GetEnumerator();
}

public readonly record struct UIntTemplate : ISpaceTemplate<uint>, ISpaceMatchable<uint, UIntTuple>
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