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
    [JsonIgnore] public bool IsEmpty => Length == 0;

    public ref readonly sbyte this[int index] => ref fields[index];

    Span<sbyte> INumericTuple<sbyte>.Fields => fields.AsSpan();

    /// <summary>
    /// Initializes an empty tuple.
    /// </summary>
    public SByteTuple() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes an empty tuple.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public SByteTuple([AllowNull] params sbyte[] fields)
        => this.fields = fields is null ? Array.Empty<sbyte>() : fields;

    /// <summary>
    /// Returns a <see cref="SByteTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    /// <remarks><i>If <see cref="Length"/> is 0, than default <see cref="SByteTemplate"/> is created.</i></remarks>
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
        => this.TryParallelEquals<sbyte, SByteTuple>(other, out bool result) ?
               result : this.SequentialEquals<sbyte, SByteTuple>(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    static SByteTuple ISpaceFactory<sbyte, SByteTuple>.Create(sbyte[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan<sbyte, SByteTuple>(Constants.MaxFieldCharLength_SByte);
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
    /// Initializes a template with a single <see langword="null"/> field.
    /// </summary>
    public SByteTemplate() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes a template with a single <see langword="null"/> field.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    public SByteTemplate([AllowNull] params sbyte?[] fields)
        => this.fields = fields is null || fields.Length == 0 ? new sbyte?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(SByteTuple tuple) => this.Matches<sbyte, SByteTuple, SByteTemplate>(tuple);
    public bool Equals(SByteTemplate other) => this.SequentialEquals<sbyte, SByteTemplate>(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TemplateHelpers.ToString(fields);

    public ReadOnlySpan<sbyte?>.Enumerator GetEnumerator() => new ReadOnlySpan<sbyte?>(fields).GetEnumerator();
}

