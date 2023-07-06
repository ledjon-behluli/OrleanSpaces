using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[GenerateSerializer, Immutable]
public readonly struct ByteTuple : INumericTuple<byte>, IEquatable<ByteTuple>
{
    [Id(0), JsonProperty] private readonly byte[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly byte this[int index] => ref fields[index];

    Span<byte> INumericTuple<byte>.Fields => fields.AsSpan();

    public ByteTuple() => fields = Array.Empty<byte>();
    public ByteTuple([AllowNull] params byte[] fields) 
        => this.fields = fields is null ? Array.Empty<byte>() : fields;

    public static bool operator ==(ByteTuple left, ByteTuple right) => left.Equals(right);
    public static bool operator !=(ByteTuple left, ByteTuple right) => !(left == right);

    public static explicit operator ByteTemplate(ByteTuple tuple)
    {
        ref byte?[] fields = ref TupleHelpers.CastAs<byte[], byte?[]>(in tuple.fields);
        return new ByteTemplate(fields);
    }

    public override bool Equals(object? obj) => obj is ByteTuple tuple && Equals(tuple);
    public bool Equals(ByteTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    ISpaceTemplate<byte> ISpaceTuple<byte>.ToTemplate() => (ByteTemplate)this;
    static ISpaceTuple<byte> ISpaceTuple<byte>.Create(byte[] fields) => new ByteTuple(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Byte);
    public ReadOnlySpan<byte>.Enumerator GetEnumerator() => new ReadOnlySpan<byte>(fields).GetEnumerator();
}

public readonly record struct ByteTemplate : ISpaceTemplate<byte>
{
    private readonly byte?[] fields;

    public ref readonly byte? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public ByteTemplate() => fields = Array.Empty<byte?>();
    public ByteTemplate([AllowNull] params byte?[] fields) 
        => this.fields = fields is null ? Array.Empty<byte?>() : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<byte>
        => TupleHelpers.Matches<byte, ByteTuple>(this, tuple);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<byte?>.Enumerator GetEnumerator() => new ReadOnlySpan<byte?>(fields).GetEnumerator();
}
