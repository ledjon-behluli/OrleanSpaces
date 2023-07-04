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

    public FloatTuple() : this(Array.Empty<float>()) { }
    public FloatTuple(params float[] fields) => this.fields = fields;

    public static bool operator ==(FloatTuple left, FloatTuple right) => left.Equals(right);
    public static bool operator !=(FloatTuple left, FloatTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is FloatTuple tuple && Equals(tuple);
    public bool Equals(FloatTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    public ISpaceTemplate<float> ToTemplate()
    {
        ref float?[] fields = ref TupleHelpers.CastAs<float[], float?[]>(in this.fields);
        return new FloatTemplate(fields);
    }

    static ISpaceTuple<float> ISpaceTuple<float>.Create(float[] fields) => new FloatTuple(fields);
    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Float);
    public ReadOnlySpan<float>.Enumerator GetEnumerator() => new ReadOnlySpan<float>(fields).GetEnumerator();
}

public readonly record struct FloatTemplate : ISpaceTemplate<float>
{
    private readonly float?[] fields;

    public ref readonly float? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public FloatTemplate([AllowNull] params float?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new float?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<float>
        => TupleHelpers.Matches<float, FloatTuple>(this, tuple);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<float?>.Enumerator GetEnumerator() => new ReadOnlySpan<float?>(fields).GetEnumerator();
}
