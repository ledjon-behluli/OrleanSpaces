using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly record struct IntTuple :
    IEquatable<IntTuple>,
    INumericTuple<int>, 
    ISpaceFactory<int, IntTuple>,
    ISpaceConvertible<int, IntTemplate>
{
    [Id(0), JsonProperty] private readonly int[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly int this[int index] => ref fields[index];

    Span<int> INumericTuple<int>.Fields => fields.AsSpan();

    public IntTuple() => fields = Array.Empty<int>();
    public IntTuple([AllowNull] params int[] fields)
        => this.fields = fields is null ? Array.Empty<int>() : fields;

    public IntTemplate ToTemplate()
    {
        int length = Length;
        int?[] fields = new int?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new IntTemplate(fields);
    }

    public bool Equals(IntTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static IntTuple ISpaceFactory<int, IntTuple>.Create(int[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Int);
    public ReadOnlySpan<int>.Enumerator GetEnumerator() => new ReadOnlySpan<int>(fields).GetEnumerator();
}

public readonly record struct IntTemplate : 
    IEquatable<IntTemplate>,
    ISpaceTemplate<int>, 
    ISpaceMatchable<int, IntTuple>
{
    private readonly int?[] fields;

    public ref readonly int? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public IntTemplate() => fields = Array.Empty<int?>();
    public IntTemplate([AllowNull] params int?[] fields)
        => this.fields = fields is null ? Array.Empty<int?>() : fields;

    public bool Matches(IntTuple tuple) => this.Matches<int, IntTuple>(tuple);
    public bool Equals(IntTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<int?>.Enumerator GetEnumerator() => new ReadOnlySpan<int?>(fields).GetEnumerator();
}