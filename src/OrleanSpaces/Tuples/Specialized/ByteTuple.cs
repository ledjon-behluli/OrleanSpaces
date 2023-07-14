using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="byte"/> field types only.
/// </summary>
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

    /// <summary>
    /// Initializes an empty tuple.
    /// </summary>
    public ByteTuple() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes an empty tuple.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public ByteTuple([AllowNull] params byte[] fields)
        => this.fields = fields is null ? Array.Empty<byte>() : fields;

    /// <summary>
    /// Returns a <see cref="ByteTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
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

/// <summary>
/// Represents a template which has <see cref="byte"/> field types only.
/// </summary>
public readonly record struct ByteTemplate : 
    IEquatable<ByteTemplate>,
    ISpaceTemplate<byte>, 
    ISpaceMatchable<byte, ByteTuple>
{
    private readonly byte?[] fields;

    public ref readonly byte? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Initializes a template with a single <see langword="null"/> field.
    /// </summary>
    public ByteTemplate() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes a template with a single <see langword="null"/> field.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    public ByteTemplate([AllowNull] params byte?[] fields)
        => this.fields = fields is null || fields.Length == 0 ? new byte?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(ByteTuple tuple) => this.Matches<byte, ByteTuple>(tuple);
    public bool Equals(ByteTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);
    
    public ReadOnlySpan<byte?>.Enumerator GetEnumerator() => new ReadOnlySpan<byte?>(fields).GetEnumerator();
}
