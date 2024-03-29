﻿using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="ushort"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct UShortTuple :
    IEquatable<UShortTuple>,
    IEqualityOperators<UShortTuple, UShortTuple, bool>,
    INumericTuple<ushort>, 
    ISpaceFactory<ushort, UShortTuple>,
    ISpaceConvertible<ushort, UShortTemplate>
{
    [Id(0), JsonProperty] private readonly ushort[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;
    [JsonIgnore] public bool IsEmpty => Length == 0;

    public ref readonly ushort this[int index] => ref fields[index];

    Span<ushort> INumericTuple<ushort>.Fields => fields.AsSpan();

    /// <summary>
    /// Initializes an empty tuple.
    /// </summary>
    public UShortTuple() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes an empty tuple.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public UShortTuple([AllowNull] params ushort[] fields)
        => this.fields = fields is null ? Array.Empty<ushort>() : fields;

    /// <summary>
    /// Returns a <see cref="UShortTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    /// <remarks><i>If <see cref="Length"/> is 0, than default <see cref="UShortTemplate"/> is created.</i></remarks>
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
        => this.TryParallelEquals<ushort, UShortTuple>(other, out bool result) ?
               result : this.SequentialEquals<ushort, UShortTuple>(other);

    public override int GetHashCode() => fields?.GetHashCode() ?? 0;
    public override string ToString() => TupleHelpers.ToString(fields);

    static UShortTuple ISpaceFactory<ushort, UShortTuple>.Create(ushort[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan<ushort, UShortTuple>(Constants.MaxFieldCharLength_UShort);
    public ReadOnlySpan<ushort>.Enumerator GetEnumerator() => 
        (fields is null ? ReadOnlySpan<ushort>.Empty : new ReadOnlySpan<ushort>(fields)).GetEnumerator();
}

/// <summary>
/// Represents a template which has <see cref="ushort"/> field types only.
/// </summary>
public readonly record struct UShortTemplate :
    IEquatable<UShortTemplate>,
    IEqualityOperators<UShortTemplate, UShortTemplate, bool>,
    ISpaceTemplate<ushort>, 
    ISpaceMatchable<ushort, UShortTuple>
{
    private readonly ushort?[] fields;

    public ref readonly ushort? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Initializes a template with a single <see langword="null"/> field.
    /// </summary>
    public UShortTemplate() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes a template with a single <see langword="null"/> field.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    public UShortTemplate([AllowNull] params ushort?[] fields)
        => this.fields = fields is null || fields.Length == 0 ? new ushort?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(UShortTuple tuple) => this.Matches<ushort, UShortTuple, UShortTemplate>(tuple);
    public bool Equals(UShortTemplate other) => this.SequentialEquals<ushort, UShortTemplate>(other);

    public override int GetHashCode() => fields?.GetHashCode() ?? 0;
    public override string ToString() => TemplateHelpers.ToString(fields);

    public ReadOnlySpan<ushort?>.Enumerator GetEnumerator() => 
        (fields is null ? ReadOnlySpan<ushort?>.Empty : new ReadOnlySpan<ushort?>(fields)).GetEnumerator();
}
