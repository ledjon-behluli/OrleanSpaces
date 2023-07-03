using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[GenerateSerializer, Immutable]
public readonly struct IntTuple : INumericTuple<int>, IEquatable<IntTuple>
{
    [Id(0), JsonProperty] private readonly int[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly int this[int index] => ref fields[index];

    Span<int> INumericTuple<int>.Fields => fields.AsSpan();

    public IntTuple() : this(Array.Empty<int>()) { }
    public IntTuple(params int[] fields) => this.fields = fields;

    public static bool operator ==(IntTuple left, IntTuple right) => left.Equals(right);
    public static bool operator !=(IntTuple left, IntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is IntTuple tuple && Equals(tuple);
    public bool Equals(IntTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    static ISpaceTuple<int> ISpaceTuple<int>.Create(int[] fields) => new IntTuple(fields);
    ISpaceTemplate<int> ISpaceTuple<int>.ToTemplate()
    {
        ref int?[] fields = ref TupleHelpers.CastAs<int[], int?[]>(in this.fields);
        return new IntTemplate(fields);
    }

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Int);
    public ReadOnlySpan<int>.Enumerator GetEnumerator() => new ReadOnlySpan<int>(fields).GetEnumerator();
}

public readonly record struct IntTemplate : ISpaceTemplate<int>
{
    private readonly int?[] fields;

    public ref readonly int? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public IntTemplate([AllowNull] params int?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new int?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<int>
        => TupleHelpers.Matches<int, IntTuple>(this, tuple);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<int?>.Enumerator GetEnumerator() => new ReadOnlySpan<int?>(fields).GetEnumerator();
}