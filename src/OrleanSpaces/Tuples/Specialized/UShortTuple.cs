using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="ushort"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct UShortTuple :
    IEquatable<UShortTuple>,
    INumericTuple<ushort>, 
    ISpaceFactory<ushort, UShortTuple>,
    ISpaceConvertible<ushort, UShortTemplate>
{
    [Id(0), JsonProperty] private readonly ushort[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;

    public ref readonly ushort this[int index] => ref fields[index];

    Span<ushort> INumericTuple<ushort>.Fields => fields.AsSpan();

    /// <summary>
    /// Default constructor which instantiates an empty tuple. 
    /// </summary>
    public UShortTuple() => fields = Array.Empty<ushort>();

    /// <summary>
    /// Main constructor which instantiates a non-empty tuple, when at least one field is supplied, otherwise an empty tuple is instantiated.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public UShortTuple([AllowNull] params ushort[] fields)
        => this.fields = fields is null ? Array.Empty<ushort>() : fields;

    /// <summary>
    /// Returns a <see cref="UShortTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    public UShortTemplate ToTemplate()
    {
        int length = Length;
        ushort?[] fields = new ushort?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new UShortTemplate(fields);
    }

    public bool Equals(UShortTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static UShortTuple ISpaceFactory<ushort, UShortTuple>.Create(ushort[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_UShort);
    public ReadOnlySpan<ushort>.Enumerator GetEnumerator() => new ReadOnlySpan<ushort>(fields).GetEnumerator();
}

/// <summary>
/// Represents a template which has <see cref="ushort"/> field types only.
/// </summary>
public readonly record struct UShortTemplate :
    IEquatable<UShortTemplate>,
    ISpaceTemplate<ushort>, 
    ISpaceMatchable<ushort, UShortTuple>
{
    private readonly ushort?[] fields;

    public ref readonly ushort? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Default constructor which instantiates an empty template. 
    /// </summary>
    public UShortTemplate() => fields = Array.Empty<ushort?>();

    /// <summary>
    /// Main constructor which instantiates a non-empty template.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    /// <remarks><i>If <paramref name="fields"/> is <see langword="null"/>, a template with a single <see langword="null"/> field is returned.</i></remarks>
    public UShortTemplate([AllowNull] params ushort?[] fields)
        => this.fields = fields is null ? new ushort?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(UShortTuple tuple) => this.Matches<ushort, UShortTuple>(tuple);
    public bool Equals(UShortTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<ushort?>.Enumerator GetEnumerator() => new ReadOnlySpan<ushort?>(fields).GetEnumerator();
}
