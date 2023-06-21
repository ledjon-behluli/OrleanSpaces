using Orleans.Concurrency;
using OrleanSpaces;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct UShortTuple : INumericTuple<ushort>, IEquatable<UShortTuple>
{
    private readonly ushort[] fields;

    public ref readonly ushort this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<ushort> INumericTuple<ushort>.Fields => fields.AsSpan();

    public UShortTuple() : this(Array.Empty<ushort>()) { }
    public UShortTuple(params ushort[] fields) => this.fields = fields;

    public static bool operator ==(UShortTuple left, UShortTuple right) => left.Equals(right);
    public static bool operator !=(UShortTuple left, UShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UShortTuple tuple && Equals(tuple);
    public bool Equals(UShortTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_UShort);

    public ReadOnlySpan<ushort>.Enumerator GetEnumerator() => new ReadOnlySpan<ushort>(fields).GetEnumerator();
}

[Immutable]
public readonly struct UShortTemplate : ISpaceTemplate<ushort>
{
    private readonly ushort?[] fields;

    public ref readonly ushort? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public UShortTemplate([AllowNull] params ushort?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new ushort?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<ushort>
        => TupleHelpers.Matches(this, tuple);

    ISpaceTuple<ushort> ISpaceTemplate<ushort>.Create(ushort[] fields) => new UShortTuple(fields);

    public ReadOnlySpan<ushort?>.Enumerator GetEnumerator() => new ReadOnlySpan<ushort?>(fields).GetEnumerator();
}
