using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="short"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct ShortTuple :
    IEquatable<ShortTuple>,
    INumericTuple<short>, 
    ISpaceFactory<short, ShortTuple>,
    ISpaceConvertible<short, ShortTemplate>
{
    [Id(0), JsonProperty] private readonly short[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;

    public ref readonly short this[int index] => ref fields[index];

    Span<short> INumericTuple<short>.Fields => fields.AsSpan();

    /// <summary>
    /// Default constructor which instantiates an empty tuple. 
    /// </summary>
    public ShortTuple() => fields = Array.Empty<short>();

    /// <summary>
    /// Main constructor which instantiates a non-empty tuple, when at least one field is supplied, otherwise an empty tuple is instantiated.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public ShortTuple([AllowNull] params short[] fields)
        => this.fields = fields is null ? Array.Empty<short>() : fields;

    /// <summary>
    /// Returns a <see cref="ShortTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    public ShortTemplate ToTemplate()
    {
        int length = Length;
        short?[] fields = new short?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new ShortTemplate(fields);
    }

    public bool Equals(ShortTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static ShortTuple ISpaceFactory<short, ShortTuple>.Create(short[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Short);
    public ReadOnlySpan<short>.Enumerator GetEnumerator() => new ReadOnlySpan<short>(fields).GetEnumerator();
}

/// <summary>
/// Represents a template which has <see cref="short"/> field types only.
/// </summary>
public readonly record struct ShortTemplate :
    IEquatable<ShortTemplate>,
    ISpaceTemplate<short>,
    ISpaceMatchable<short, ShortTuple>
{
    private readonly short?[] fields;

    public ref readonly short? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Default constructor which instantiates an empty template. 
    /// </summary>
    public ShortTemplate() => fields = Array.Empty<short?>();

    /// <summary>
    /// Main constructor which instantiates a non-empty template.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    /// <remarks><i>If <paramref name="fields"/> is <see langword="null"/>, a template with a single <see langword="null"/> field is returned.</i></remarks>
    public ShortTemplate([AllowNull] params short?[] fields)
        => this.fields = fields is null ? new short?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(ShortTuple tuple) => this.Matches<short, ShortTuple>(tuple);
    public bool Equals(ShortTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<short?>.Enumerator GetEnumerator() => new ReadOnlySpan<short?>(fields).GetEnumerator();
}
