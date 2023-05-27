using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct ByteTuple : INumericTuple<byte>, IEquatable<ByteTuple>, IComparable<ByteTuple>
{
    private readonly byte[] fields;

    public ref readonly byte this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<byte> INumericTuple<byte>.Fields => fields.AsSpan();

    public ByteTuple() : this(Array.Empty<byte>()) { }
    public ByteTuple(params byte[] fields) => this.fields = fields;

    public static bool operator ==(ByteTuple left, ByteTuple right) => left.Equals(right);
    public static bool operator !=(ByteTuple left, ByteTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ByteTuple tuple && Equals(tuple);
    public bool Equals(ByteTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(ByteTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Byte);

    public ReadOnlySpan<byte>.Enumerator GetEnumerator() => new ReadOnlySpan<byte>(fields).GetEnumerator();
}

[Immutable]
public readonly struct SpaceByte
{
    public readonly byte Value;

    internal static readonly SpaceByte Default = new();

    public SpaceByte(byte value) => Value = value;

    public static implicit operator SpaceByte(byte value) => new(value);
    public static implicit operator byte(SpaceByte value) => value.Value;
}

[Immutable]
public readonly struct ByteTemplate : ISpaceTemplate<ByteTuple>
{
    private readonly SpaceByte[] fields;

    public ByteTemplate([AllowNull] params SpaceByte[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new SpaceByte[1] { new SpaceUnit() } : fields;
}
