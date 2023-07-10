using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly struct UShortTuple :
    IEquatable<UShortTuple>,
    INumericTuple<ushort>, 
    ISpaceFactory<ushort, UShortTuple>,
    ISpaceConvertible<ushort, UShortTemplate>
{
    [Id(0), JsonProperty] private readonly ushort[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly ushort this[int index] => ref fields[index];

    Span<ushort> INumericTuple<ushort>.Fields => fields.AsSpan();

    public UShortTuple() => fields = Array.Empty<ushort>();
    public UShortTuple([AllowNull] params ushort[] fields)
        => this.fields = fields is null ? Array.Empty<ushort>() : fields;

    public static bool operator ==(UShortTuple left, UShortTuple right) => left.Equals(right);
    public static bool operator !=(UShortTuple left, UShortTuple right) => !(left == right);

    public UShortTemplate ToTemplate()
    {
        int length = Length;
        ushort?[] fields = new ushort?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new UShortTemplate(fields);
    }

    public override bool Equals(object? obj) => obj is UShortTuple tuple && Equals(tuple);
    public bool Equals(UShortTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    static UShortTuple ISpaceFactory<ushort, UShortTuple>.Create(ushort[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_UShort);
    public ReadOnlySpan<ushort>.Enumerator GetEnumerator() => new ReadOnlySpan<ushort>(fields).GetEnumerator();
}

public readonly record struct UShortTemplate : ISpaceTemplate<ushort>, ISpaceMatchable<ushort, UShortTuple>
{
    private readonly ushort?[] fields;

    public ref readonly ushort? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public UShortTemplate() => fields = Array.Empty<ushort?>();
    public UShortTemplate([AllowNull] params ushort?[] fields)
        => this.fields = fields is null ? Array.Empty<ushort?>() : fields;

    public bool Matches(UShortTuple tuple) => TupleHelpers.Matches<ushort, UShortTuple>(this, tuple);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<ushort?>.Enumerator GetEnumerator() => new ReadOnlySpan<ushort?>(fields).GetEnumerator();
}
