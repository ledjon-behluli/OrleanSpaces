using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="int"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct IntTuple :
    IEquatable<IntTuple>,
    INumericTuple<int>, 
    ISpaceFactory<int, IntTuple>,
    ISpaceConvertible<int, IntTemplate>
{
    [Id(0), JsonProperty] private readonly int[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;

    public ref readonly int this[int index] => ref fields[index];

    Span<int> INumericTuple<int>.Fields => fields.AsSpan();

    /// <summary>
    /// Default constructor which instantiates an empty tuple. 
    /// </summary>
    public IntTuple() => fields = Array.Empty<int>();

    /// <summary>
    /// Main constructor which instantiates a non-empty tuple, when at least one field is supplied, otherwise an empty tuple is instantiated.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public IntTuple([AllowNull] params int[] fields)
        => this.fields = fields is null ? Array.Empty<int>() : fields;

    /// <summary>
    /// Returns a <see cref="IntTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    public IntTemplate ToTemplate()
    {
        int length = Length;
        int?[] fields = new int?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new IntTemplate(fields);
    }

    public bool Equals(IntTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static IntTuple ISpaceFactory<int, IntTuple>.Create(int[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Int);
    public ReadOnlySpan<int>.Enumerator GetEnumerator() => new ReadOnlySpan<int>(fields).GetEnumerator();
}

/// <summary>
/// Represents a template which has <see cref="int"/> field types only.
/// </summary>
public readonly record struct IntTemplate : 
    IEquatable<IntTemplate>,
    ISpaceTemplate<int>, 
    ISpaceMatchable<int, IntTuple>
{
    private readonly int?[] fields;

    public ref readonly int? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Default constructor which instantiates an empty template. 
    /// </summary>
    public IntTemplate() => fields = Array.Empty<int?>();

    /// <summary>
    /// Main constructor which instantiates a non-empty template.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    /// <remarks><i>If <paramref name="fields"/> is <see langword="null"/>, a template with a single <see langword="null"/> field is returned.</i></remarks>
    public IntTemplate([AllowNull] params int?[] fields)
        => this.fields = fields is null ? new int?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(IntTuple tuple) => this.Matches<int, IntTuple>(tuple);
    public bool Equals(IntTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<int?>.Enumerator GetEnumerator() => new ReadOnlySpan<int?>(fields).GetEnumerator();
}