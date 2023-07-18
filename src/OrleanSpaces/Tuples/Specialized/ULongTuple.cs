using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="ulong"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct ULongTuple :
    IEquatable<ULongTuple>,
    INumericTuple<ulong>, 
    ISpaceFactory<ulong, ULongTuple>,
    ISpaceConvertible<ulong, ULongTemplate>
{
    [Id(0), JsonProperty] private readonly ulong[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;
    [JsonIgnore] public bool IsEmpty => Length == 0;

    public ref readonly ulong this[int index] => ref fields[index];

    Span<ulong> INumericTuple<ulong>.Fields => fields.AsSpan();

    /// <summary>
    /// Initializes an empty tuple.
    /// </summary>
    public ULongTuple() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes an empty tuple.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public ULongTuple([AllowNull] params ulong[] fields)
        => this.fields = fields is null ? Array.Empty<ulong>() : fields;

    /// <summary>
    /// Returns a <see cref="ULongTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    /// <remarks><i>If <see cref="Length"/> is 0, than default <see cref="ULongTemplate"/> is created.</i></remarks>
    public ULongTemplate ToTemplate()
    {
        int length = Length;
        ulong?[] fields = new ulong?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new ULongTemplate(fields);
    }

    public bool Equals(ULongTuple other)
        => this.TryParallelEquals<ulong, ULongTuple>(other, out bool result) ? 
               result : this.SequentialEquals<ulong, ULongTuple>(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    static ULongTuple ISpaceFactory<ulong, ULongTuple>.Create(ulong[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan<ulong, ULongTuple>(Constants.MaxFieldCharLength_ULong);
    public ReadOnlySpan<ulong>.Enumerator GetEnumerator() => new ReadOnlySpan<ulong>(fields).GetEnumerator();
}

/// <summary>
/// Represents a template which has <see cref="ulong"/> field types only.
/// </summary>
public readonly record struct ULongTemplate : 
    IEquatable<ULongTemplate>,
    ISpaceTemplate<ulong>, 
    ISpaceMatchable<ulong, ULongTuple>
{
    private readonly ulong?[] fields;

    public ref readonly ulong? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Initializes a template with a single <see langword="null"/> field.
    /// </summary>
    public ULongTemplate() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes a template with a single <see langword="null"/> field.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    public ULongTemplate([AllowNull] params ulong?[] fields)
        => this.fields = fields is null || fields.Length == 0 ? new ulong?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(ULongTuple tuple) => this.Matches<ulong, ULongTuple, ULongTemplate>(tuple);
    public bool Equals(ULongTemplate other) => this.SequentialEquals<ulong, ULongTemplate>(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TemplateHelpers.ToString(fields);

    public ReadOnlySpan<ulong?>.Enumerator GetEnumerator() => new ReadOnlySpan<ulong?>(fields).GetEnumerator();
}
