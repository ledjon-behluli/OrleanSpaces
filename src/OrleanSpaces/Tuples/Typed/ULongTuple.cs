using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct ULongTuple : INumericTuple<ulong>, IEquatable<ULongTuple>, IComparable<ULongTuple>
{
    private readonly ulong[] fields;

    public ref readonly ulong this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<ulong> INumericTuple<ulong>.Fields => fields.AsSpan();

    public ULongTuple() : this(Array.Empty<ulong>()) { }
    public ULongTuple(params ulong[] fields) => this.fields = fields;

    public static bool operator ==(ULongTuple left, ULongTuple right) => left.Equals(right);
    public static bool operator !=(ULongTuple left, ULongTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ULongTuple tuple && Equals(tuple);
    public bool Equals(ULongTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(ULongTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_ULong);

    public ReadOnlySpan<ulong>.Enumerator GetEnumerator() => new ReadOnlySpan<ulong>(fields).GetEnumerator();
}

[Immutable]
public readonly struct ULongTemplate : ISpaceTemplate<ulong>
{
    private readonly ulong?[] fields;

    public ref readonly ulong? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public ULongTemplate([AllowNull] params ulong?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new ulong?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<ulong>
        => Helpers.Matches(this, tuple);

    ISpaceTuple<ulong> ISpaceTemplate<ulong>.Create(ulong[] fields) => new ULongTuple(fields);
}
