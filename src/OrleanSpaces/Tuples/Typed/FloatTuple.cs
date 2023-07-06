using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[GenerateSerializer, Immutable]
public readonly struct FloatTuple : INumericTuple<float>, IEquatable<FloatTuple>
{
    [Id(0), JsonProperty] private readonly float[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly float this[int index] => ref fields[index];

    Span<float> INumericTuple<float>.Fields => fields.AsSpan();

    public FloatTuple() => fields = Array.Empty<float>();
    public FloatTuple([AllowNull] params float[] fields) 
        => this.fields = fields is null ? Array.Empty<float>() : fields;

    public static bool operator ==(FloatTuple left, FloatTuple right) => left.Equals(right);
    public static bool operator !=(FloatTuple left, FloatTuple right) => !(left == right);

    public static explicit operator FloatTemplate(FloatTuple tuple)
    {
        int length = tuple.Length;
        float?[] fields = new float?[length];
        
        for (int i = 0; i < length; i++)
        {
            fields[i] = tuple[i];
        }

        return new FloatTemplate(fields);
    }

    public override bool Equals(object? obj) => obj is FloatTuple tuple && Equals(tuple);
    public bool Equals(FloatTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    ISpaceTemplate<float> ISpaceTuple<float>.ToTemplate() => (FloatTemplate)this;
    static ISpaceTuple<float> ISpaceTuple<float>.Create(float[] fields) => new FloatTuple(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Float);
    public ReadOnlySpan<float>.Enumerator GetEnumerator() => new ReadOnlySpan<float>(fields).GetEnumerator();
}

public readonly record struct FloatTemplate : ISpaceTemplate<float>
{
    private readonly float?[] fields;

    public ref readonly float? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public FloatTemplate() => fields = Array.Empty<float?>();
    public FloatTemplate([AllowNull] params float?[] fields) 
        => this.fields = fields is null ? Array.Empty<float?>() : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<float>
        => TupleHelpers.Matches<float, FloatTuple>(this, tuple);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<float?>.Enumerator GetEnumerator() => new ReadOnlySpan<float?>(fields).GetEnumerator();
}
