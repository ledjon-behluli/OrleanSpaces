using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct UIntTuple : INumericTuple<uint, UIntTuple>, IFieldFormater<uint>
{
    private readonly uint[] fields;

    public uint this[int index] => fields[index];
    public Span<uint> Fields => fields.AsSpan();
    public int Length => fields.Length;

    public UIntTuple() : this(Array.Empty<uint>()) { }
    public UIntTuple(params uint[] fields) => this.fields = fields;

    public static bool operator ==(UIntTuple left, UIntTuple right) => left.Equals(right);
    public static bool operator !=(UIntTuple left, UIntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UIntTuple tuple && Equals(tuple);
    public bool Equals(UIntTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(UIntTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => this.TryFormatTuple(destination, out charsWritten);

    Span<uint> INumericTuple<uint, UIntTuple>.Fields => fields.AsSpan();

    static int IFieldFormater<uint>.MaxCharsWrittable => 11;

    static bool IFieldFormater<uint>.TryFormat(uint field, Span<char> destination, out int charsWritten)
        => field.TryFormat(destination, out charsWritten);

    public override string ToString() => $"({string.Join(", ", fields)})";
}
