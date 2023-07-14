using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="uint"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct UIntTuple :
    IEquatable<UIntTuple>,
    INumericTuple<uint>, 
    ISpaceFactory<uint, UIntTuple>,
    ISpaceConvertible<uint, UIntTemplate>
{
    [Id(0), JsonProperty] private readonly uint[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;

    public ref readonly uint this[int index] => ref fields[index];

    Span<uint> INumericTuple<uint>.Fields => fields.AsSpan();

    /// <summary>
    /// Default constructor which instantiates an empty tuple. 
    /// </summary>
    public UIntTuple() => fields = Array.Empty<uint>();

    /// <summary>
    /// Main constructor which instantiates a non-empty tuple, when at least one field is supplied, otherwise an empty tuple is instantiated.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public UIntTuple([AllowNull] params uint[] fields)
        => this.fields = fields is null ? Array.Empty<uint>() : fields;

    /// <summary>
    /// Returns a <see cref="UIntTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    public UIntTemplate ToTemplate()
    {
        int length = Length;
        uint?[] fields = new uint?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new UIntTemplate(fields);
    }

    public bool Equals(UIntTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static UIntTuple ISpaceFactory<uint, UIntTuple>.Create(uint[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_UInt);
    public ReadOnlySpan<uint>.Enumerator GetEnumerator() => new ReadOnlySpan<uint>(fields).GetEnumerator();
}

/// <summary>
/// Represents a template which has <see cref="uint"/> field types only.
/// </summary>
public readonly record struct UIntTemplate :
    IEquatable<UIntTemplate>,
    ISpaceTemplate<uint>, 
    ISpaceMatchable<uint, UIntTuple>
{
    private readonly uint?[] fields;

    public ref readonly uint? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Default constructor which instantiates an empty template. 
    /// </summary>
    public UIntTemplate() => fields = Array.Empty<uint?>();

    /// <summary>
    /// Main constructor which instantiates a non-empty template.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    /// <remarks><i>If <paramref name="fields"/> is <see langword="null"/>, a template with a single <see langword="null"/> field is returned.</i></remarks>
    public UIntTemplate([AllowNull] params uint?[] fields)
        => this.fields = fields is null ? new uint?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(UIntTuple tuple) => this.Matches<uint, UIntTuple>(tuple);
    public bool Equals(UIntTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<uint?>.Enumerator GetEnumerator() => new ReadOnlySpan<uint?>(fields).GetEnumerator();
}