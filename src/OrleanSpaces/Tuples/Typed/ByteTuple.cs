using OrleanSpaces;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[GenerateSerializer, Immutable]
public readonly struct ByteTuple : INumericTuple<byte>, IEquatable<ByteTuple>
{
    [Id(0)] private readonly byte[] fields;

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

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Byte);

    public ReadOnlySpan<byte>.Enumerator GetEnumerator() => new ReadOnlySpan<byte>(fields).GetEnumerator();
}

public readonly record struct ByteTemplate : ISpaceTemplate<byte>
{
    private readonly byte?[] fields;

    public ref readonly byte? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public ByteTemplate([AllowNull] params byte?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new byte?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<byte>
        => TupleHelpers.Matches(this, tuple);

    ISpaceTuple<byte> ISpaceTemplate<byte>.Create(byte[] fields) => new ByteTuple(fields);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<byte?>.Enumerator GetEnumerator() => new ReadOnlySpan<byte?>(fields).GetEnumerator();
}
