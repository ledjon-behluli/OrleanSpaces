using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly record struct FloatTuple :
    IEquatable<FloatTuple>,
    INumericTuple<float>, 
    ISpaceFactory<float, FloatTuple>,
    ISpaceConvertible<float, FloatTemplate>
{
    [Id(0), JsonProperty] private readonly float[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly float this[int index] => ref fields[index];

    Span<float> INumericTuple<float>.Fields => fields.AsSpan();

    public FloatTuple() => fields = Array.Empty<float>();
    public FloatTuple([AllowNull] params float[] fields)
        => this.fields = fields is null ? Array.Empty<float>() : fields;

    public FloatTemplate ToTemplate()
    {
        int length = Length;
        float?[] fields = new float?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new FloatTemplate(fields);
    }

    public bool Equals(FloatTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static FloatTuple ISpaceFactory<float, FloatTuple>.Create(float[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Float);
    public ReadOnlySpan<float>.Enumerator GetEnumerator() => new ReadOnlySpan<float>(fields).GetEnumerator();
}

public readonly record struct FloatTemplate : 
    IEquatable<FloatTemplate>,
    ISpaceTemplate<float>,
    ISpaceMatchable<float, FloatTuple>
{
    private readonly float?[] fields;

    public ref readonly float? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public FloatTemplate() => fields = Array.Empty<float?>();
    public FloatTemplate([AllowNull] params float?[] fields)
        => this.fields = fields is null ? Array.Empty<float?>() : fields;

    public bool Matches(FloatTuple tuple) => this.Matches<float, FloatTuple>(tuple);
    public bool Equals(FloatTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<float?>.Enumerator GetEnumerator() => new ReadOnlySpan<float?>(fields).GetEnumerator();
}
