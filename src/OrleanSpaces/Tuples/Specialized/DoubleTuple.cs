using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly struct DoubleTuple :
    IEquatable<DoubleTuple>,
    INumericTuple<double>,
    ISpaceFactory<double, DoubleTuple>,
    ISpaceConvertible<double, DoubleTemplate>
{
    [Id(0), JsonProperty] private readonly double[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly double this[int index] => ref fields[index];

    Span<double> INumericTuple<double>.Fields => fields.AsSpan();

    public DoubleTuple() => fields = Array.Empty<double>();
    public DoubleTuple([AllowNull] params double[] fields)
        => this.fields = fields is null ? Array.Empty<double>() : fields;

    public static bool operator ==(DoubleTuple left, DoubleTuple right) => left.Equals(right);
    public static bool operator !=(DoubleTuple left, DoubleTuple right) => !(left == right);

    public DoubleTemplate ToTemplate()
    {
        int length = Length;
        double?[] fields = new double?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new DoubleTemplate(fields);
    }

    public override bool Equals(object? obj) => obj is DoubleTuple tuple && Equals(tuple);
    public bool Equals(DoubleTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    static DoubleTuple ISpaceFactory<double, DoubleTuple>.Create(double[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Double);
    public ReadOnlySpan<double>.Enumerator GetEnumerator() => new ReadOnlySpan<double>(fields).GetEnumerator();
}

public readonly record struct DoubleTemplate : ISpaceTemplate<double>, ISpaceMatchable<double, DoubleTuple>
{
    private readonly double?[] fields;

    public ref readonly double? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public DoubleTemplate() => fields = Array.Empty<double?>();
    public DoubleTemplate([AllowNull] params double?[] fields)
        => this.fields = fields is null ? Array.Empty<double?>() : fields;

    public bool Matches(DoubleTuple tuple) => TupleHelpers.Matches<double, DoubleTuple>(this, tuple);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<double?>.Enumerator GetEnumerator() => new ReadOnlySpan<double?>(fields).GetEnumerator();
}
