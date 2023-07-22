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
    [JsonIgnore] public bool IsEmpty => Length == 0;

    public ref readonly int this[int index] => ref fields[index];

    Span<int> INumericTuple<int>.Fields => fields.AsSpan();

    /// <summary>
    /// Initializes an empty tuple.
    /// </summary>
    public IntTuple() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes an empty tuple.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public IntTuple([AllowNull] params int[] fields)
        => this.fields = fields is null ? Array.Empty<int>() : fields;

    /// <summary>
    /// Returns a <see cref="IntTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    /// <remarks><i>If <see cref="Length"/> is 0, than default <see cref="IntTemplate"/> is created.</i></remarks>
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
        => this.TryParallelEquals<int, IntTuple>(other, out bool result) ?
               result : this.SequentialEquals<int, IntTuple>(other);

    public override int GetHashCode() => fields?.GetHashCode() ?? 0;
    public override string ToString() => TupleHelpers.ToString(fields);

    static IntTuple ISpaceFactory<int, IntTuple>.Create(int[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan<int, IntTuple>(Constants.MaxFieldCharLength_Int);
    public ReadOnlySpan<int>.Enumerator GetEnumerator() => 
        (fields is null ? ReadOnlySpan<int>.Empty : new ReadOnlySpan<int>(fields)).GetEnumerator();
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
    /// Initializes a template with a single <see langword="null"/> field.
    /// </summary>
    public IntTemplate() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes a template with a single <see langword="null"/> field.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    public IntTemplate([AllowNull] params int?[] fields)
        => this.fields = fields is null || fields.Length == 0 ? new int?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(IntTuple tuple) => this.Matches<int, IntTuple, IntTemplate>(tuple);
    public bool Equals(IntTemplate other) => this.SequentialEquals<int, IntTemplate>(other);

    public override int GetHashCode() => fields?.GetHashCode() ?? 0;
    public override string ToString() => TemplateHelpers.ToString(fields);

    public ReadOnlySpan<int?>.Enumerator GetEnumerator() => 
        (fields is null ? ReadOnlySpan<int?>.Empty : new ReadOnlySpan<int?>(fields)).GetEnumerator();
}