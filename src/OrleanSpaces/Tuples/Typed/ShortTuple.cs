using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct ShortTuple : INumericTuple<short>, IEquatable<ShortTuple>, IComparable<ShortTuple>
{
    private readonly short[] fields;

    public ref readonly short this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<short> INumericTuple<short>.Fields => fields.AsSpan();

    public ShortTuple() : this(Array.Empty<short>()) { }
    public ShortTuple(params short[] fields) => this.fields = fields;

    public static bool operator ==(ShortTuple left, ShortTuple right) => left.Equals(right);
    public static bool operator !=(ShortTuple left, ShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ShortTuple tuple && Equals(tuple);
    public bool Equals(ShortTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(ShortTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Short);

    public ReadOnlySpan<short>.Enumerator GetEnumerator() => new ReadOnlySpan<short>(fields).GetEnumerator();
}

[Immutable]
public readonly struct ShortTemplate : ISpaceTemplate<short>
{
    private readonly short?[] fields;

    public ref readonly short? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public ShortTemplate([AllowNull] params short?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new short?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<short>
        => Helpers.Matches(this, tuple);

    ISpaceTuple<short> ISpaceTemplate<short>.Create(short[] fields) => new ShortTuple(fields);
}
