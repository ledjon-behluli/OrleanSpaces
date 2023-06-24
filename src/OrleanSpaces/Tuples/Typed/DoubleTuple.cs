using OrleanSpaces;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[GenerateSerializer, Immutable]
public readonly struct DoubleTuple : INumericTuple<double>, IEquatable<DoubleTuple>
{
    private readonly double[] fields;

    public ref readonly double this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<double> INumericTuple<double>.Fields => fields.AsSpan();

    public DoubleTuple() : this(Array.Empty<double>()) { }
    public DoubleTuple(params double[] fields) => this.fields = fields;

    public static bool operator ==(DoubleTuple left, DoubleTuple right) => left.Equals(right);
    public static bool operator !=(DoubleTuple left, DoubleTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is DoubleTuple tuple && Equals(tuple);
    public bool Equals(DoubleTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Double);

    public ReadOnlySpan<double>.Enumerator GetEnumerator() => new ReadOnlySpan<double>(fields).GetEnumerator();
}

[GenerateSerializer, Immutable]
public readonly struct DoubleTemplate : ISpaceTemplate<double>
{
    private readonly double?[] fields;

    public ref readonly double? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public DoubleTemplate([AllowNull] params double?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new double?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<double>
        => TupleHelpers.Matches(this, tuple);

    ISpaceTuple<double> ISpaceTemplate<double>.Create(double[] fields) => new DoubleTuple(fields);

    public ReadOnlySpan<double?>.Enumerator GetEnumerator() => new ReadOnlySpan<double?>(fields).GetEnumerator();
}
