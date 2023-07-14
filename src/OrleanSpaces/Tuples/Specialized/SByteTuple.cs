using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="sbyte"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct SByteTuple :
    IEquatable<SByteTuple>,
    INumericTuple<sbyte>, 
    ISpaceFactory<sbyte, SByteTuple>,
    ISpaceConvertible<sbyte, SByteTemplate>
{
    [Id(0), JsonProperty] private readonly sbyte[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;

    public ref readonly sbyte this[int index] => ref fields[index];

    Span<sbyte> INumericTuple<sbyte>.Fields => fields.AsSpan();

    /// <summary>
    /// Default constructor which instantiates an empty tuple. 
    /// </summary>
    public SByteTuple() => fields = Array.Empty<sbyte>();

    /// <summary>
    /// Main constructor which instantiates a non-empty tuple, when at least one field is supplied, otherwise an empty tuple is instantiated.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public SByteTuple([AllowNull] params sbyte[] fields)
        => this.fields = fields is null ? Array.Empty<sbyte>() : fields;

    /// <summary>
    /// Returns a <see cref="SByteTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    public SByteTemplate ToTemplate()
    {
        int length = Length;
        sbyte?[] fields = new sbyte?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new SByteTemplate(fields);
    }

    public bool Equals(SByteTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static SByteTuple ISpaceFactory<sbyte, SByteTuple>.Create(sbyte[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_SByte);
    public ReadOnlySpan<sbyte>.Enumerator GetEnumerator() => new ReadOnlySpan<sbyte>(fields).GetEnumerator();
}

/// <summary>
/// Represents a template which has <see cref="sbyte"/> field types only.
/// </summary>
public readonly record struct SByteTemplate : 
    IEquatable<SByteTemplate>,
    ISpaceTemplate<sbyte>, 
    ISpaceMatchable<sbyte, SByteTuple>
{
    private readonly sbyte?[] fields;

    public ref readonly sbyte? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Default constructor which instantiates an empty template. 
    /// </summary>
    public SByteTemplate() => fields = Array.Empty<sbyte?>();

    /// <summary>
    /// Main constructor which instantiates a non-empty template.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    /// <remarks><i>If <paramref name="fields"/> is <see langword="null"/>, a template with a single <see langword="null"/> field is returned.</i></remarks>
    public SByteTemplate([AllowNull] params sbyte?[] fields)
        => this.fields = fields is null ? new sbyte?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(SByteTuple tuple) => this.Matches<sbyte, SByteTuple>(tuple);
    public bool Equals(SByteTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<sbyte?>.Enumerator GetEnumerator() => new ReadOnlySpan<sbyte?>(fields).GetEnumerator();
}

