using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="char"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct CharTuple :
    IEquatable<CharTuple>,
    ISpaceTuple<char>, 
    ISpaceFactory<char, CharTuple>,
    ISpaceConvertible<char, CharTemplate>
{
    [Id(0), JsonProperty] private readonly char[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;

    public ref readonly char this[int index] => ref fields[index];

    /// <summary>
    /// Initializes an empty tuple.
    /// </summary>
    public CharTuple() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes an empty tuple.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public CharTuple([AllowNull] params char[] fields)
        => this.fields = fields is null ? Array.Empty<char>() : fields;

    /// <summary>
    /// Returns a <see cref="CharTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    public CharTemplate ToTemplate()
    {
        int length = Length;
        char?[] fields = new char?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new CharTemplate(fields);
    }

    public bool Equals(CharTuple other)
    {
        // Since 'char' is not a number (doesn't implement INumber<>), we are transforming it into a type which does implement INumber<>.
        // The sizeof(char) = 2 Bytes, therefor it can be represented by many number types, but the lowest possible (the one that provides the best parallelization)
        // number type that can safley represent any type of 'char', is 'ushort'. This is because the range of 'char' is U+0000 to U+FFFF.

        // In systems where 128-bit vector operations are subject to hardware acceleration, a total of 8 operations can be performed on 'ushort's
        //      128 / (2 * 8) = 128 / 16 = 8  --> means: we can compare 8 chars at the same time!

        // In systems where 256-bit vector operations are subject to hardware acceleration, a total of 16 operations can be performed on 'ushort's
        //      256 / (2 * 8) = 256 / 16 = 16 --> means: we can compare 16 chars at the same time!

        NumericMarshaller<char, ushort> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static CharTuple ISpaceFactory<char, CharTuple>.Create(char[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Char);
    public ReadOnlySpan<char>.Enumerator GetEnumerator() => new ReadOnlySpan<char>(fields).GetEnumerator();
}

/// <summary>
/// Represents a template which has <see cref="char"/> field types only.
/// </summary>
public readonly record struct CharTemplate :
    IEquatable<CharTemplate>,
    ISpaceTemplate<char>,
    ISpaceMatchable<char, CharTuple>
{
    private readonly char?[] fields;

    public ref readonly char? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Initializes a template with a single <see langword="null"/> field.
    /// </summary>
    public CharTemplate() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes a template with a single <see langword="null"/> field.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    public CharTemplate([AllowNull] params char?[] fields)
        => this.fields = fields is null || fields.Length == 0 ? new char?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(CharTuple tuple) => this.Matches<char, CharTuple>(tuple);
    public bool Equals(CharTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<char?>.Enumerator GetEnumerator() => new ReadOnlySpan<char?>(fields).GetEnumerator();
}
