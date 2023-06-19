using Orleans.Concurrency;
using OrleanSpaces.Tuples;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct IntTuple : INumericTuple<int>, IEquatable<IntTuple>
{
    private readonly int[] fields;

    public ref readonly int this[int index] => ref fields[index];
    public int Length => fields.Length;
    public static IntTuple Empty => new();

    Span<int> INumericTuple<int>.Fields => fields.AsSpan();

    public IntTuple() : this(Array.Empty<int>()) { }
    public IntTuple(params int[] fields) => this.fields = fields;

    public static bool operator ==(IntTuple left, IntTuple right) => left.Equals(right);
    public static bool operator !=(IntTuple left, IntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is IntTuple tuple && Equals(tuple);
    public bool Equals(IntTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Int);
    public ReadOnlySpan<int>.Enumerator GetEnumerator() => new ReadOnlySpan<int>(fields).GetEnumerator();
}

[Immutable]
public readonly struct IntTemplate : ISpaceTemplate<int>
{
    private readonly int?[] fields;

    public ref readonly int? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public IntTemplate([AllowNull] params int?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new int?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<int>
        => Helpers.Matches(this, tuple);

    ISpaceTuple<int> ISpaceTemplate<int>.Create(int[] fields) => new IntTuple(fields);

    public ReadOnlySpan<int?>.Enumerator GetEnumerator() => new ReadOnlySpan<int?>(fields).GetEnumerator();


    public static implicit operator IntTemplate(IntTuple tuple)
    {
        int length = tuple.Length;
        int[] fields = new int[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = tuple[i];
        }

        return new IntTuple(fields);
    }
}