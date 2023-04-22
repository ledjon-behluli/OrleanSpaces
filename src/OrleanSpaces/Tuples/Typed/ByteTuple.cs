using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct ByteTuple : INumericTuple<byte, ByteTuple>, IFieldFormater<byte>
{
    private readonly byte[] fields;

    public byte this[int index] => fields[index];
    public int Length => fields.Length;

    public ByteTuple() : this(Array.Empty<byte>()) { }
    public ByteTuple(params byte[] fields) => this.fields = fields;

    public static bool operator ==(ByteTuple left, ByteTuple right) => left.Equals(right);
    public static bool operator !=(ByteTuple left, ByteTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ByteTuple tuple && Equals(tuple);
    public bool Equals(ByteTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(ByteTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => this.TryFormatTuple(destination, out charsWritten);

    Span<byte> INumericTuple<byte, ByteTuple>.Fields => fields.AsSpan();

    static int IFieldFormater<byte>.MaxCharsWrittable => 11;

    static bool IFieldFormater<byte>.TryFormat(byte field, Span<char> destination, out int charsWritten)
        => field.TryFormat(destination, out charsWritten);

    public override string ToString() => $"({string.Join(", ", fields)})";
}