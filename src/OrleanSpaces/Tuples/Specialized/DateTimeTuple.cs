using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="DateTime"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct DateTimeTuple :
    IEquatable<DateTimeTuple>,
    ISpaceTuple<DateTime>,
    ISpaceFactory<DateTime, DateTimeTuple>,
    ISpaceConvertible<DateTime, DateTimeTemplate>
{
    [Id(0), JsonProperty] private readonly DateTime[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;
    [JsonIgnore] public bool IsEmpty => Length == 0;

    public ref readonly DateTime this[int index] => ref fields[index];

    /// <summary>
    /// Initializes an empty tuple.
    /// </summary>
    public DateTimeTuple() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes an empty tuple.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public DateTimeTuple([AllowNull] params DateTime[] fields)
        => this.fields = fields is null ? Array.Empty<DateTime>() : fields;

    /// <summary>
    /// Returns a <see cref="DateTimeTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    /// <remarks><i>If <see cref="Length"/> is 0, than default <see cref="DateTimeTemplate"/> is created.</i></remarks>
    public DateTimeTemplate ToTemplate()
    {
        int length = Length;
        DateTime?[] fields = new DateTime?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new DateTimeTemplate(fields);
    }

    public bool Equals(DateTimeTuple other)
    {
        NumericMarshaller<DateTime, long> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static DateTimeTuple ISpaceFactory<DateTime, DateTimeTuple>.Create(DateTime[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_DateTime);
    public ReadOnlySpan<DateTime>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTime>(fields).GetEnumerator();
}

/// <summary>
/// Represents a template which has <see cref="DateTime"/> field types only.
/// </summary>
public readonly record struct DateTimeTemplate : 
    IEquatable<DateTimeTemplate>,
    ISpaceTemplate<DateTime>, 
    ISpaceMatchable<DateTime, DateTimeTuple>
{
    private readonly DateTime?[] fields;

    public ref readonly DateTime? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Initializes a template with a single <see langword="null"/> field.
    /// </summary>
    public DateTimeTemplate() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes a template with a single <see langword="null"/> field.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    public DateTimeTemplate([AllowNull] params DateTime?[] fields) =>
        this.fields = fields is null || fields.Length == 0 ? new DateTime?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(DateTimeTuple tuple) => this.Matches<DateTime, DateTimeTuple>(tuple);
    public bool Equals(DateTimeTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<DateTime?>.Enumerator GetEnumerator() => new ReadOnlySpan<DateTime?>(fields).GetEnumerator();
}
