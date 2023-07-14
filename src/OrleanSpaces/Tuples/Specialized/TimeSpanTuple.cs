using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="TimeSpan"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct TimeSpanTuple :
    IEquatable<TimeSpanTuple>,
    ISpaceTuple<TimeSpan>, 
    ISpaceFactory<TimeSpan, TimeSpanTuple>,
    ISpaceConvertible<TimeSpan, TimeSpanTemplate>
{
    [Id(0), JsonProperty] private readonly TimeSpan[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;

    public ref readonly TimeSpan this[int index] => ref fields[index];

    /// <summary>
    /// Initializes an empty tuple.
    /// </summary>
    public TimeSpanTuple() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes an empty tuple.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public TimeSpanTuple([AllowNull] params TimeSpan[] fields)
        => this.fields = fields is null ? Array.Empty<TimeSpan>() : fields;

    /// <summary>
    /// Returns a <see cref="TimeSpanTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    public TimeSpanTemplate ToTemplate()
    {
        int length = Length;
        TimeSpan?[] fields = new TimeSpan?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new TimeSpanTemplate(fields);
    }

    public bool Equals(TimeSpanTuple other)
    {
        NumericMarshaller<TimeSpan, long> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static TimeSpanTuple ISpaceFactory<TimeSpan, TimeSpanTuple>.Create(TimeSpan[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_TimeSpan);
    public ReadOnlySpan<TimeSpan>.Enumerator GetEnumerator() => new ReadOnlySpan<TimeSpan>(fields).GetEnumerator();
}

/// <summary>
/// Represents a template which has <see cref="TimeSpan"/> field types only.
/// </summary>
public readonly record struct TimeSpanTemplate : 
    IEquatable<TimeSpanTemplate>,
    ISpaceTemplate<TimeSpan>, 
    ISpaceMatchable<TimeSpan, TimeSpanTuple>
{
    private readonly TimeSpan?[] fields;

    public ref readonly TimeSpan? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Initializes a template with a single <see langword="null"/> field.
    /// </summary>
    public TimeSpanTemplate() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes a template with a single <see langword="null"/> field.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    public TimeSpanTemplate([AllowNull] params TimeSpan?[] fields)
        => this.fields = fields is null || fields.Length == 0 ? new TimeSpan?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(TimeSpanTuple tuple) => this.Matches<TimeSpan, TimeSpanTuple>(tuple);
    public bool Equals(TimeSpanTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<TimeSpan?>.Enumerator GetEnumerator() => new ReadOnlySpan<TimeSpan?>(fields).GetEnumerator();
}