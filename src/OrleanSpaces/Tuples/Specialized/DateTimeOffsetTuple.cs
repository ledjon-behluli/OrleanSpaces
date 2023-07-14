using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="DateTimeOffset"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct DateTimeOffsetTuple :
    IEquatable<DateTimeOffsetTuple>,
    ISpaceTuple<DateTimeOffset>, 
    ISpaceFactory<DateTimeOffset, DateTimeOffsetTuple>,
    ISpaceConvertible<DateTimeOffset, DateTimeOffsetTemplate>
{
    [Id(0), JsonProperty] private readonly DateTimeOffset[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;

    public ref readonly DateTimeOffset this[int index] => ref fields[index];

    /// <summary>
    /// Default constructor which instantiates an empty tuple. 
    /// </summary>
    public DateTimeOffsetTuple() => fields = Array.Empty<DateTimeOffset>();

    /// <summary>
    /// Main constructor which instantiates a non-empty tuple, when at least one field is supplied, otherwise an empty tuple is instantiated.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public DateTimeOffsetTuple([AllowNull] params DateTimeOffset[] fields)
        => this.fields = fields is null ? Array.Empty<DateTimeOffset>() : fields;

    /// <summary>
    /// Returns a <see cref="DateTimeOffsetTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    public DateTimeOffsetTemplate ToTemplate()
    {
        int length = Length;
        DateTimeOffset?[] fields = new DateTimeOffset?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new DateTimeOffsetTemplate(fields);
    }

    public bool Equals(DateTimeOffsetTuple other)
    {
        NumericMarshaller<DateTimeOffset, long> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static DateTimeOffsetTuple ISpaceFactory<DateTimeOffset, DateTimeOffsetTuple>.Create(DateTimeOffset[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_DateTimeOffset);
    public ReadOnlySpan<DateTimeOffset>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTimeOffset>(fields).GetEnumerator();
}

/// <summary>
/// Represents a template which has <see cref="DateTimeOffset"/> field types only.
/// </summary>
public readonly record struct DateTimeOffsetTemplate : 
    IEquatable<DateTimeOffsetTemplate>,
    ISpaceTemplate<DateTimeOffset>, 
    ISpaceMatchable<DateTimeOffset, DateTimeOffsetTuple>
{
    private readonly DateTimeOffset?[] fields;

    public ref readonly DateTimeOffset? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Default constructor which instantiates an empty template. 
    /// </summary>
    public DateTimeOffsetTemplate() => fields = Array.Empty<DateTimeOffset?>();

    /// <summary>
    /// Main constructor which instantiates a non-empty template.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    /// <remarks><i>If <paramref name="fields"/> is <see langword="null"/>, a template with a single <see langword="null"/> field is returned.</i></remarks>
    public DateTimeOffsetTemplate([AllowNull] params DateTimeOffset?[] fields) =>
        this.fields = fields is null ? new DateTimeOffset?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(DateTimeOffsetTuple tuple) => this.Matches<DateTimeOffset, DateTimeOffsetTuple>(tuple);
    public bool Equals(DateTimeOffsetTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<DateTimeOffset?>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTimeOffset?>(fields).GetEnumerator();
}
