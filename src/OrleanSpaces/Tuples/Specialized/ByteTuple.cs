using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly record struct ByteTuple :
    IEquatable<ByteTuple>,
    INumericTuple<byte>,
    ISpaceFactory<byte, ByteTuple>,
    ISpaceConvertible<byte, ByteTemplate>
{
    [Id(0), JsonProperty] private readonly byte[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;

    public ref readonly byte this[int index] => ref fields[index];

    Span<byte> INumericTuple<byte>.Fields => fields.AsSpan();

    public ByteTuple() => fields = Array.Empty<byte>();
    public ByteTuple([AllowNull] params byte[] fields)
        => this.fields = fields is null ? Array.Empty<byte>() : fields;

    public ByteTemplate ToTemplate()
    {
        int length = Length;
        byte?[] fields = new byte?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new ByteTemplate(fields);
    }

    public bool Equals(ByteTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static ByteTuple ISpaceFactory<byte, ByteTuple>.Create(byte[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Byte);
    public ReadOnlySpan<byte>.Enumerator GetEnumerator() => new ReadOnlySpan<byte>(fields).GetEnumerator();
}

public readonly record struct ByteTemplate : 
    IEquatable<ByteTemplate>,
    ISpaceTemplate<byte>, 
    ISpaceMatchable<byte, ByteTuple>
{
    private readonly byte?[] fields;

    public ref readonly byte? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    public ByteTemplate() => fields = Array.Empty<byte?>();
    public ByteTemplate([AllowNull] params byte?[] fields)
        => this.fields = fields is null ? Array.Empty<byte?>() : fields;

    public bool Matches(ByteTuple tuple) => this.Matches<byte, ByteTuple>(tuple);
    public bool Equals(ByteTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);
    
    public ReadOnlySpan<byte?>.Enumerator GetEnumerator() => new ReadOnlySpan<byte?>(fields).GetEnumerator();
}
