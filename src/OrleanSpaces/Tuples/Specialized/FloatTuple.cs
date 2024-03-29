﻿using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="float"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct FloatTuple :
    IEquatable<FloatTuple>,
    IEqualityOperators<FloatTuple, FloatTuple, bool>,
    INumericTuple<float>, 
    ISpaceFactory<float, FloatTuple>,
    ISpaceConvertible<float, FloatTemplate>
{
    [Id(0), JsonProperty] private readonly float[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;
    [JsonIgnore] public bool IsEmpty => Length == 0;

    public ref readonly float this[int index] => ref fields[index];

    Span<float> INumericTuple<float>.Fields => fields.AsSpan();

    /// <summary>
    /// Initializes an empty tuple.
    /// </summary>
    public FloatTuple() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes an empty tuple.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public FloatTuple([AllowNull] params float[] fields)
        => this.fields = fields is null ? Array.Empty<float>() : fields;

    /// <summary>
    /// Returns a <see cref="FloatTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    /// <remarks><i>If <see cref="Length"/> is 0, than default <see cref="FloatTemplate"/> is created.</i></remarks>
    public FloatTemplate ToTemplate()
    {
        int length = Length;
        float?[] fields = new float?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new FloatTemplate(fields);
    }

    public bool Equals(FloatTuple other)
        => this.TryParallelEquals<float, FloatTuple>(other, out bool result) ?
                result : this.SequentialEquals<float, FloatTuple>(other);

    public override int GetHashCode() => fields?.GetHashCode() ?? 0;
    public override string ToString() => TupleHelpers.ToString(fields);

    static FloatTuple ISpaceFactory<float, FloatTuple>.Create(float[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan<float, FloatTuple>(Constants.MaxFieldCharLength_Float);
    public ReadOnlySpan<float>.Enumerator GetEnumerator() => 
        (fields is null ? ReadOnlySpan<float>.Empty : new ReadOnlySpan<float>(fields)).GetEnumerator();
}

/// <summary>
/// Represents a template which has <see cref="float"/> field types only.
/// </summary>
public readonly record struct FloatTemplate : 
    IEquatable<FloatTemplate>,
    IEqualityOperators<FloatTemplate, FloatTemplate, bool>,
    ISpaceTemplate<float>,
    ISpaceMatchable<float, FloatTuple>
{
    private readonly float?[] fields;

    public ref readonly float? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Initializes a template with a single <see langword="null"/> field.
    /// </summary>
    public FloatTemplate() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes a template with a single <see langword="null"/> field.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    public FloatTemplate([AllowNull] params float?[] fields)
        => this.fields = fields is null || fields.Length == 0 ? new float?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(FloatTuple tuple) => this.Matches<float, FloatTuple, FloatTemplate>(tuple);
    public bool Equals(FloatTemplate other) => this.SequentialEquals<float, FloatTemplate>(other);

    public override int GetHashCode() => fields?.GetHashCode() ?? 0;
    public override string ToString() => TemplateHelpers.ToString(fields);

    public ReadOnlySpan<float?>.Enumerator GetEnumerator() => 
        (fields is null ? ReadOnlySpan<float?>.Empty : new ReadOnlySpan<float?>(fields)).GetEnumerator();
}
