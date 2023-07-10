using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly struct SByteTuple : 
    INumericTuple<sbyte>, ISpaceConvertible<sbyte, SByteTemplate>, IEquatable<SByteTuple>
{
    [Id(0), JsonProperty] private readonly sbyte[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly sbyte this[int index] => ref fields[index];

    Span<sbyte> INumericTuple<sbyte>.Fields => fields.AsSpan();

    public SByteTuple() => fields = Array.Empty<sbyte>();
    public SByteTuple([AllowNull] params sbyte[] fields)
        => this.fields = fields is null ? Array.Empty<sbyte>() : fields;

    public static bool operator ==(SByteTuple left, SByteTuple right) => left.Equals(right);
    public static bool operator !=(SByteTuple left, SByteTuple right) => !(left == right);

    public SByteTemplate ToTemplate()
    {
        int length = Length;
        sbyte?[] fields = new sbyte?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new SByteTemplate(fields);
    }

    public override bool Equals(object? obj) => obj is SByteTuple tuple && Equals(tuple);
    public bool Equals(SByteTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    static ISpaceTuple<sbyte> ISpaceTuple<sbyte>.Create(sbyte[] fields) => new SByteTuple(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_SByte);
    public ReadOnlySpan<sbyte>.Enumerator GetEnumerator() => new ReadOnlySpan<sbyte>(fields).GetEnumerator();
}

public readonly record struct SByteTemplate : ISpaceTemplate<sbyte>, ISpaceMatchable<sbyte, SByteTuple>
{
    private readonly sbyte?[] fields;

    public ref readonly sbyte? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public SByteTemplate() => fields = Array.Empty<sbyte?>();
    public SByteTemplate([AllowNull] params sbyte?[] fields)
        => this.fields = fields is null ? Array.Empty<sbyte?>() : fields;

    public bool Matches(SByteTuple tuple) => TupleHelpers.Matches<sbyte, SByteTuple>(this, tuple);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<sbyte?>.Enumerator GetEnumerator() => new ReadOnlySpan<sbyte?>(fields).GetEnumerator();
}

