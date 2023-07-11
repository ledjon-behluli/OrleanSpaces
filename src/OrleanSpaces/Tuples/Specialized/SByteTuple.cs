using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly record struct SByteTuple :
    IEquatable<SByteTuple>,
    INumericTuple<sbyte>, 
    ISpaceFactory<sbyte, SByteTuple>,
    ISpaceConvertible<sbyte, SByteTemplate>
{
    [Id(0), JsonProperty] private readonly sbyte[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly sbyte this[int index] => ref fields[index];

    Span<sbyte> INumericTuple<sbyte>.Fields => fields.AsSpan();

    public SByteTuple() => fields = Array.Empty<sbyte>();
    public SByteTuple([AllowNull] params sbyte[] fields)
        => this.fields = fields is null ? Array.Empty<sbyte>() : fields;

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

    public bool Equals(SByteTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static SByteTuple ISpaceFactory<sbyte, SByteTuple>.Create(sbyte[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_SByte);
    public ReadOnlySpan<sbyte>.Enumerator GetEnumerator() => new ReadOnlySpan<sbyte>(fields).GetEnumerator();
}

public readonly record struct SByteTemplate : 
    IEquatable<SByteTemplate>,
    ISpaceTemplate<sbyte>, 
    ISpaceMatchable<sbyte, SByteTuple>
{
    private readonly sbyte?[] fields;

    public ref readonly sbyte? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public SByteTemplate() => fields = Array.Empty<sbyte?>();
    public SByteTemplate([AllowNull] params sbyte?[] fields)
        => this.fields = fields is null ? Array.Empty<sbyte?>() : fields;

    public bool Matches(SByteTuple tuple) => this.Matches<sbyte, SByteTuple>(tuple);
    public bool Equals(SByteTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<sbyte?>.Enumerator GetEnumerator() => new ReadOnlySpan<sbyte?>(fields).GetEnumerator();
}

