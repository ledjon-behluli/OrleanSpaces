using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly struct IntTuple : INumericTuple<int>, IEquatable<IntTuple>
{
    [Id(0), JsonProperty] private readonly int[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly int this[int index] => ref fields[index];

    Span<int> INumericTuple<int>.Fields => fields.AsSpan();

    public IntTuple() => fields = Array.Empty<int>();
    public IntTuple([AllowNull] params int[] fields)
        => this.fields = fields is null ? Array.Empty<int>() : fields;

    public static bool operator ==(IntTuple left, IntTuple right) => left.Equals(right);
    public static bool operator !=(IntTuple left, IntTuple right) => !(left == right);

    public static explicit operator IntTemplate(IntTuple tuple)
    {
        int length = tuple.Length;
        int?[] fields = new int?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = tuple[i];
        }

        return new IntTemplate(fields);
    }

    public override bool Equals(object? obj) => obj is IntTuple tuple && Equals(tuple);
    public bool Equals(IntTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    ISpaceTemplate<int> ISpaceTuple<int>.ToTemplate() => (IntTemplate)this;
    static ISpaceTuple<int> ISpaceTuple<int>.Create(int[] fields) => new IntTuple(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Int);
    public ReadOnlySpan<int>.Enumerator GetEnumerator() => new ReadOnlySpan<int>(fields).GetEnumerator();
}

public readonly record struct IntTemplate : ISpaceTemplate<int>, ITupleMatcher<int, IntTuple>
{
    private readonly int?[] fields;

    public ref readonly int? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public IntTemplate() => fields = Array.Empty<int?>();
    public IntTemplate([AllowNull] params int?[] fields)
        => this.fields = fields is null ? Array.Empty<int?>() : fields;

    public bool Matches(IntTuple tuple) => TupleHelpers.Matches<int, IntTuple>(this, tuple);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<int?>.Enumerator GetEnumerator() => new ReadOnlySpan<int?>(fields).GetEnumerator();
}